using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Common;
using SafeMum.Domain.Entities.NutritionHealthTracking;
using SafeMum.Domain.Entities.Users;

namespace SafeMum.Infrastructure.Services
{
    public class AppointmentReminderJob : IReminderJob
    {
        private readonly Supabase.Client _client;
        private readonly IPushNotificationService _notificationService;

        public AppointmentReminderJob(ISupabaseClientFactory clientFactory, IPushNotificationService notificationService)
        {
            _client = clientFactory.GetClient();
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync()
        {
            var tomorrowStart = DateTime.UtcNow.Date.AddDays(1);
            var tomorrowEnd = tomorrowStart.AddDays(1);

            // Get appointments for the entire next day
            var appointmentsResult = await _client
                .From<PrenatalAppointment>()
                .Where(x => x.AppointmentDate >= tomorrowStart && x.AppointmentDate < tomorrowEnd)
                .Get();

            var appointments = appointmentsResult.Models;

            foreach (var appointment in appointments)
            {
                try
                {
                    await SendAppointmentRemindersAsync(appointment.Id);
                }
                catch (Exception ex)
                {
                    // Log properly instead of Console.WriteLine
                    Console.WriteLine($"Error sending reminder for appointment {appointment.Id}: {ex.Message}");
                }
            }
        }
        public async Task SendAppointmentRemindersAsync(Guid appointmentId)
        {
            // Get appointment
            var appointmentResult = await _client
                .From<PrenatalAppointment>()
                .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, appointmentId.ToString())
                .Get();

            var appointment = appointmentResult.Models.FirstOrDefault();
            if (appointment == null) return;

            // Get device token from DeviceToken table (like you do in PrenatalReminderScheduler)
            var tokenResult = await _client
                .From<DeviceToken>()
                .Where(t => t.UserId == appointment.UserId)
                .Get();

            var token = tokenResult.Models.FirstOrDefault()?.Token;
            if (string.IsNullOrEmpty(token)) return;

            // Send notification
            var title = "Appointment Reminder";
            var body = $"You have an appointment with {appointment.DoctorName} tomorrow at {appointment.HospitalNamae}.";
            await _notificationService.SendPushNotification(token, title, body);
        }
    }

}

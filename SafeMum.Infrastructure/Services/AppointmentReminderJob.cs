using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeMum.Application.Interfaces;
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
            var tomorrow = DateTime.UtcNow.Date.AddDays(1);

            // Get all appointments scheduled for tomorrow
            var appointmentsResult = await _client
                .From<PrenatalAppointment>()
                .Filter("appointment_date", Supabase.Postgrest.Constants.Operator.Equals, tomorrow.ToString("yyyy-MM-dd"))
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
                    // You may log this error or handle it as needed
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

            // Get user
            var userResult = await _client
                .From<User>()
                .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, appointment.UserId.ToString())
                .Get();

            var user = userResult.Models.FirstOrDefault();
            if (user == null || string.IsNullOrEmpty(user.DeviceToken)) return;

            // Send notification
            var title = "Appointment Reminder";
            var body = $"You have an appointment with {appointment.DoctorName} tomorrow at {appointment.HospitalNamae}.";
            await _notificationService.SendPushNotification(user.DeviceToken, title, body);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AppointmentReminderJob> _logger;

        public AppointmentReminderJob(
            ISupabaseClientFactory clientFactory,
            IPushNotificationService notificationService,
            ILogger<AppointmentReminderJob> logger)
        {
            _client = clientFactory.GetClient();
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Send24HourReminderAsync(Guid appointmentId)
        {
            await SendReminderAsync(appointmentId, "24-hour reminder",
                "Don't forget your appointment tomorrow with {0} at {1}");
        }

        public async Task Send1HourReminderAsync(Guid appointmentId)
        {
            await SendReminderAsync(appointmentId, "1-hour reminder",
                "Your appointment with {0} is in 1 hour at {1}");
        }

        public async Task Send15MinuteReminderAsync(Guid appointmentId)
        {
            await SendReminderAsync(appointmentId, "Final reminder",
                "Your appointment with {0} starts in 15 minutes at {1}");
        }

        private async Task SendReminderAsync(Guid appointmentId, string title, string bodyTemplate)
        {
            try
            {
                // Get appointment with user info in single query
                var appointmentResult = await _client
                    .From<PrenatalAppointment>()
                    .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, appointmentId.ToString())
                    .Get();

                var appointment = appointmentResult.Models.FirstOrDefault();
                if (appointment == null)
                {
                    _logger.LogWarning($"Appointment {appointmentId} not found");
                    return;
                }

                // Get all device tokens for this user
                var tokensResult = await _client
                    .From<DeviceToken>()
                    .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, appointment.UserId.ToString())
                    .Get();

                var deviceTokens = tokensResult.Models;
                if (deviceTokens == null || !deviceTokens.Any())
                {
                    _logger.LogWarning($"No device tokens found for user {appointment.UserId}");
                    return;
                }

                var body = string.Format(bodyTemplate,
                    appointment.DoctorName,
                    $"{appointment.HospitalNamae} at {appointment.AppointmentDate:MMM dd, h:mm tt}");

                // Send to all user's devices
                var tasks = deviceTokens.Select(async token =>
                {
                    try
                    {
                        await _notificationService.SendPushNotification(token.Token, title, body);
                        _logger.LogInformation($"Notification sent successfully to token ending in ...{token.Token.Substring(token.Token.Length - 8)}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to send notification to token {token.Token}: {ex.Message}");

                        // Handle invalid/expired tokens
                        if (ex.Message.Contains("UNREGISTERED") || ex.Message.Contains("NOT_FOUND"))
                        {
                            await RemoveInvalidToken(token.Id);
                        }
                    }
                });

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendReminderAsync for appointment {appointmentId}: {ex.Message}");
                throw;
            }
        }

        private async Task RemoveInvalidToken(Guid tokenId)
        {
            try
            {
                await _client.From<DeviceToken>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, tokenId.ToString()).Delete();
                _logger.LogInformation($"Removed invalid device token {tokenId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to remove invalid token {tokenId}: {ex.Message}");
            }
        }

        public async Task ExecuteAsync()
        {
            var tomorrow = DateTime.UtcNow.Date.AddDays(1);

            // Get all appointments scheduled for tomorrow
            var appointmentsResult = await _client
                .From<PrenatalAppointment>()
                .Filter("appointmentDate", Supabase.Postgrest.Constants.Operator.Equals, tomorrow.ToString("yyyy-MM-dd"))
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

            // Get device tokens for this user
            var tokensResult = await _client
                .From<DeviceToken>()
                .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, appointment.UserId.ToString())
                .Get();

            var deviceTokens = tokensResult.Models;
            if (deviceTokens == null || !deviceTokens.Any()) return;

            // Send notification to each token
            var title = "Appointment Reminder";
            var body = $"You have an appointment with {appointment.DoctorName} tomorrow at {appointment.HospitalNamae}.";

            foreach (var token in deviceTokens)
            {
                try
                {
                    await _notificationService.SendPushNotification(token.Token, title, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send notification to token {token.Token}: {ex.Message}");
                }
            }
        }
    }
}


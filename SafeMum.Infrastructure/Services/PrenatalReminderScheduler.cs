//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Hosting;
//using SafeMum.Application.Interfaces;
//using SafeMum.Domain.Entities.Common;
//using SafeMum.Domain.Entities.NutritionHealthTracking;

//namespace SafeMum.Infrastructure.Services
//{
//    public class PrenatalReminderScheduler : BackgroundService
//    {
//        private readonly Supabase.Client _client;
//        private readonly IPushNotificationService _pushService;

//        public PrenatalReminderScheduler(ISupabaseClientFactory clientFactory, IPushNotificationService pushService)
//        {
//            _client = clientFactory.GetClient();
//            _pushService = pushService;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                await SendReminders();
//                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
//            }
//        }

//        private async Task SendReminders()
//        {
//            var now = DateTime.UtcNow;
//            var in24h = now.AddHours(24);

//            var appointments = await _client
//                .From<PrenatalAppointment>()
//                .Where(x => x.AppointmentDate <= in24h && x.AppointmentDate > now)
//                .Get();

//            foreach (var appt in appointments.Models)
//            {
//                // Get Device Token
//                var tokenResult = await _client
//                    .From<DeviceToken>()
//                    .Where(t => t.UserId == appt.UserId)
//                    .Get();

//                var token = tokenResult.Models.FirstOrDefault()?.Token;
//                if (token == null) continue;

//                var message = $"Reminder: You have an appointment with Dr. {appt.DoctorName} at {appt.AppointmentDate:hh:mm tt}";
//                await _pushService.SendPushNotification(token, "Prenatal Appointment Reminder", message);
//            }
//        }
//    }

//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.NutritionHealthTracking;
using SafeMum.Infrastructure.Services;

namespace SafeMum.Application.Features.NutritionHealthTracking.PrenatalAppointments.AddPrenatalAppointment
{
    public class AddPrenatalAppointmentHandler : IRequestHandler<AddPrenatalAppointmentRequest, Result>
    {

        private readonly Supabase.Client _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBackgroundJobClient _backgroundJobs;

        private readonly IPushNotificationService _notificationService;
        private readonly IReminderJob _reminderJob;
        private readonly IInAppNotificationService _inAppNotificationService;



        public AddPrenatalAppointmentHandler(ISupabaseClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IPushNotificationService notificationService, IBackgroundJobClient backgroundJobs, IReminderJob reminderJob, IInAppNotificationService inAppNotificationService)
        {
            _client = clientFactory.GetClient();
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
            _backgroundJobs = backgroundJobs;
            _reminderJob = reminderJob;
            _inAppNotificationService = inAppNotificationService;
        }
        public async Task<Result> Handle(AddPrenatalAppointmentRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userid = Guid.Parse(userId);

            // Combine date and time
            var appointmentDateTime = request.AppointmentDate.Date + request.AppointmentTime;

            var prenatalAppoint = new PrenatalAppointment
            {
                Id = Guid.NewGuid(),
                UserId = userid,
                DoctorName = request.DoctorName,
                HospitalNamae = request.HospitalNamae,
                AppointmentDate = appointmentDateTime,  // Store full datetime
                Location = request.Location,
            };

            await _client.From<PrenatalAppointment>().Insert(prenatalAppoint);



            await _inAppNotificationService.CreateNotificationAsync(
        userid,
        "Appointment Scheduled",
        $"Your appointment with Dr. {request.DoctorName} has been scheduled for {appointmentDateTime:MMM dd, yyyy 'at' h:mm tt}",
        "appointment",
        new { appointmentId = prenatalAppoint.Id }
    );

            // Schedule multiple reminders
            var appointmentDate = prenatalAppoint.AppointmentDate;

            // 24 hours before
            var reminderTime24h = appointmentDate.AddHours(-24);
            if (reminderTime24h > DateTime.UtcNow)
            {
                _backgroundJobs.Schedule<AppointmentReminderJob>(
                    job => job.Send24HourReminderAsync(prenatalAppoint.Id),
                    reminderTime24h
                );
            }

            // 1 hour before  
            var reminderTime1h = appointmentDate.AddHours(-1);
            if (reminderTime1h > DateTime.UtcNow)
            {
                _backgroundJobs.Schedule<AppointmentReminderJob>(
                    job => job.Send1HourReminderAsync(prenatalAppoint.Id),
                    reminderTime1h
                );
            }

            // 15 minutes before
            var reminderTime15m = appointmentDate.AddMinutes(-15);
            if (reminderTime15m > DateTime.UtcNow)
            {
                _backgroundJobs.Schedule<AppointmentReminderJob>(
                    job => job.Send15MinuteReminderAsync(prenatalAppoint.Id),
                    reminderTime15m
                );
            }

            return Result.Success();
        }
    }
}

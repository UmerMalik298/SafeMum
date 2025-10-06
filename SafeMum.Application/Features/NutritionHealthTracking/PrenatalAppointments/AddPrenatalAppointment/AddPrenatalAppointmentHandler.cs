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
        private readonly IPushNotificationService _notificationService; // not used here, but kept if you're injecting it
        private readonly IReminderJob _reminderJob;
        private readonly IInAppNotificationService _inAppNotificationService;

        public AddPrenatalAppointmentHandler(
            ISupabaseClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor,
            IPushNotificationService notificationService,
            IBackgroundJobClient backgroundJobs,
            IReminderJob reminderJob,
            IInAppNotificationService inAppNotificationService)
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
            // 1) Resolve user id from auth
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Result.Failure("User not authenticated.");

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Result.Failure("Invalid user id.");

            // 2) Combine date & time, normalize to UTC for storage & scheduling
            var localCombined = request.AppointmentDate.Date + request.AppointmentTime;
            var localDateTime = DateTime.SpecifyKind(localCombined, DateTimeKind.Local);
            var appointmentUtc = localDateTime.ToUniversalTime();

            // 3) Create & save appointment (storing UTC is recommended)
            var appt = new PrenatalAppointment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DoctorName = request.DoctorName,
                HospitalNamae = request.HospitalNamae,          // consider renaming to HospitalName later
                AppointmentDate = appointmentUtc,               // store UTC
                Location = request.Location
            };

            await _client.From<PrenatalAppointment>().Insert(appt);

            // 4) In-app notification (DB + SignalR broadcast via your service/gateway)
            await _inAppNotificationService.CreateNotificationAsync(
                userId,
                "Appointment Scheduled",
                $"Your appointment with Dr. {request.DoctorName} has been scheduled for {localDateTime:MMM dd, yyyy 'at' h:mm tt}.",
                "appointment",
                new { appointmentId = appt.Id }
            );

            // 5) Schedule reminders via Hangfire on the IReminderJob interface
            var nowUtc = DateTimeOffset.UtcNow;

            var at24h = new DateTimeOffset(appointmentUtc.AddHours(-24), TimeSpan.Zero);
            var at1h = new DateTimeOffset(appointmentUtc.AddHours(-1), TimeSpan.Zero);
            var at15m = new DateTimeOffset(appointmentUtc.AddMinutes(-15), TimeSpan.Zero);

            if (at24h > nowUtc)
            {
                _backgroundJobs.Schedule<IReminderJob>(
                    job => job.Send24HourReminderAsync(appt.Id),
                    at24h
                );
            }

            if (at1h > nowUtc)
            {
                _backgroundJobs.Schedule<IReminderJob>(
                    job => job.Send1HourReminderAsync(appt.Id),
                    at1h
                );
            }

            if (at15m > nowUtc)
            {
                _backgroundJobs.Schedule<IReminderJob>(
                    job => job.Send15MinuteReminderAsync(appt.Id),
                    at15m
                );
            }

            return Result.Success();
        }
    }
}


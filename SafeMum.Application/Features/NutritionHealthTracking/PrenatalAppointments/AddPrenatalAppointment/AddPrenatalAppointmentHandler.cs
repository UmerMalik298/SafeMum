﻿using System;
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
        //private readonly FirebaseNotificationService _firebaseService;


        public AddPrenatalAppointmentHandler(ISupabaseClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = clientFactory.GetClient();
            _httpContextAccessor = httpContextAccessor;
            
        }
        public async Task<Result> Handle(AddPrenatalAppointmentRequest request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userid = Guid.Parse(userId);
            var prenatalAppoint = new PrenatalAppointment
            {
                Id  = new Guid(),
                UserId = userid,
                DoctorName = request.DoctorName,
                HospitalNamae = request.HospitalNamae,
                AppointmentDate = request.AppointmentDate,  

                Location = request.Location,
            };


            await _client.From<PrenatalAppointment>().Insert(prenatalAppoint);

            return Result.Success();
        }
    }
}

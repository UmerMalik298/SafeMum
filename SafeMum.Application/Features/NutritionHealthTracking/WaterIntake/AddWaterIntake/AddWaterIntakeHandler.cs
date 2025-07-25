﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.NutritionHealthTracking;

namespace SafeMum.Application.Features.NutritionHealthTracking.WaterIntake.AddWaterIntake
{
    public class AddWaterIntakeHandler : IRequestHandler<AddWaterIntakeRequest, Result>
    {
        private readonly Supabase.Client _client;
        private readonly IHttpContextAccessor _contextAccessor;
        public AddWaterIntakeHandler(ISupabaseClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _client = clientFactory.GetClient();
            _contextAccessor = contextAccessor;
        }
        public async Task<Result> Handle(AddWaterIntakeRequest request, CancellationToken cancellationToken)
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var twentyFourHoursAgo = DateTime.UtcNow.AddHours(-24);

           var  userid = Guid.Parse(userId);

            var existingLog = await _client
                .From<WaterIntakeLog>()
                .Where(x => x.UserId == userid && x.ConsumedAt >= twentyFourHoursAgo)
                .Single();

            if (existingLog != null)
                return Result.Failure("You have already added a water intake log in the last 24 hours.");


            var result = new WaterIntakeLog
            {
                Id = new Guid(),
                UserId = Guid.Parse(userId),
                AmountInMl = request.AmountInMl,
                ConsumedAt = DateTime.UtcNow,            
            };
            await _client.From<WaterIntakeLog>().Insert(result);


            return Result.Success();
        }
    }
}

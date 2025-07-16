using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.NutritionHealthTracking;
using SafeMum.Infrastructure.Services;

namespace SafeMum.Application.Features.NutritionHealthTracking.Supplement.AddSupplement
{
    public class AddSupplementHandler : IRequestHandler<AddSupplementRequest, Result>
    {
        private readonly Supabase.Client _client;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddSupplementHandler(ISupabaseClientFactory clientFactory, IHttpContextAccessor contextAccessor)
        {
            _client = clientFactory.GetClient();
            _contextAccessor = contextAccessor;
            
        }
        public async Task<Result> Handle(AddSupplementRequest request, CancellationToken cancellationToken)
        {

            var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userid = Guid.Parse(userId);

            var twentyFourHoursAgo = DateTime.UtcNow.AddHours(-24);

            var existingLog = await _client
             .From<SupplementLog>()
             .Where(x => x.UserId == userid && x.TakenAt >= twentyFourHoursAgo)
             .Single();

            if (existingLog != null)
                return Result.Failure("You have already added a supplement intake log in the last 24 hours.");


            var result = new SupplementLog
            {
                Id = new Guid(),
                UserId = Guid.Parse(userId),
                Name = request.Name,
                Dosage = request.Dosage 
            };
            await _client.From<SupplementLog>().Insert(result);


            return Result.Success();
          
        }
    }
}

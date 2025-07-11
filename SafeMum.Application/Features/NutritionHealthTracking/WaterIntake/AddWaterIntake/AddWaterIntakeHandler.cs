using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;

namespace SafeMum.Application.Features.NutritionHealthTracking.WaterIntake.AddWaterIntake
{
    public class AddWaterIntakeHandler : IRequestHandler<AddWaterIntakeRequest, Result>
    {
        private readonly Supabase.Client _client;
        public AddWaterIntakeHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public Task<Result> Handle(AddWaterIntakeRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

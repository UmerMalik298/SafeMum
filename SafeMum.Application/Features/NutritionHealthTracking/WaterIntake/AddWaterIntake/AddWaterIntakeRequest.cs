using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.NutritionHealthTracking.WaterIntake.AddWaterIntake
{
    public class AddWaterIntakeRequest : IRequest<Result>
    {
        public int AmountInMl { get; set; }
    }

}

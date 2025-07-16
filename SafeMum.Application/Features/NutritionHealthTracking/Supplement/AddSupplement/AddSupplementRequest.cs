using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.NutritionHealthTracking.Supplement.AddSupplement
{
    public class AddSupplementRequest : IRequest<Result>
    {
        public string Name { get; set; }

        public string Dosage { get; set; }
    }
}

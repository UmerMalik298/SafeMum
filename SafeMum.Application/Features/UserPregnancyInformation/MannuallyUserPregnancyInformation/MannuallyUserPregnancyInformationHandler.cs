using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.PregnancyInformation;

namespace SafeMum.Application.Features.UserPregnancyInformation.MannuallyUserPregnancyInformation
{
    public class MannuallyUserPregnancyInformationHandler : IRequestHandler<MannuallyUserPregnancyInformationRequest, Result>
    {


        private readonly Supabase.Client _client;

        public MannuallyUserPregnancyInformationHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
        }
        public async Task<Result> Handle(MannuallyUserPregnancyInformationRequest request, CancellationToken cancellationToken)
        {
            var userPregnancyInfo = new UserPregnancyInfo
            {

                Id = Guid.NewGuid(),

               // UserId = Guid.NewGuid(),
                IsCurrentlyPregnant = request.CurrentlyPregnant,
                EDD = request.EDD.HasValue
                       ? request.EDD.Value.ToDateTime(TimeOnly.MinValue)
                       : null,
                Gravida = request.NoOfPreviousPregnancies,
                Parity = request.NoOfLiveBirths,
                EmergencyContactName = request.EmergencyContactName,
                EmergencyContactNumber = request.EmergencyContactNumber,
                CreatedAt = DateTime.UtcNow,
                IsDiabetic = request.IsDiabetic,
                IsSmoker = request.IsSmoker,
                HasHypertension = request.HasHypertension,
                BloodGroup = request.BloodGroup,
                TakesMedication = request.TakesMedication ?? new List<string>(),


            };

            var response = await _client
                .From<UserPregnancyInfo>()
                .Insert(userPregnancyInfo);

            return Result.Success();
        }
    }
}

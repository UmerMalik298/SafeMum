using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.PregnancyInformation;
using SafeMum.Domain.Entities.Users;

namespace SafeMum.Application.Features.UserPregnancyInformation
{
    public class UserPregnancyInformationHandler : IRequestHandler<UserPregnancyInformationRequest, Result>
    {
        private readonly Supabase.Client _client; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserPregnancyInformationHandler(ISupabaseClientFactory supabaseClient, IHttpContextAccessor httpContextAccessor)
        {
            _client = supabaseClient.GetClient();
            _httpContextAccessor = httpContextAccessor;
            
        }
        public async Task<Result> Handle(UserPregnancyInformationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                

                if (string.IsNullOrWhiteSpace(userId))
                    return Result.Failure("User is Not Authenticated");


                var userPregnancyInfo = new UserPregnancyInfo
                {

                    Id = Guid.NewGuid(),

                    UserId = userId ,
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
            catch (Exception ex)
            {
                return Result.Failure($"Failed to save pregnancy information: {ex.Message}");
            }
        }
    }
    }


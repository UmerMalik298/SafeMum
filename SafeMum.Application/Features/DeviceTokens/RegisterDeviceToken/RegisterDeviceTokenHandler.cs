using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Common;
using SafeMum.Domain.Entities.Users;


namespace SafeMum.Application.Features.DeviceTokens.RegisterDeviceToken
{
    public class RegisterDeviceTokenHandler : IRequestHandler<RegisterDeviceTokenRequest, Result>
    {
        private readonly Supabase.Client _client;

        public RegisterDeviceTokenHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
        }

        public async Task<Result> Handle(RegisterDeviceTokenRequest request, CancellationToken cancellationToken)
        {

            var userId = await _client.From<User>().Single();
            if (userId == null)
            {
                return Result.Failure("User Not Found");
            }
            var tokenRecord = new DeviceToken
            {
                Id = Guid.NewGuid(), // Generate a proper new Guid
                UserId = request.UserId,
                Token = request.DeviceToken,
                CreatedAt = DateTime.UtcNow
            };

            var response = await _client.From<DeviceToken>().Insert(tokenRecord);

            if (response == null || response.Models == null || response.Models.Count == 0)
                return Result.Failure("Token registration failed.");

            return Result.Success();
        }

    }

}

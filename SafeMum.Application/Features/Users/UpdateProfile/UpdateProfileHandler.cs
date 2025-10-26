using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Users;

namespace SafeMum.Application.Features.Users.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileRequest, Result>
    {
        private readonly Supabase.Client _client;
        private readonly IImageUploadService _imageUploadService;

        public UpdateProfileHandler(ISupabaseClientFactory clientFactory, IImageUploadService imageUploadService)
        {
            _client = clientFactory.GetClient();
            _imageUploadService = imageUploadService;
        }

        public async Task<Result> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // 1️⃣ Get user
                var user = await _client
                    .From<User>()
                    .Where(x => x.Id == request.UserId)
                    .Single();

                if (user == null)
                    return Result.Failure("User not found.");

             
                string? profileUrl = user.ProfileUrl;
                if (request.ProfileImage != null)
                {
                    var uploadedUrl = await _imageUploadService.UploadImageAsync(request.ProfileImage);
                    if (!string.IsNullOrEmpty(uploadedUrl))
                        profileUrl = uploadedUrl;
                }

               
                if (!string.IsNullOrEmpty(request.PhoneNo))
                    user.PhoneNo = request.PhoneNo;

                if (!string.IsNullOrEmpty(request.Address))
                    user.Address = request.Address;

                if (!string.IsNullOrEmpty(profileUrl))
                    user.ProfileUrl = profileUrl;

                user.UpdatedAt = DateTime.UtcNow;

          
                await _client
                    .From<User>()
                    .Update(user);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating profile: {ex.Message}");
            }
        }
    }

}

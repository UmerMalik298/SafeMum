using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Communication;
using SafeMum.Domain.Entities.WeeklyPregnancyProfile;

namespace SafeMum.Application.Features.Communication.VoiceMessage
{
    public class SendVoiceMessageHandler : IRequestHandler<SendVoiceMessageRequest, Result>
    {
        private readonly Supabase.Client _client;
        private readonly IVoiceUploadService _VoiceUploadService;

        public SendVoiceMessageHandler(ISupabaseClientFactory clientFactory, IVoiceUploadService VoiceUploadService)
        {
            _client = clientFactory.GetClient();
            _VoiceUploadService = VoiceUploadService;


        }
        public async Task<Result> Handle(SendVoiceMessageRequest request, CancellationToken cancellationToken)
        {
            var fileUrl = await _VoiceUploadService.UploadVoiceAsync(request.VoiceMessage);

            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId ?? Guid.Empty,
                GroupId = request.GroupId,
                Content = fileUrl,
                MessageType = "Audio",
                SendAt = DateTime.UtcNow,
               
            
            };


            var response = await _client
                .From<Message>()
                .Insert(message);

            return Result.Success();


         
        }
    }
}

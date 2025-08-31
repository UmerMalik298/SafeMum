using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SafeMum.Application.Common;

namespace SafeMum.Application.Features.Communication.VoiceMessage
{
    public class SendVoiceMessageRequest : IRequest<Result>
    {
        [FromForm] public Guid SenderId { get; set; }

        [FromForm] public Guid? ReceiverId { get; set; }

        [FromForm] public Guid? GroupId { get; set; }

        [FromForm] public IFormFile VoiceMessage { get; set; }




    }
}

using MediatR;
using SafeMum.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification.GetAllInAppNotifications
{
    public class GetAllInAppNotificationsHandler : IRequestHandler<GetAllInAppNotificationsRequest, List<GetAllInAppNotificationsResponse>>
    {
        private readonly Supabase.Client _client;
        public GetAllInAppNotificationsHandler(ISupabaseClientFactory clientFactory)
        {
            _client = clientFactory.GetClient();
            
        }
        public Task<List<GetAllInAppNotificationsResponse>> Handle(GetAllInAppNotificationsRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

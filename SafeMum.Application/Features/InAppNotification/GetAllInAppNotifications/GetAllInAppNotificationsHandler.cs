using MediatR;
using Microsoft.AspNetCore.Http;
using SafeMum.Application.Interfaces;
using Supabase.Postgrest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.InAppNotification.GetAllInAppNotifications
{
    public class GetAllInAppNotificationsHandler : IRequestHandler<GetAllInAppNotificationsRequest, List<GetAllInAppNotificationsResponse>>
    {
        private readonly Supabase.Client _client;
        private readonly IHttpContextAccessor _http;

        public GetAllInAppNotificationsHandler(ISupabaseClientFactory clientFactory, IHttpContextAccessor http)
        {
            _client = clientFactory.GetClient();
            _http = http;
        }

        public async Task<List<GetAllInAppNotificationsResponse>> Handle(
            GetAllInAppNotificationsRequest request, CancellationToken cancellationToken)
        {
            // get current user id from JWT
            var userIdStr = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdStr))
                return new List<GetAllInAppNotificationsResponse>(); // or throw/return error type as you prefer

            var userId = Guid.Parse(userIdStr);

            // Query Supabase table for this user's notifications
            // NOTE: Use the fully-qualified entity type to avoid the namespace collision.
            var res = await _client
                .From<SafeMum.Domain.Entities.AppNotification.InAppNotification>()
                .Filter("userid", Constants.Operator.Equals, userId.ToString())       // adjust to "user_id" if that's your column
                .Order("created_at", Constants.Ordering.Descending)
                .Range((request.Page - 1) * request.PageSize, request.Page * request.PageSize - 1)
                .Get();

            // Map to your response
            var list = res.Models
                .Select(n => new GetAllInAppNotificationsResponse
                {
                    Title = n.Title,
                    Message = n.Message
                })
                .ToList();

            return list;
        }
    }
}


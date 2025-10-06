using MediatR;

namespace SafeMum.API.EndPoints
{
    internal class GetInAppNotificationsReqeust : IRequest<object?>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
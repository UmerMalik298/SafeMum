using MediatR;
using SafeMum.Application.Common;
using SafeMum.Application.Features.DeviceTokens.TestNotification;
using SafeMum.Application.Interfaces;
using SafeMum.Domain.Entities.Common;
using SafeMum.Infrastructure.Services;
using Supabase;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class TestNotificationHandler : IRequestHandler<TestNotificationRequest, Result>
{
    private readonly Supabase.Client _client;
    private readonly IPushNotificationService _notificationService;

    public TestNotificationHandler(ISupabaseClientFactory clientFactory, IPushNotificationService notificationService)
    {
        _client = clientFactory.GetClient();
        _notificationService = notificationService;
    }

    public async Task<Result> Handle(TestNotificationRequest request, CancellationToken cancellationToken)
    {
        // Get device token from DB
        var deviceResult = await _client
            .From<DeviceToken>()
            .Filter("userid", Supabase.Postgrest.Constants.Operator.Equals, request.UserId.ToString())
            .Get();

        var device = deviceResult.Models.FirstOrDefault();
        if (device == null || string.IsNullOrEmpty(device.Token))
            return Result.Failure("Device token not found for this user.");

        try
        {
            // Try to send notification
            await _notificationService.SendPushNotification(
                deviceToken: device.Token,
                title: "Hello",
                body: "This is a v1 API test!"
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
          
            if (ex.Message.Contains("\"errorCode\": \"UNREGISTERED\""))
            {
                
                await _client
                    .From<DeviceToken>()
                    .Where(x => x.Id == device.Id)
                    .Delete();

                return Result.Failure("Device token was unregistered and has been removed.");
            }

           
            throw;
        }
    }


}

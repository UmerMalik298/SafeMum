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

        Console.WriteLine($"Found token for user {request.UserId}: {device.Token.Substring(0, 10)}...");

        try
        {
            // Try to send notification
            await _notificationService.SendPushNotification(
                deviceToken: device.Token,
                title: "Hello",
                body: "This is a v1 API test!"
            );

            Console.WriteLine("Test notification sent successfully!");
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending test notification: {ex.Message}");

            if (ex.Message.Contains("\"errorCode\": \"UNREGISTERED\""))
            {
                Console.WriteLine("Token is unregistered, removing from database...");

                await _client
                    .From<DeviceToken>()
                    .Where(x => x.Id == device.Id)
                    .Delete();

                return Result.Failure("Device token was unregistered and has been removed.");
            }

            // Add more specific error handling
            if (ex.Message.Contains("401") || ex.Message.Contains("Unauthorized"))
            {
                return Result.Failure("Firebase authentication failed. Check service account credentials.");
            }

            if (ex.Message.Contains("404") || ex.Message.Contains("Not Found"))
            {
                return Result.Failure("Firebase project not found. Check project ID.");
            }

            return Result.Failure($"Failed to send test notification: {ex.Message}");
        }
    }


}

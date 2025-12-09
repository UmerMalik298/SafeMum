using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMum.Application.Features.Users.ResetPasswordRedirect
{
    public class ResetPasswordRedirectHandler : IRequestHandler<ResetPasswordRedirectRequest, ResetPasswordRedirectResponse>
    {
        public Task<ResetPasswordRedirectResponse> Handle(ResetPasswordRedirectRequest request, CancellationToken cancellationToken)
        {
            var html = @"
<!DOCTYPE html>
<html>
<head>
    <title>Redirecting to SafeMum App...</title>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <style>
        /* Your existing styles */
    </style>
</head>
<body>
    <div class='container'>
        <h2>Redirecting to SafeMum App...</h2>
        <div class='spinner'></div>
        <p id='status'>Please wait while we redirect you to the app.</p>
        <div id='error' style='display: none;' class='error'>
            <p>Unable to automatically open the app.</p>
            <a href='#' id='manualLink' class='manual-link'>Open SafeMum App Manually</a>
        </div>
        <p style='font-size: 0.8rem; margin-top: 2rem; opacity: 0.7;'>
            If nothing happens, make sure you have the SafeMum app installed.
        </p>
    </div>

    <script>
        // Parameters passed from server
        const params = {
            access_token: '" + request.AccessToken + @"',
            refresh_token: '" + request.RefreshToken + @"',
            expires_at: '" + request.ExpiresAt + @"',
            token_type: '" + request.TokenType + @"'
        };

        function redirectToApp() {
            if (!params.access_token) {
                document.getElementById('status').textContent = 'Invalid or expired reset link.';
                document.getElementById('error').style.display = 'block';
                return;
            }

            // Build deep link URL for React Native app
            const queryParams = new URLSearchParams({
                access_token: params.access_token,
                refresh_token: params.refresh_token || '',
                expires_at: params.expires_at || '',
                token_type: params.token_type || 'bearer'
            }).toString();
            
            const deepLink = 'safemum://reset-password?' + queryParams;
            
            console.log('Attempting to open:', deepLink);
            
            // First, try to open the app
            window.location.href = deepLink;
            
            // If app isn't installed, show manual option after delay
            setTimeout(function() {
                document.getElementById('error').style.display = 'block';
                document.getElementById('manualLink').href = deepLink;
                document.getElementById('status').textContent = 'App not detected. Click the button above to open manually.';
                
                // Optional: Redirect to app store if app not installed
                // setTimeout(() => {
                //     if (confirm('SafeMum app not found. Would you like to install it?')) {
                //         // Redirect to App Store or Play Store
                //         window.location.href = 'https://apps.apple.com/app/idYOUR_APP_ID'; // iOS
                //         // window.location.href = 'https://play.google.com/store/apps/details?id=YOUR_PACKAGE_NAME'; // Android
                //     }
                // }, 1000);
            }, 2500);
        }

        // Execute on page load
        window.onload = redirectToApp;
    </script>
</body>
</html>";

            return Task.FromResult(new ResetPasswordRedirectResponse { HtmlContent = html });
        }
    }
}

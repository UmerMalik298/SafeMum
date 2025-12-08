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
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            margin: 0;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }
        .container {
            text-align: center;
            padding: 2rem;
            background: rgba(255, 255, 255, 0.1);
            border-radius: 12px;
            backdrop-filter: blur(10px);
            max-width: 400px;
        }
        .spinner {
            border: 4px solid rgba(255, 255, 255, 0.3);
            border-radius: 50%;
            border-top: 4px solid white;
            width: 40px;
            height: 40px;
            animation: spin 1s linear infinite;
            margin: 20px auto;
        }
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        .error {
            background: rgba(255, 59, 48, 0.2);
            padding: 1rem;
            border-radius: 8px;
            margin-top: 1rem;
        }
        .manual-link {
            margin-top: 1rem;
            padding: 0.75rem 1.5rem;
            background: white;
            color: #667eea;
            text-decoration: none;
            border-radius: 6px;
            display: inline-block;
            font-weight: 600;
        }
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
    </div>

    <script>
        function getFragmentParams() {
            const hash = window.location.hash.substring(1);
            const params = new URLSearchParams(hash);
            return {
                access_token: params.get('access_token'),
                refresh_token: params.get('refresh_token'),
                expires_at: params.get('expires_at'),
                expires_in: params.get('expires_in'),
                type: params.get('type')
            };
        }

        function redirectToApp() {
            const params = getFragmentParams();
            
            if (!params.access_token || !params.refresh_token) {
                document.getElementById('status').textContent = 'Invalid reset link.';
                document.getElementById('error').style.display = 'block';
                return;
            }

            // Build deep link for your React Native app
            const deepLink = 'safemum://reset-password?access_token=' + encodeURIComponent(params.access_token) + 
                           '&refresh_token=' + encodeURIComponent(params.refresh_token) + 
                           '&expires_at=' + params.expires_at;
            
            // Try to open the app
            window.location.href = deepLink;
            
            // Show manual option after 2 seconds if automatic redirect fails
            setTimeout(function() {
                document.getElementById('error').style.display = 'block';
                document.getElementById('manualLink').href = deepLink;
            }, 2000);
        }

        // Execute on page load
        redirectToApp();
    </script>
</body>
</html>";

            var response = new ResetPasswordRedirectResponse
            {
                HtmlContent = html
            };

            return Task.FromResult(response);
        }
    }
}

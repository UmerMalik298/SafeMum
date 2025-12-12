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
            // Log received parameters
            Console.WriteLine($"🔗 Token Hash: {request.TokenHash?.Substring(0, Math.Min(20, request.TokenHash?.Length ?? 0))}...");
            Console.WriteLine($"🔖 Type: {request.Type}");

            // Build HTML that will exchange token_hash for session and redirect to app
            var html = @"
<!DOCTYPE html>
<html>
<head>
    <title>Redirecting to SafeMum App...</title>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <meta charset='UTF-8'>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', 'Cantarell', sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }
        .container {
            background: white;
            border-radius: 20px;
            padding: 40px;
            max-width: 450px;
            width: 100%;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            text-align: center;
        }
        h2 {
            color: #333;
            margin-bottom: 20px;
            font-size: 24px;
            font-weight: 600;
        }
        .spinner {
            border: 4px solid #f3f3f3;
            border-top: 4px solid #667eea;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
            margin: 30px auto;
        }
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        #status {
            color: #666;
            margin: 20px 0;
            font-size: 16px;
            line-height: 1.5;
        }
        .error {
            background: #fee;
            border: 1px solid #fcc;
            border-radius: 8px;
            padding: 20px;
            margin-top: 20px;
            display: none;
        }
        .error p {
            color: #c33;
            margin-bottom: 15px;
            font-weight: 500;
        }
        .manual-link {
            display: inline-block;
            background: #667eea;
            color: white;
            padding: 12px 30px;
            border-radius: 8px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s ease;
            cursor: pointer;
        }
        .manual-link:hover {
            background: #5568d3;
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
        }
        .footer-text {
            font-size: 0.85rem;
            margin-top: 2rem;
            opacity: 0.7;
            color: #666;
        }
        .debug {
            background: #f5f5f5;
            border-radius: 8px;
            padding: 15px;
            margin-top: 20px;
            font-size: 11px;
            font-family: 'Courier New', monospace;
            color: #666;
            text-align: left;
            max-height: 250px;
            overflow-y: auto;
            display: none;
            word-break: break-all;
        }
    </style>
    <script src='https://cdn.jsdelivr.net/npm/@supabase/supabase-js@2'></script>
</head>
<body>
    <div class='container'>
        <h2>🔐 Verifying Reset Link</h2>
        <div class='spinner' id='spinner'></div>
        <p id='status'>Please wait while we verify your password reset request...</p>
        <div id='error' class='error'>
            <p id='errorMessage'>Unable to verify reset link.</p>
            <a href='#' id='manualLink' class='manual-link'>Try Again</a>
        </div>
        <p class='footer-text'>
            If nothing happens, make sure you have the SafeMum app installed.
        </p>
        <div id='debug' class='debug'></div>
    </div>

    <script>
        // Supabase configuration - REPLACE WITH YOUR ACTUAL VALUES
        const SUPABASE_URL = 'YOUR_SUPABASE_URL'; // e.g., 'https://xxxxx.supabase.co'
        const SUPABASE_ANON_KEY = 'YOUR_SUPABASE_ANON_KEY';

        function log(message, isError = false) {
            const prefix = isError ? '❌' : '✅';
            const fullMessage = prefix + ' ' + message;
            console.log(fullMessage);
            
            const debugEl = document.getElementById('debug');
            debugEl.style.display = 'block';
            const line = document.createElement('div');
            line.textContent = fullMessage;
            if (isError) line.style.color = '#c33';
            debugEl.appendChild(line);
            debugEl.scrollTop = debugEl.scrollHeight;
        }

        function showError(message) {
            document.getElementById('spinner').style.display = 'none';
            document.getElementById('status').textContent = message;
            document.getElementById('error').style.display = 'block';
            document.getElementById('errorMessage').textContent = message;
            log(message, true);
        }

        async function verifyAndRedirect() {
            try {
                log('🚀 Starting password reset verification...');
                
                // Get token_hash and type from URL query parameters
                const urlParams = new URLSearchParams(window.location.search);
                const tokenHash = urlParams.get('token_hash');
                const type = urlParams.get('type');

                log('Token Hash: ' + (tokenHash ? tokenHash.substring(0, 20) + '...' : 'MISSING'));
                log('Type: ' + (type || 'MISSING'));

                if (!tokenHash || !type) {
                    showError('Invalid reset link - missing verification token.');
                    return;
                }

                // Verify the type is 'recovery'
                if (type !== 'recovery') {
                    showError('Invalid link type. Expected password recovery link.');
                    return;
                }

                // Initialize Supabase client
                const supabase = window.supabase.createClient(SUPABASE_URL, SUPABASE_ANON_KEY);
                log('Supabase client initialized');

                document.getElementById('status').textContent = 'Verifying your reset link...';

                // Exchange token_hash for session using verifyOtp
                log('Exchanging token for session...');
                const { data, error } = await supabase.auth.verifyOtp({
                    token_hash: tokenHash,
                    type: 'recovery'
                });

                if (error) {
                    log('Verification error: ' + error.message, true);
                    showError('Invalid or expired reset link. Please request a new one.');
                    return;
                }

                if (!data.session) {
                    log('No session returned', true);
                    showError('Failed to create session. Please try again.');
                    return;
                }

                log('Session created successfully!');
                log('Access Token: ' + data.session.access_token.substring(0, 20) + '...');
                log('Refresh Token: ' + data.session.refresh_token.substring(0, 20) + '...');

                // Build deep link with session tokens
                const deepLinkParams = new URLSearchParams({
                    access_token: data.session.access_token,
                    refresh_token: data.session.refresh_token,
                    expires_at: data.session.expires_at.toString(),
                    expires_in: data.session.expires_in.toString(),
                    token_type: 'bearer',
                    type: 'recovery'
                });

                const deepLink = 'safemum://reset-password?' + deepLinkParams.toString();
                
                log('Deep link created successfully');
                document.getElementById('status').textContent = 'Opening SafeMum app...';

                // Try to open the app
                window.location.href = deepLink;
                log('Redirect initiated');

                // Fallback if app doesn't open
                setTimeout(() => {
                    document.getElementById('spinner').style.display = 'none';
                    document.getElementById('error').style.display = 'block';
                    document.getElementById('manualLink').href = deepLink;
                    document.getElementById('manualLink').textContent = 'Open SafeMum App Manually';
                    document.getElementById('status').textContent = 'App not detected. Click the button to try again.';
                    log('Timeout reached - showing manual option', true);
                }, 3000);

            } catch (err) {
                log('Unexpected error: ' + err.message, true);
                showError('An unexpected error occurred. Please try again.');
            }
        }

        // Start verification when page loads
        window.addEventListener('load', () => {
            log('📄 Page loaded');
            setTimeout(verifyAndRedirect, 500);
        });
    </script>
</body>
</html>";

            return Task.FromResult(new ResetPasswordRedirectResponse { HtmlContent = html });
        }
    }
}

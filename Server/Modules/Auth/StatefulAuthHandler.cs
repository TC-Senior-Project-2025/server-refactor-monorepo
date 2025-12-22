using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Server.Modules.Auth;

/// <summary>
/// Custom authentication handler for stateful token-based authentication.
/// Supports authentication via Bearer token, cookie, or query parameter.
/// </summary>
public class StatefulAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IServiceProvider serviceProvider)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    /// <summary>
    /// Handles the authentication process by validating tokens from various sources.
    /// </summary>
    /// <returns>The authentication result.</returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? tokenString = null;

        // 1. Check Authorization Header (Bearer)
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            tokenString = authHeader["Bearer ".Length..].Trim();
        }

        // 2. Check Cookie
        if (string.IsNullOrEmpty(tokenString) && Request.Cookies.TryGetValue("access_token", out var cookieToken))
        {
            tokenString = cookieToken;
        }

        // 3. Check Query Parameter (for WebSockets or links)
        if (string.IsNullOrEmpty(tokenString) && Request.Query.TryGetValue("token", out var queryToken))
        {
            tokenString = queryToken;
        }

        if (string.IsNullOrEmpty(tokenString))
        {
            return AuthenticateResult.Fail("Missing Authorization Header or Query Parameter");
        }

        if (!Guid.TryParse(tokenString, out var token))
        {
            return AuthenticateResult.Fail("Invalid Token Format");
        }

        try
        {
            // Resolve AuthService from scoped provider
            using var scope = serviceProvider.CreateScope();
            var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

            var user = await authService.ValidateToken(token);
            if (user == null)
            {
                return AuthenticateResult.Fail("Invalid or Expired Token");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("TokenId", token.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail($"Authentication Logic Error: {ex.Message}");
        }
    }
}

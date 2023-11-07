using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Utapoi.MusicQuiz.Infrastructure.Identity;

public sealed class UtapoiJwtBearerHandler : JwtBearerHandler
{
    private readonly HttpClient _httpClient;
    public UtapoiJwtBearerHandler(
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<JwtBearerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock)
    {
        _httpClient = httpClientFactory.CreateClient("UtapoiHttpClient");
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Cookies.TryGetValue("Utapoi-Token", out var uToken))
        {
            return AuthenticateResult.Fail("Authorization header not found.");
        }

        var response = await _httpClient.GetAsync($"Auth/Verify");

        if (!response.IsSuccessStatusCode)
        {
            return AuthenticateResult.Fail("Token validation failed");
        }

        var claims = GetClaims(uToken);

        return claims == null
            ? AuthenticateResult.Fail("Token validation failed")
            : AuthenticateResult.Success(new AuthenticationTicket(claims, "Utapoi-Token"));
    }

    private static ClaimsPrincipal? GetClaims(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(token) is not JwtSecurityToken tokenResult)
        {
            return null;
        }

        var claimsIdentity = new ClaimsIdentity(tokenResult.Claims, "Token");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }
}
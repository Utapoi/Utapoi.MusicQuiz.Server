using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Infrastructure.Identity;

public sealed class UtapoiJwtBearerHandler : JwtBearerHandler
{
    private readonly HttpClient _httpClient;

    private readonly IMongoDatabase _db;

    public UtapoiJwtBearerHandler(
        IHttpClientFactory httpClientFactory,
        IOptionsMonitor<JwtBearerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IMongoDatabase db
    ) : base(options, logger, encoder, clock)
    {
        _db = db;
        _httpClient = httpClientFactory.CreateClient("UtapoiHttpClient");
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Context.Request.Cookies.TryGetValue("Utapoi-Token", out var uToken))
        {
            return AuthenticateResult.Fail("Authorization header not found.");
        }

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"Auth/Verify", UriKind.Relative),
        };
        request.Headers.Add("Cookie", string.Join(";", Context.Request.Cookies.Select(c => $"{c.Key}={c.Value}")));

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return AuthenticateResult.Fail("Token validation failed");
        }

        var claims = GetClaims(uToken);

        if (claims == null)
        {
            return AuthenticateResult.Fail("Token validation failed");
        }

        // TODO:  Better Guard/Validation?
        await TryCreateUserAsync(
            Guid.Parse(claims.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty),
            claims.FindFirstValue(ClaimTypes.Name) ?? string.Empty
        );

        return AuthenticateResult.Success(new AuthenticationTicket(claims, "Utapoi-Token"));
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

    private async Task TryCreateUserAsync(
        Guid utapoiId,
        string username,
        CancellationToken cancellationToken = default
    )
    {
        var collection = _db.GetCollection<User>("Users");
        var user = await collection
            .Find(x => x.UtapoiId == utapoiId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user != null)
        {
            return;
        }

        user = new User
        {
            UtapoiId = utapoiId,
            Created = DateTime.UtcNow,
            Username = username,
        };

        await collection.InsertOneAsync(user, null, cancellationToken);
    }
}
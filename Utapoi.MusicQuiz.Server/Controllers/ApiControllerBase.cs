using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Utapoi.MusicQuiz.Server.Controllers;

/// <summary>
///     The base controller for the application.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    /// <summary>
    ///     Gets the mediator.
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    /// <summary>
    ///     Gets the origin from the request.
    /// </summary>
    /// <returns>
    ///     The origin from the request.
    /// </returns>
    protected string GetOriginFromRequest()
    {
        return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
    }

    /// <summary>
    ///     Gets the IP address from the request.
    /// </summary>
    /// <returns>
    ///     The IP address from the request or an empty string if it cannot be found.
    /// </returns>
    protected string GetIpAddressFromRequest()
    {
        return Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
    }

    protected Guid GetUserId()
    {
        var id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out var gId))
        {
            return gId;
        }

        return Guid.Empty;
    }
}
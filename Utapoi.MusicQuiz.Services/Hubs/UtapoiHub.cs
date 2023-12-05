using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Utapoi.MusicQuiz.Infrastructure.Hubs;

public class UtapoiHub<T> : Hub<T> where T : class
{
    protected Guid GetCallerUserId()
    {
        var id = Context.GetHttpContext()?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out var gId))
        {
            return gId;
        }

        return Guid.Empty;
    }
}
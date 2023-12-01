using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms.Responses;

public class UserJoinedResponse
{
    public WebSocketUser User { get; set; } = default!;

    // TODO: Replace with User Info.
}   
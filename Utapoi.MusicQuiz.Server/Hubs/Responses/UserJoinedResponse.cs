using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Server.Hubs.Responses;

public class UserJoinedResponse
{
    public WebSocketPlayer Player { get; set; } = default!;

    // TODO: Replace with Player Info.
}   
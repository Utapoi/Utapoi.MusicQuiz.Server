using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Server.Hubs.Responses;

public sealed class RoomJoinedResponse
{
    public WebSocketRoom Room { get; set; } = default!;
}
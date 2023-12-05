using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Server.Hubs.Responses;

public sealed class RoomCreatedResponse
{
    public WebSocketRoom Room { get; set; } = null!;
}
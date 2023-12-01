using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms.Responses;

public sealed class RoomJoinedResponse
{
    public WebSocketRoom Room { get; set; } = default!;
}
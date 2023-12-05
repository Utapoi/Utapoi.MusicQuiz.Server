using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;

public static partial class GetRooms
{
    public class Response
    {
        public IReadOnlyCollection<WebSocketRoom> Rooms { get; init; } = Array.Empty<WebSocketRoom>();
    }
}
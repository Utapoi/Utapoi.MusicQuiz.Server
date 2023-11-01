using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRooms;

public static partial class GetRooms
{
    public class Response
    {
        public IReadOnlyCollection<Room> Rooms { get; init; } = Array.Empty<Room>();
    }
}
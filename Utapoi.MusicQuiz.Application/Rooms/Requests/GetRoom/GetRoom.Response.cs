using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Application.Rooms.Requests.GetRoom;

public static partial class GetRoom
{
    public sealed class Response
    {
        public Room Room { get; set; } = default!;
    }
}
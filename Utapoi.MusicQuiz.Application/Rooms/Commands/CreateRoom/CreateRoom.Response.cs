using Utapoi.MusicQuiz.Core.Enums;

namespace Utapoi.MusicQuiz.Application.Rooms.Commands.CreateRoom;

public static partial class CreateRoom
{
    public class Response
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public RoomType Type { get; init; }
    }
}
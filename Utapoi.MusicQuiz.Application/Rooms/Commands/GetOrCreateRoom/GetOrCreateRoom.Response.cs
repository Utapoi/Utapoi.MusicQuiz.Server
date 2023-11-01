namespace Utapoi.MusicQuiz.Application.Rooms.Commands.GetOrCreateRoom;

public static partial class GetOrCreateRoom
{
    public sealed record Response
    {
        public Guid Id { get; init; }
    }
}
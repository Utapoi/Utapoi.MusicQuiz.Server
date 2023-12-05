using Utapoi.MusicQuiz.Core.Enums;

namespace Utapoi.MusicQuiz.Server.Hubs.Requests;

public class CreateRoomRequest
{
    public string Name { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public RoomType Type { get; set; }

    public int MaxPlayers { get; init; }

    public int Rounds { get; init; }
}
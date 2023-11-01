namespace Utapoi.MusicQuiz.Server.Requests.Rooms;

public class CreateRoomRequest
{
    public string Name { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public int MaxPlayers { get; init; }

    public int Rounds { get; init; }
}
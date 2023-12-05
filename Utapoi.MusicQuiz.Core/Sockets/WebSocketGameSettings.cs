namespace Utapoi.MusicQuiz.Core.Sockets;

public sealed class WebSocketGameSettings
{
    public int Rounds { get; set; } = 20;

    public int MaxPlayers { get; set; } = 4;
}
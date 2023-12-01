namespace Utapoi.MusicQuiz.Infrastructure.Hubs.Game;

public class GameHub : UtapoiHub<IGameHub>
{
    public event EventHandler OnPlayerGuessed;
}
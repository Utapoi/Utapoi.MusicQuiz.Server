namespace Utapoi.MusicQuiz.Infrastructure.Hubs.Game;

public interface IGameHub
{
    public event EventHandler OnPlayerGuessed;
}

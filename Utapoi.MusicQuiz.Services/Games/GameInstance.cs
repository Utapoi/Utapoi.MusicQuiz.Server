using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Utapoi.MusicQuiz.Application.Games;
using Utapoi.MusicQuiz.Core.Sockets;
using Utapoi.MusicQuiz.Infrastructure.Hubs.Game;
using Timer = System.Timers.Timer;

namespace Utapoi.MusicQuiz.Infrastructure.Games;

/// <summary>
/// Manages a <b>single</b> instance of an active <see cref="WebSocketGame"/> and <see cref="WebSocketRoom"/>.
/// </summary>
public class GameInstance : IDisposable
{
    public Guid RoomId { get; set; }

    private readonly GameManager _gameManager;

    private readonly IHubContext<GameHub> _hubContext;

    private readonly ILogger<IGameManager> _logger;

    #region Instance Information

    private Timer GameTimer { get; }

    private WebSocketRoom CurrentRoom => _gameManager.GetRoom(RoomId)!;

    private ConcurrentDictionary<Guid, WebSocketUser> Players => CurrentRoom.Players;

    #endregion

    public GameInstance(GameManager gameManager, IHubContext<GameHub> hubContext, ILogger<IGameManager> logger)
    {
        _gameManager = gameManager;
        _hubContext = hubContext;
        _logger = logger;

        GameTimer = new Timer();
    }

    public Task StartGame()
    {
        _logger.LogInformation("Started a Game: {Name}", CurrentRoom.Name);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        GameTimer.Dispose();

        _logger.LogInformation("Game Instance Disposed: {Name}", CurrentRoom.Name);
    }
}
using System.Security.Claims;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Utapoi.MusicQuiz.Application.Games;
using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Server.Hubs;

/// <summary>
/// The main and unique SignalR Hub of Utapoi.
/// </summary>
/// <remarks>
/// SignalR only allows a single Hub per server.
/// UtapoiHub is divided in partial classes, each one is responsible of a different sections of WebSockets communication.
/// This main file defines methods or dependencies used in other files.
/// </remarks>
[Authorize]
public partial class UtapoiHub : Hub<IUtapoiHub>
{
    // Note (Mikyan): Do we really need to dispatch events using MediatR since we only use IGameManager?
    protected ISender Mediator { get; }

    protected IGameManager GameManager { get; }

    protected ILogger<UtapoiHub> Logger { get; }

    public UtapoiHub(ISender mediator, IGameManager gameManager, ILogger<UtapoiHub> logger)
    {
        Mediator = mediator;
        GameManager = gameManager;
        Logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        await OnPlayerConnected();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // TODO: Update last connection time.
        // Remove player from GameManager after x minutes

        await base.OnDisconnectedAsync(exception);
    }

    protected Guid GetCallerUserId()
    {
        var id = Context.GetHttpContext()?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out var gId))
        {
            return gId;
        }

        return Guid.Empty;
    }

    protected string GetCallerName()
    {
        return Context.GetHttpContext()?.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    }

    protected string GetCallerAvatar()
    {
        return Context.GetHttpContext()?.User.FindFirstValue("Avatar") ?? string.Empty;
    }

    #region Hub Methods

    [HubMethodName("HeartBeat")]
    [UsedImplicitly]
    public Task PlayerHeartBeat()
    {
        return OnPlayerHeartBeat();
    }

    #endregion

    #region Handlers

    private Task OnPlayerConnected()
    {
        GameManager.AddPlayer(new WebSocketPlayer
        {
            Id = GetCallerUserId(),
            Name = GetCallerName(),
            ConnectionId = Context.ConnectionId,
            LastHeartBeat = DateTime.UtcNow,
        });

        // TODO: Check if player is in a room
        // TODO: Check the last connection time of the player
        // TODO: Try to reconnect the player to the room

        return Task.CompletedTask;
    }

    private Task OnPlayerHeartBeat()
    {
        var player = GameManager.GetPlayer(GetCallerUserId());

        if (player == null)
        {
            return Task.CompletedTask;
        }

        player.LastHeartBeat = DateTime.UtcNow;

        return Task.CompletedTask;
    }

    #endregion
}
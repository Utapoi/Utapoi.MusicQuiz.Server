using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Utapoi.MusicQuiz.Application.Games;
using Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests;
using CreateRoomRequest = Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests.CreateRoomRequest;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms;

[Authorize]
public sealed partial class RoomHub : UtapoiHub<IRoomHub>
{
    private readonly ISender _mediator;

    private readonly IGameManager _gameManager;
    
    private readonly ILogger<RoomHub> _logger;

    public RoomHub(ISender mediator, IGameManager gameManager, ILogger<RoomHub> logger)
    {
        _mediator = mediator;
        _gameManager = gameManager;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        await OnPlayerConnected();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // TODO: Update last connection time.
        // Context.User.GetId()
        // Update db and state.

        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("HeartBeat")]
    [UsedImplicitly]
    public Task PlayerHeartBeat()
    {
        return OnPlayerHeartBeat();
    }

    [HubMethodName("CreateRoom")]
    [UsedImplicitly]
    public Task CreateRoom(CreateRoomRequest request)
    {
        return OnCreateRoomRequested(request);
    }

    [HubMethodName("JoinRoom")]
    [UsedImplicitly]
    public Task JoinRoom(JoinRoomRequest request)
    {
        return OnJoinRoomRequested(request);
    }

    [HubMethodName("LeaveRoom")]
    [UsedImplicitly]
    public Task LeaveRoom(LeaveRoomRequest request)
    {
        return OnLeaveRoomRequested(request);
    }

    [HubMethodName("StartGame")]
    [UsedImplicitly]
    public Task StartGame(StartGameRequest request)
    {
        // TODO: Only Host can start the game.

        return OnStartGameRequested(request);
    }
}
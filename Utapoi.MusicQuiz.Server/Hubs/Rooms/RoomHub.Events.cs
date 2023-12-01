using Utapoi.MusicQuiz.Application.Rooms.Commands.CreateRoom;
using Utapoi.MusicQuiz.Core.Sockets;
using Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests;
using Utapoi.MusicQuiz.Server.Hubs.Rooms.Responses;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms;

public sealed partial class RoomHub
{
    private async Task OnPlayerConnected()
    {
        _gameManager.AddPlayer(new WebSocketUser
        {
            Id = GetCallerUserId(),
            Name = GetCallerName(),
            ConnectionId = Context.ConnectionId,
            LastHeartBeat = DateTime.UtcNow,
        });

        // TODO: Check if user is in a room
        // TODO: Check the last connection time of the user
        // TODO: Try to reconnect the user to the room
    }

    private Task OnPlayerHeartBeat()
    {
        var player = _gameManager.GetPlayer(GetCallerUserId());

        if (player == null)
        {
            return Task.CompletedTask;
        }

        player.LastHeartBeat = DateTime.UtcNow;

        return Task.CompletedTask;
    }

    private async Task OnCreateRoomRequested(CreateRoomRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new CreateRoom.Command
        {
            Name = request.Name,
            Password = request.Password,
            MaxPlayers = request.MaxPlayers,
            Rounds = request.Rounds,
            Type = request.Type,
        }, cancellationToken);

        _gameManager.AddRoom(new WebSocketRoom
        {
            Id = result.Value.Id,
            Name = result.Value.Name,
            Type = result.Value.Type,
            HubGroup = result.Value.Id.ToString(),
            // TODO: Add Caller to Players.
        });

        _logger.LogInformation("Created a new Room: {Name}", result.Value.Name);

        await Groups.AddToGroupAsync(Context.ConnectionId, result.Value.Id.ToString(), cancellationToken);
        await Clients.Caller.OnRoomCreated(new RoomCreatedResponse
        {
            Id = result.Value.Id.ToString(),
            Name = result.Value.Name,
            Type = result.Value.Type
        });
    }

    private async Task OnJoinRoomRequested(JoinRoomRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Validation / Authorization / Authentication.

        if (!Guid.TryParse(request.RoomId, out var roomId) || !Guid.TryParse(request.UserId, out var userId))
        {
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, request.RoomId, cancellationToken);
        await _gameManager.AddPlayerToRoomAsync(roomId, userId, Context.ConnectionId, cancellationToken);

        // TODO: Some implementation ideas
        // - Get room info from database
        // - Add user to the room in db

        await Clients.Caller.OnRoomJoined(new RoomJoinedResponse
        {
            Room = _gameManager.GetRoom(roomId)!
        });

        await Clients.OthersInGroup(request.RoomId).OnUserJoined(new UserJoinedResponse
        {
            User = _gameManager.GetPlayer(userId)!
        });
    }

    private async Task OnLeaveRoomRequested(LeaveRoomRequest request, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(request.RoomId, out var roomId) || !Guid.TryParse(request.UserId, out var userId))
        {
            return;
        }

        if (!_gameManager.RemovePlayerFromRoom(roomId, userId))
        {
            return;
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, request.RoomId, cancellationToken);
        await Clients.Caller.OnRoomLeft(new RoomLeftResponse());

        await Clients.OthersInGroup(request.RoomId).OnUserLeft(new UserLeftResponse
        {
            UserId = request.UserId
        });
    }

    private async Task OnStartGameRequested(StartGameRequest request, CancellationToken cancellationToken = default)
    {
        _gameManager.AddGameInstance(Guid.Parse(request.RoomId));

        await Clients.Group(request.RoomId).OnGameStarted(new GameStartedResponse());
    }
}
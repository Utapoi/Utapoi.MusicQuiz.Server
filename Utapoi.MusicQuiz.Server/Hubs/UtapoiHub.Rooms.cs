using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using Utapoi.MusicQuiz.Core.Sockets;
using Utapoi.MusicQuiz.Server.Hubs.Requests;
using Utapoi.MusicQuiz.Server.Hubs.Responses;

namespace Utapoi.MusicQuiz.Server.Hubs;

/// <summary>
/// The UtapoiHub responsible of managing Rooms.
/// </summary>
public partial class UtapoiHub // Rooms
{
    #region Hub Methods

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

    #endregion

    #region Handlers

    private async Task OnCreateRoomRequested(CreateRoomRequest request, CancellationToken cancellationToken = default)
    {
        var roomId = Guid.NewGuid();
        var room = new WebSocketRoom
        {
            Id = roomId,
            Name = request.Name,
            Type = request.Type,
            HubGroup = roomId.ToString(),
            Host = GetCallerUserId(),
        };

        // TODO: Add validation and checks.

        GameManager.AddRoom(room);

        Logger.LogInformation("Created a new Room: {Name}", room.Name);

        await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString(), cancellationToken);
        await GameManager.AddPlayerToRoomAsync(roomId, GetCallerUserId(), Context.ConnectionId, cancellationToken);
        await Clients.Caller.OnRoomCreated(new RoomCreatedResponse
        {
            Room = room,
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
        await GameManager.AddPlayerToRoomAsync(roomId, userId, Context.ConnectionId, cancellationToken);

        // TODO: Some implementation ideas
        // - Get room info from database
        // - Add player to the room in db

        await Clients.Caller.OnRoomJoined(new RoomJoinedResponse
        {
            Room = GameManager.GetRoom(roomId)!
        });

        await Clients.OthersInGroup(request.RoomId).OnUserJoined(new UserJoinedResponse
        {
            Player = GameManager.GetPlayer(userId)!
        });
    }

    private async Task OnLeaveRoomRequested(LeaveRoomRequest request, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(request.RoomId, out var roomId) || !Guid.TryParse(request.UserId, out var userId))
        {
            return;
        }

        if (!GameManager.RemovePlayerFromRoom(roomId, userId))
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

    #endregion
}
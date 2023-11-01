using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Utapoi.MusicQuiz.Application.Rooms.Commands.GetOrCreateRoom;
using Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests;
using Utapoi.MusicQuiz.Server.Hubs.Rooms.Responses;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms;

//[Authorize]
public sealed class RoomHub : Hub<IRoomHub>
{
    private readonly ISender _mediator;

    public RoomHub(ISender mediator)
    {
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        // TODO: Check if user is authenticated
        // TODO: Check if user is in a room
        // TODO: Check the last connection time of the user
        // TODO: Try to reconnect the user to the room

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // TODO: Start a timer to check if the user reconnects before 2min
        // If the user reconnects, cancel the timer
        // If the timer runs out, disconnect the user from the room

        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("JoinRoom")]
    public Task JoinRoom(JoinRoomRequest request)
    {
        return OnJoinRoomRequested(request);
    }

    [HubMethodName("LeaveRoom")]
    public Task LeaveRoom(LeaveRoomRequest request, CancellationToken cancellationToken = default)
    {
        return OnLeaveRoomRequested(request, cancellationToken);
    }

    private async Task OnJoinRoomRequested(JoinRoomRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Validation / Authorization / Authentication.

        await Groups.AddToGroupAsync(Context.ConnectionId, request.RoomId, cancellationToken);

        var result = await _mediator.Send(new GetOrCreateRoom.Command
        {
            RoomId = Guid.Parse(request.RoomId),
            UserId = Guid.Parse(request.UserId)
        }, cancellationToken);

        // TODO: Some implementation ideas
        // - Get room info from database
        // - Create a new room if it doesn't exist
        // - Add user to the room in db

        await Clients.Caller.OnRoomJoined(new RoomJoinedResponse
        {
            RoomId = result.Value.Id.ToString()
        });

        await Clients.OthersInGroup(request.RoomId).OnUserJoined(new UserJoinedResponse
        {
            UserId = result.Value.Id.ToString()
        });
    }

    private async Task OnLeaveRoomRequested(LeaveRoomRequest request, CancellationToken cancellationToken = default)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, request.RoomId.ToString(), cancellationToken);
        await Clients.Caller.OnRoomLeft(new RoomLeftResponse());
    }
}
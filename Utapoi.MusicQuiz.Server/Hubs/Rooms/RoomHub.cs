using Microsoft.AspNetCore.SignalR;
using Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests;
using Utapoi.MusicQuiz.Server.Hubs.Rooms.Responses;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms;

public sealed class RoomHub : Hub<IRoomHub>
{
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
        await Groups.AddToGroupAsync(Context.ConnectionId, request.RoomId, cancellationToken);

        // TODO: Some implementation ideas
        // - Get room info from database
        // - Create a new room if it doesn't exist
        // - Add user to the room in db

        await Clients.Caller.OnRoomJoined(new RoomJoinedResponse
        {
            RoomId = request.RoomId
        });

        await Clients.OthersInGroup(request.RoomId).OnUserJoined(new UserJoinedResponse
        {
            UserId = request.UserId
        });
    }

    private async Task OnLeaveRoomRequested(LeaveRoomRequest request, CancellationToken cancellationToken = default)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, request.RoomId.ToString(), cancellationToken);
        await Clients.Caller.OnRoomLeft(new RoomLeftResponse());
    }
}
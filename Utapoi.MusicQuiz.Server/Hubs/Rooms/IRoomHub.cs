using Utapoi.MusicQuiz.Server.Hubs.Rooms.Responses;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms;

public interface IRoomHub
{
    Task OnRoomJoined(RoomJoinedResponse response);

    Task OnRoomLeft(RoomLeftResponse response);

    Task OnUserJoined(UserJoinedResponse response);

    Task GetRoomInfo(Guid roomId, CancellationToken cancellationToken = default);
}
using Utapoi.MusicQuiz.Server.Hubs.Responses;

namespace Utapoi.MusicQuiz.Server.Hubs;

public interface IUtapoiHub
{
    #region UtapoiHub.Rooms

    Task OnRoomCreated(RoomCreatedResponse response);

    Task OnRoomJoined(RoomJoinedResponse response);

    Task OnRoomLeft(RoomLeftResponse response);

    Task OnUserJoined(UserJoinedResponse response);

    Task OnUserLeft(UserLeftResponse response);

    Task OnGameStarted(GameStartedResponse response);

    Task GetRoomInfo(Guid roomId, CancellationToken cancellationToken = default);

    #endregion
}
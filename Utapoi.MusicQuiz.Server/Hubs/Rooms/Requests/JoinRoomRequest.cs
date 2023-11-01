namespace Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests;

public sealed class JoinRoomRequest
{
    public string UserId { get; set; } = string.Empty;

    public string RoomId { get; set; } = string.Empty;
}
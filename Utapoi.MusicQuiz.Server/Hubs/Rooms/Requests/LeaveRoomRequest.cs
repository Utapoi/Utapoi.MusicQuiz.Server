namespace Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests;

public sealed class LeaveRoomRequest
{
    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }
}
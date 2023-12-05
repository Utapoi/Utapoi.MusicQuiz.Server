namespace Utapoi.MusicQuiz.Server.Hubs.Requests;

public sealed class LeaveRoomRequest
{
    public string RoomId { get; set; } = string.Empty;
 
    public string UserId { get; set; } = string.Empty;
}
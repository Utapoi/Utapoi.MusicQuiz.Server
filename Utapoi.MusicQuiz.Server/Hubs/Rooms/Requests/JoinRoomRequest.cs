using System.ComponentModel.DataAnnotations;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms.Requests;

public sealed class JoinRoomRequest
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string RoomId { get; set; } = string.Empty;

    public string RoomPassword { get; set; } = string.Empty;
}
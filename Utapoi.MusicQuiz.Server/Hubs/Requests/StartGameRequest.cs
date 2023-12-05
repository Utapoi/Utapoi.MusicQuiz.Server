using System.ComponentModel.DataAnnotations;

namespace Utapoi.MusicQuiz.Server.Hubs.Requests;

public sealed class StartGameRequest
{
    [Required] public string RoomId { get; set; } = string.Empty;
}
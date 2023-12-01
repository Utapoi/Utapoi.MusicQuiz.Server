using Utapoi.MusicQuiz.Core.Enums;

namespace Utapoi.MusicQuiz.Server.Hubs.Rooms.Responses;

public sealed class RoomCreatedResponse
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public RoomType Type { get; set; }
}
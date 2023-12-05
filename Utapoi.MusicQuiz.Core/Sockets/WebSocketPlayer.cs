using System.Text.Json.Serialization;
using Utapoi.MusicQuiz.Core.Enums;

namespace Utapoi.MusicQuiz.Core.Sockets;

public sealed class WebSocketPlayer
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public string ConnectionId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;

    public UserResponseState State { get; set; } = UserResponseState.NotAnswered;

    public string CurrentResponse { get; set; } = string.Empty;

    public int Score { get; set; }

    public int Rank { get; set; }

    public DateTime LastHeartBeat { get; set; }

    [JsonIgnore]
    public bool IsConnected => (DateTime.UtcNow - LastHeartBeat) < TimeSpan.FromSeconds(15);
}
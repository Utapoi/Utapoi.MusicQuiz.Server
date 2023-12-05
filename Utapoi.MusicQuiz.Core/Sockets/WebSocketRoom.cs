using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using Utapoi.MusicQuiz.Core.Enums;

namespace Utapoi.MusicQuiz.Core.Sockets;

public sealed class WebSocketRoom
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public string  Password { get; set; } = string.Empty;

    public bool HasPassword => !string.IsNullOrWhiteSpace(Password);

    public RoomType Type { get; set; } = RoomType.SinglePlayer;

    /// <summary>
    /// The name of the SignalR Group.
    /// </summary>
    [JsonIgnore]
    public string HubGroup { get; set; } = string.Empty;

    public Guid Host { get; set; } = default!;

    public ConcurrentDictionary<Guid, WebSocketPlayer> Players { get; set; } = new();

    public ConcurrentDictionary<Guid, WebSocketPlayer> Spectators { get; set; } = new();

    public WebSocketGame Game { get; set; } = default!;

    public WebSocketGameSettings Settings { get; set; } = new();
}
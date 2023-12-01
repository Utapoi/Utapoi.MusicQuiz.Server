using System.Text.Json.Serialization;
using Utapoi.MusicQuiz.Core.Entities;
using Utapoi.MusicQuiz.Core.Enums;

namespace Utapoi.MusicQuiz.Core.Sockets;

public sealed class WebSocketGame
{
    public GameState GameState { get; set; } = GameState.Configuring;

    /// <summary>
    /// The list of songs that need to be guessed during the game.
    /// Since we don't send them to clients, we don't need a specific WebSocket object.
    /// </summary>
    [JsonIgnore]
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}
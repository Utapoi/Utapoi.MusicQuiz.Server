using Utapoi.MusicQuiz.Core.Sockets;

namespace Utapoi.MusicQuiz.Application.Games;

public interface IGameManager
{
    bool AddPlayer(WebSocketPlayer player);

    WebSocketPlayer? GetPlayer(Guid userId);

    bool AddRoom(WebSocketRoom room);

    IReadOnlyCollection<WebSocketRoom> GetRooms();

    WebSocketRoom? GetRoom(Guid roomId);

    bool RemoveRoom(Guid roomId);

    bool AddGameInstance(Guid roomId);

    Task<bool> AddPlayerToRoomAsync(
        Guid roomId,
        Guid userId,
        string connectionId,
        CancellationToken cancellationToken = default
    );

    bool RemovePlayerFromRoom(
        Guid roomId,
        Guid userId
    );
}
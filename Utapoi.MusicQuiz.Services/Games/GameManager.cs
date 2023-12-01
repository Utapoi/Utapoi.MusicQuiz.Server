using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Utapoi.MusicQuiz.Application.Games;
using Utapoi.MusicQuiz.Core.Entities;
using Utapoi.MusicQuiz.Core.Sockets;
using Utapoi.MusicQuiz.Infrastructure.Hubs.Game;

namespace Utapoi.MusicQuiz.Infrastructure.Games;

public sealed class GameManager : IGameManager
{
    private ConcurrentDictionary<Guid, GameInstance> Instances { get; } = new();

    private ConcurrentDictionary<Guid, WebSocketRoom> Rooms { get; } = new();

    private ConcurrentDictionary<Guid, WebSocketUser> Players { get; } = new();

    private IHubContext<GameHub> HubContext { get; set; }

    private readonly IMongoDatabase _db;

    private readonly ILogger<IGameManager> _logger;

    public GameManager(IHubContext<GameHub> hub, ILogger<IGameManager> logger, IMongoDatabase db)
    {
        HubContext = hub;
        _logger = logger;
        _db = db;
    }

    public bool AddPlayer(WebSocketUser user)
    {
        return Players.TryAdd(user.Id, user);
    }

    public WebSocketUser? GetPlayer(Guid userId)
    {
        return Players.TryGetValue(userId, out var user)
            ? user
            : null;
    }

    public bool AddRoom(WebSocketRoom room)
    {
        return Rooms.TryAdd(room.Id, room);
    }

    public WebSocketRoom? GetRoom(Guid roomId)
    {
        return Rooms.TryGetValue(roomId, out var room)
            ? room
            : null;
    }

    public bool AddGameInstance(Guid roomId)
    {
        var result = Instances.TryAdd(roomId, new GameInstance(this, HubContext, _logger)
        {
            RoomId = roomId,
        });

        if (!result)
        {
            return false;
        }

        Task.Run(() => Instances[roomId].StartGame());

        return true;
    }

    public async Task<bool> AddPlayerToRoomAsync(
        Guid roomId,
        Guid userId,
        string connectionId,
        CancellationToken cancellationToken = default
    )
    {
        var room = GetRoom(roomId);

        if (room == null)
        {
            return false;
        }

        var user = GetPlayer(userId);

        // Note(Mikyan): This should never happen...?
        if (user == null)
        {
            var dbUser = await _db
                .GetCollection<User>("Users")
                .Find(x => x.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbUser != null)
            {
                user = new WebSocketUser
                {
                    Id = dbUser.Id,
                    ConnectionId = connectionId,
                    Name = dbUser.Username,
                    LastHeartBeat = DateTime.UtcNow,
                };

                AddPlayer(user);
            }
            else
            {
                return false;
            }
        }

        room.Players.TryAdd(user.Id, user);

        return true;
    }

    public bool RemovePlayerFromRoom(
        Guid roomId,
        Guid userId
    )
    {
        var room = GetRoom(roomId);

        if (room == null)
        {
            return false;
        }

        var user = GetPlayer(userId);

        if (user == null)
        {
            return false;
        }

        return room.Players.TryRemove(userId, out _);
    }

    private void OnPlayerGuessed(object? sender, EventArgs e)
    {
    }
}
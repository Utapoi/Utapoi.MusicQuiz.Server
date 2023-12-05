using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Utapoi.MusicQuiz.Application.Rooms;
using Utapoi.MusicQuiz.Application.Rooms.Commands.CreateRoom;
using Utapoi.MusicQuiz.Application.Rooms.Commands.GetOrCreateRoom;
using Utapoi.MusicQuiz.Application.Users;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Infrastructure.Rooms;

internal sealed class RoomsService : IRoomsService
{
    public static readonly string RoomsTable = "Rooms";

    private IMongoCollection<Room> Rooms { get; }

    private readonly IUsersService _usersService;

    public RoomsService(IMongoDatabase db, IUsersService usersService)
    {
        _usersService = usersService;

        Rooms = db.GetCollection<Room>(RoomsTable);
    }

    public Task<List<Room>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Add pagination and search filters.
        return Rooms
            .Find(_ =>  true)
            .ToListAsync(cancellationToken);
    }

    public async Task<Room> GetOrCreateAsync(
        GetOrCreateRoom.Command command,
        CancellationToken cancellationToken = default
    )
    {
        var room = await GetAsync(command.RoomId, cancellationToken)
                   ?? await CreateAsync(cancellationToken);

        room.AddUser(await _usersService.GetAsync(command.UserId, cancellationToken));

        await Rooms.ReplaceOneAsync(x => x.Id == room.Id, room, cancellationToken: cancellationToken);

        return room;
    }

    public async Task<Room?> GetAsync(
        Guid roomId,
        CancellationToken cancellationToken = default
    )
    {
        if (roomId == Guid.Empty)
        {
            return null;
        }

        return await Rooms
            .Find(x => x.Id == roomId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Room> CreateAsync(
        CancellationToken cancellationToken = default
    )
    {
        var room = new Room
        {
            Id = Guid.NewGuid()
        };

        await Rooms.InsertOneAsync(room, cancellationToken: cancellationToken);

        return room;
    }

    public async Task<Room> CreateAsync(
        CreateRoom.Command command,
        CancellationToken cancellationToken = default
    )
    {
        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Password = command.Password, // TODO: Should we hash this? This is just a password for a temporary room...
            MaxPlayers = command.MaxPlayers,
        };

        await Rooms.InsertOneAsync(room, cancellationToken: cancellationToken);

        return room;
    }
}
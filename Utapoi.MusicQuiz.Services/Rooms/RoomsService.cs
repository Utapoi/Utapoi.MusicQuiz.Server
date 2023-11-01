using Microsoft.EntityFrameworkCore;
using Utapoi.MusicQuiz.Application.Persistence;
using Utapoi.MusicQuiz.Application.Rooms;
using Utapoi.MusicQuiz.Application.Rooms.Commands.GetOrCreateRoom;
using Utapoi.MusicQuiz.Application.Users;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Infrastructure.Rooms;

internal sealed class RoomsService : IRoomsService
{
    private readonly IMusicQuizContext _context;

    private readonly IUsersService _usersService;

    public RoomsService(IMusicQuizContext context, IUsersService usersService)
    {
        _context = context;
        _usersService = usersService;
    }

    public Task<List<Room>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Add pagination and search filters.
        return _context.Rooms.ToListAsync(cancellationToken);
    }

    public async Task<Room> GetOrCreateAsync(
        GetOrCreateRoom.Command command,
        CancellationToken cancellationToken = default
    )
    {
        var room = await GetAsync(command.RoomId, cancellationToken)
                   ?? await CreateAsync(cancellationToken);

        room.AddUser(await _usersService.GetAsync(command.UserId, cancellationToken));

        await _context.SaveChangesAsync(cancellationToken);

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

        return await _context
            .Rooms
            .FirstOrDefaultAsync(x => x.Id == roomId, cancellationToken);
    }

    public async Task<Room> CreateAsync(
        CancellationToken cancellationToken = default
    )
    {
        var room = _context.Rooms.Add(new Room
        {
            Id = Guid.NewGuid()
        }).Entity;

        await _context.SaveChangesAsync(cancellationToken);

        return room;
    }
}
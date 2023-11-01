using Utapoi.MusicQuiz.Application.Rooms.Commands.GetOrCreateRoom;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Application.Rooms;

public interface IRoomsService
{
    Task<List<Room>> GetAllAsync(
        CancellationToken cancellationToken = default
    );

    Task<Room> GetOrCreateAsync(
        GetOrCreateRoom.Command command,
        CancellationToken cancellationToken = default
    );

    Task<Room?> GetAsync(
        Guid roomId,
        CancellationToken cancellationToken = default
    );

    Task<Room> CreateAsync(
        CancellationToken cancellationToken = default
    );
}
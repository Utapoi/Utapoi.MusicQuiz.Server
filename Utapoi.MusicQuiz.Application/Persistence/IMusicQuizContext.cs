using Microsoft.EntityFrameworkCore;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Application.Persistence;

public interface IMusicQuizContext
{
    DbSet<Room> Rooms { get; }

    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
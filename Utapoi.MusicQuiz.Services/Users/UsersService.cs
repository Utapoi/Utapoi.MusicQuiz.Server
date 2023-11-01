using Microsoft.EntityFrameworkCore;
using Utapoi.MusicQuiz.Application.Persistence;
using Utapoi.MusicQuiz.Application.Users;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Infrastructure.Users;

internal sealed class UsersService : IUsersService
{
    private readonly IMusicQuizContext _context;

    public UsersService(IMusicQuizContext context)
    {
        _context = context;
    }

    public async Task<User?> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        if (userId == Guid.Empty)
        {
            return null;
        }

        return await _context
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }
}
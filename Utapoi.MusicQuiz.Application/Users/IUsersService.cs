using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Application.Users;

public interface IUsersService
{
    Task<User?> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
}
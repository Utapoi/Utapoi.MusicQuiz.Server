using MongoDB.Driver;
using Utapoi.MusicQuiz.Application.Users;
using Utapoi.MusicQuiz.Core.Entities;

namespace Utapoi.MusicQuiz.Infrastructure.Users;

internal sealed class UsersService : IUsersService
{
    public static readonly string UsersTable = "Users";

    private IMongoCollection<User> Users { get; }

    public UsersService(IMongoDatabase db)
    {
        Users = db.GetCollection<User>(UsersTable);
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

        return await Users
            .Find(x => x.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
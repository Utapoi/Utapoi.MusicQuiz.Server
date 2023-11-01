using Utapoi.MusicQuiz.Core.Entities.Common;

namespace Utapoi.MusicQuiz.Core.Entities;

public sealed class Room : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Adds a user to the room.
    /// </summary>
    /// <param name="user">
    /// The user to add.
    /// </param>
    public void AddUser(User? user)
    {
        if (user is null)
            return;

        if (Users.Any(u => u.UtapoiId == user.UtapoiId))
            return;

        Users.Add(user);
    }

    /// <summary>
    /// Removes a user from the room.
    /// </summary>
    /// <param name="user">
    /// The user to remove.
    /// </param>
    public void RemoveUser(User user)
    {
        if (Users.All(u => u.UtapoiId != user.UtapoiId))
            return;

        Users.Remove(user);
    }
}
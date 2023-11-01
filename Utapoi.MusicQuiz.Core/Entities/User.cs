using Utapoi.MusicQuiz.Core.Entities.Common;

namespace Utapoi.MusicQuiz.Core.Entities;

public sealed class User : AuditableEntity
{
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// This is the Id of the account on the Identity Server of Utapoi.Auth
    /// We use this to identify the user.
    /// </summary>
    public Guid UtapoiId { get; set; }

    public Guid? CurrentRoomId { get; set; }

    public Room? CurrentRoom { get; set; }

}
using Utapoi.MusicQuiz.Core.Entities.Common;

namespace Utapoi.MusicQuiz.Core.Entities;

public sealed class Singer : Entity
{
    /// <summary>
    ///     Gets an <see cref="ICollection{T}" /> of <see cref="LocalizedString" />s representing the names of the singer.
    /// </summary>
    public ICollection<LocalizedString> Names { get; set; } = new List<LocalizedString>();

    public string ProfilePictureUrl { get; set; } = string.Empty;
}
using Utapoi.MusicQuiz.Core.Common;
using Utapoi.MusicQuiz.Core.Entities.Common;

namespace Utapoi.MusicQuiz.Core.Entities;

public sealed class Song : Entity
{
    /// <summary>
    ///     Gets the titles of the song.
    /// </summary>
    public ICollection<LocalizedString> Titles { get; set; } = new List<LocalizedString>();

    /// <summary>
    ///     Gets or sets the duration of the song.
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;

    /// <summary>
    ///     Gets or sets the release date of the song.
    /// </summary>
    public DateTime ReleaseDate { get; set; } = DateTime.MinValue;

    /// <summary>
    ///     Gets or sets the original language of the song.
    /// </summary>
    public string OriginalLanguage { get; set; } = Languages.Japanese;

    /// <summary>
    ///     Gets or sets the cover art of the song.
    /// </summary>
    public string AlbumCoverUrl { get; set; } = string.Empty;

    /// <summary>
    ///     Gets an <see cref="ICollection{T}" /> of <see cref="Singer" />s who sang the song.
    /// </summary>
    public ICollection<Singer> Singers { get; set; } = new List<Singer>();
}
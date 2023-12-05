using Utapoi.MusicQuiz.Core.Entities.Common;

namespace Utapoi.MusicQuiz.Core.Entities;

public sealed class Collection : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public IReadOnlyCollection<Song> Songs { get; set; } = new List<Song>();

    public string CoverUrl { get; set; } = string.Empty;
}
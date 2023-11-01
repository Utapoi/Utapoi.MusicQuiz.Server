using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Utapoi.MusicQuiz.Core.Entities.Common;

/// <summary>
///     Represents an entity.
/// </summary>
public abstract class Entity
{
    /// <summary>
    ///     Gets or sets the identifier.
    /// </summary>
    [Key, Column("_id")]
    public Guid Id { get; set; } = Guid.NewGuid();
}
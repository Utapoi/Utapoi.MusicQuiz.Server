using Utapoi.MusicQuiz.Core.Common;
using Utapoi.MusicQuiz.Core.Entities.Common;

namespace Utapoi.MusicQuiz.Core.Entities;

/// <summary>
///     Represents a localized string.
/// </summary>
public sealed class LocalizedString : Entity
{
    public LocalizedString()
    {
    }

    public LocalizedString(string text, string language)
    {
        Text = text;
        Language = language;
    }

    /// <summary>
    ///     Gets or sets the text.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the language.
    /// </summary>
    /// <remarks>
    ///     The default language is <see cref="Languages.English" />.
    /// </remarks>
    public string Language { get; set; } = Languages.English;
}
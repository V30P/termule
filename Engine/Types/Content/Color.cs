using System.Text.Json.Serialization;

namespace Termule.Types.Content;

/// <summary>
///     Color that can be rendered by terminals.
/// </summary>
public readonly record struct Color
{
    /// <summary>
    ///     Gets the full RGB color if one was set.
    /// </summary>
    public FullColor? Full { get; private init; }

    /// <summary>
    ///     Gets the basic preset color if one was set.
    /// </summary>
    public BasicColor Basic { get; private init; }

    /// <summary>
    ///     Creates a color from an RGB tuple.
    /// </summary>
    /// <param name="t">The RGB values to use.</param>
    public static implicit operator Color((int r, int g, int b) t)
    {
        return new Color(t.r, t.g, t.b);
    }

    /// <summary>
    ///     Creates a color from a basic color.
    /// </summary>
    /// <param name="b">The basic color to use.</param>
    public static implicit operator Color(BasicColor b)
    {
        return new Color(b);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Color" /> class.
    /// </summary>
    public Color()
        : this(BasicColor.Default)
    {
    }

    [JsonConstructor]
#pragma warning disable IDE0051
    private Color(FullColor? full, BasicColor basic)
#pragma warning restore IDE0051
    {
        Full = full;
        Basic = basic;
    }

    private Color(int r, int g, int b)
    {
        Full = new FullColor(r, g, b);
    }

    private Color(BasicColor baseColor)
    {
        Basic = baseColor;
    }
}
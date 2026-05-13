using System.Text.Json.Serialization;

namespace Termule.Engine.Types;

/// <summary>
///     Color that can be rendered by terminals.
/// </summary>
public readonly struct Color
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Color"/> struct.
    /// </summary>
    public Color()
        : this(BasicColor.Default)
    {
    }

    [JsonConstructor]
#pragma warning disable IDE0051 // Remove unused private members
    private Color(FullColor? full, BasicColor basic)
#pragma warning restore IDE0051 // Remove unused private members
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
    ///     Determines whether two Color instances are equal.
    /// </summary>
    /// <param name="c1">The first Color to compare.</param>
    /// <param name="c2">The second Color to compare.</param>
    /// <returns>true if the colors are equal; otherwise, false.</returns>
    public static bool operator ==(Color c1, Color c2)
    {
        return c1.Basic == c2.Basic && c1.Full == c2.Full;
    }

    /// <summary>
    ///     Determines whether two Color instances are not equal.
    /// </summary>
    /// <param name="c1">The first Color to compare.</param>
    /// <param name="c2">The second Color to compare.</param>
    /// <returns>true if the colors are not equal; otherwise, false.</returns>
    public static bool operator !=(Color c1, Color c2)
    {
        return !(c1 == c2);
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is Color color && this == color;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Full, Basic);
    }
}

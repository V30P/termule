namespace Termule.Engine.Types;

/// <summary>
///     Single cell on a terminal.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="Cell" /> struct.
/// </remarks>
/// <param name="color">The background color of the cell.</param>
/// <param name="glyph">The glyph in the cell.</param>
/// <param name="charColor">The color of the cell's glyph.</param>
public struct Cell(
    Color color = default,
    char glyph = '\0',
    Color charColor = default) : IEquatable<Cell>
{
    /// <summary>
    ///     Gets or sets the background color of the cell.
    /// </summary>
    public Color Color { get; set; } = color;

    /// <summary>
    ///     Gets or sets the glyph in the cell.
    /// </summary>
    public char Glyph { get; set; } = glyph;

    /// <summary>
    ///     Gets or sets the color of the cell's glyph.
    /// </summary>
    public Color GlyphColor { get; set; } = charColor;

    /// <summary>
    ///     Compares two cells by value and returns whether they match.
    /// </summary>
    /// <param name="c1">The first cell.</param>
    /// <param name="c2">The second cell.</param>
    /// <returns>If all cell values are equal.</returns>
    public static bool operator ==(Cell c1, Cell c2)
    {
        return c1.Color == c2.Color
               && c1.Glyph == c2.Glyph
               && c1.GlyphColor == c2.GlyphColor;
    }

    /// <summary>
    ///     Compares two cells by value and returns whether they don't match.
    /// </summary>
    /// <param name="c1">The first cell.</param>
    /// <param name="c2">The second cell.</param>
    /// <returns>If any cell values are different.</returns>
    public static bool operator !=(Cell c1, Cell c2)
    {
        return !(c1 == c2);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object obj)
    {
        return obj is Cell cell && this == cell;
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Color, Glyph, GlyphColor);
    }

    /// <inheritdoc />
    public readonly bool Equals(Cell other)
    {
        return Color.Equals(other.Color) && Glyph == other.Glyph && GlyphColor.Equals(other.GlyphColor);
    }
}

namespace Termule.Engine.Types;

/// <summary>
///     Single cell on a terminal.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="Cell" /> struct.
/// </remarks>
/// <param name="color">The background color of the cell (or the default if none is provided).</param>
/// <param name="character">The character in the cell (or the default if none is provided).</param>
/// <param name="charColor">The color of the cell's character (or the default if none is provided).</param>
public struct Cell(Color color = default, char character = '\0', Color charColor = default) : IEquatable<Cell>
{
    /// <summary>
    ///     Gets or sets the background color of the cell.
    /// </summary>
    public Color Color { get; set; } = color;

    /// <summary>
    ///     Gets or sets the character in the cell.
    /// </summary>
    public char Char { get; set; } = character;

    /// <summary>
    ///     Gets or sets the color of the cell's character.
    /// </summary>
    public Color CharColor { get; set; } = charColor;

    /// <summary>
    ///     Compares two cells by value and returns whether they match.
    /// </summary>
    /// <param name="c1">The first cell.</param>
    /// <param name="c2">The second cell.</param>
    /// <returns>If all cell values are equal.</returns>
    public static bool operator ==(Cell c1, Cell c2)
    {
        return c1.Color == c2.Color
               && c1.Char == c2.Char
               && c1.CharColor == c2.CharColor;
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
    public readonly override bool Equals(object obj)
    {
        return obj is Cell cell && this == cell;
    }

    /// <inheritdoc />
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Color, Char, CharColor);
    }

    /// <inheritdoc />
    public bool Equals(Cell other)
    {
        return Color.Equals(other.Color) && Char == other.Char && CharColor.Equals(other.CharColor);
    }
}

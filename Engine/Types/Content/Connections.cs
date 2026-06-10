namespace Termule.Engine.Types;

/// <summary>
///     Flags representing the connections of a Unicode box-drawing glyph.
/// </summary>
/// <remarks>
///     Use <see cref="ConnectionsConversions" /> methods to convert between
///     <see cref="Connections" /> values and <see cref="char" />s.
/// </remarks>
[Flags]
public enum Connections
{
#pragma warning disable CS1591
    None = 0,
    Up = 1 << 0,
    Right = 1 << 1,
    Down = 1 << 2,
    Left = 1 << 3
#pragma warning restore CS1591
}

/// <summary>
///     Extension methods for converting to and from <see cref="Connections" />.
/// </summary>
public static class ConnectionsConversions
{
    private static readonly Dictionary<Connections, char> ConnectionsToGlyph = new()
    {
        [Connections.Up] = '╵',
        [Connections.Right] = '╶',
        [Connections.Down] = '╷',
        [Connections.Left] = '╴',
        [Connections.Up | Connections.Down] = '│',
        [Connections.Right | Connections.Left] = '─',
        [Connections.Up | Connections.Right] = '└',
        [Connections.Right | Connections.Down] = '┌',
        [Connections.Down | Connections.Left] = '┐',
        [Connections.Left | Connections.Up] = '┘',
        [Connections.Up | Connections.Right | Connections.Down] = '├',
        [Connections.Right | Connections.Down | Connections.Left] = '┬',
        [Connections.Down | Connections.Left | Connections.Up] = '┤',
        [Connections.Left | Connections.Up | Connections.Right] = '┴',
        [Connections.Up | Connections.Right | Connections.Down | Connections.Left] = '┼'
    };

    private static readonly Dictionary<char, Connections> GlyphToConnections =
        ConnectionsToGlyph.Select(p => KeyValuePair.Create(p.Value, p.Key)).ToDictionary();

    /// <summary>
    ///     Gets the box-drawing glyph for this <see cref="Connections" />.
    /// </summary>
    /// <param name="connections">The connections to convert.</param>
    /// <returns>The corresponding unicode box glyph.</returns>
    public static char ToGlyph(this Connections connections)
    {
        return ConnectionsToGlyph.GetValueOrDefault(connections);
    }

    /// <summary>
    ///     Finds the box-drawing <see cref="Connections" /> of the given
    ///     <paramref name="glyph" />.
    /// </summary>
    /// <param name="glyph">The glyph to determine the connections of.</param>
    /// <returns>The connections of the glyph.</returns>
    /// <remarks>This will be <see cref="Connections.None" /> for most glyphs.</remarks>
    public static Connections FromGlyph(char glyph)
    {
        return GlyphToConnections.GetValueOrDefault(glyph);
    }
}

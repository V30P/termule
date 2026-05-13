namespace Termule.Engine.Types;

/// <summary>
///     Flags representing the connections of a Unicode box-drawing character.
/// </summary>
/// <remarks>
///     Use <see cref="ConnectionsExtensions" /> methods to convert between
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
///     Extension methods for working with <see cref="Connections" />.
/// </summary>
public static class ConnectionsExtensions
{
    private static readonly Dictionary<Connections, char> ConnectionsToCharacter = new()
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

    private static readonly Dictionary<char, Connections> CharacterToConnections =
        ConnectionsToCharacter.Select(p => KeyValuePair.Create(p.Value, p.Key)).ToDictionary();

    /// <summary>
    ///     Gets the box-drawing character for this <see cref="Connections" />.
    /// </summary>
    /// <param name="connections">The connections to convert.</param>
    /// <returns>The corresponding unicode box character.</returns>
    public static char ToChar(this Connections connections)
    {
        return ConnectionsToCharacter.GetValueOrDefault(connections);
    }

    /// <summary>
    ///     Finds the box-drawing <see cref="Connections" /> of the given
    ///     <paramref name="character" />.
    /// </summary>
    /// <param name="character">The character to determine the connections of.</param>
    /// <returns>The connections of the character.</returns>
    /// <remarks>This will be <see cref="Connections.None" /> for most characters.</remarks>
    public static Connections FromChar(char character)
    {
        return CharacterToConnections.GetValueOrDefault(character);
    }
}

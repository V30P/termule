namespace Termule.Engine.Types;

/// <summary>
///     Flags representing the connections of a unicode box-drawing character.
/// </summary>
/// <remarks> 
///     Use <see cref="ConnectionsExtensions"/> methods to convert to and from <see cref="char"/>s.
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
///     Extension methods for working with <see cref="Connections"/>
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

    extension(Connections connections)
    {
        /// <summary>
        ///     Gets the box-drawing character for this <see cref="Connections"/>.
        /// </summary>
        /// <returns>The corresponding unicode box character.</returns>
        public char ToChar()
        {
            return ConnectionsToCharacter.GetValueOrDefault(connections);
        }

        /// <summary>
        ///     Finds the box-drawing <see cref="Connections"/> of the given 
        ///     <paramref name="character"/>.
        /// </summary>
        /// <param name="character"></param>
        /// <returns>The connections of the character.</returns>
        /// <remarks>This will be <see cref="Connections.None"/> for most characters.</remarks>
        public static Connections FromChar(char character)
        {
            return CharacterToConnections.GetValueOrDefault(character);
        }
    }
}

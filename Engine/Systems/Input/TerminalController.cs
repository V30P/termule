using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;
using Termule.Engine.Systems.Display;

namespace Termule.Engine.Systems.Input;

// ? Should this all be on a background thread so that we can timestamp characters?

/// <summary>
///     System that converts keyboard and mouse input from a <see cref="Terminal"/> to messages
///     on the <see cref="Game"/>'s c<see cref="MessageBus"/>.
/// </summary>
public sealed partial class TerminalController : Core.System
{
    private readonly InputParser[] parsers = [
        new SGRParser(),
        new SS3Parser(),
        new CSIParser(),
        new CharParser()
    ];

    private Terminal terminal;

    /// <inheritdoc/>
    protected internal override void Start()
    {
        terminal = GetRequiredSystem<Terminal>();

        Console.Write("\e[?1003h"); // Enable any-motion mouse tracking
        Console.Write("\e[?1006h"); // Enable SGR coordinates for mouse tracking
        Console.Write("\x1b[>31u"); // Enable all Kitty protocol information (if available)
    }

    /// <inheritdoc/>
    protected internal override void Tick()
    {
        string input = terminal.CollectInput();

        foreach (InputParser parser in parsers)
        {
            foreach (InputMessage message in parser.Parse(input))
            {
                Game.Bus.Broadcast(message);
            }

            input = parser.Remainder;
        }
    }

    /// <inheritdoc/>
    protected internal override void CleanUp()
    {
        base.CleanUp();

        Console.Write("\e[?1003l"); // Disable any-motion mouse tracking
        Console.Write("\e[?1006l"); // Disable SGR coordinates for mouse tracking
        Console.Write("\x1b[<u"); // Pop the old Kitty protocol config from the stack
    }
}

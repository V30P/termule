using System.Globalization;
using System.Text.RegularExpressions;
using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;
using Termule.Engine.Systems.Display;

namespace Termule.Engine.Systems.Input;

// ? Should this all be on a background thread? It would allow us to:
// ?    1. Work continuously to avoid cutting sequences in half (already exceedingly rare)
// ?    2. Better interpret ambiguous scenarios with time context

/// <summary>
///     System that converts keyboard and mouse input from a <see cref="Terminal"/> to messages
///     on the <see cref="Game"/>'s c<see cref="MessageBus"/>.
/// </summary>
public sealed partial class TerminalController : Core.System
{
    private InputParser[] parsers;

    private Terminal terminal;

    /// <inheritdoc/>
    protected internal override void Start()
    {
        terminal = GetRequiredSystem<Terminal>();

        Console.Write("\e[?1003h"); // Enable any-motion mouse tracking
        Console.Write("\e[?1006h"); // Enable SGR coordinates for mouse tracking
        Console.Write("\e[>31u"); // Enable full Kitty protocol (if available)

        // Check that the Kitty protocol is available and the config got applied
        Console.Write("\x1b[?u");
        Thread.Sleep(50);

        string response = terminal.CollectInput();
        Match match = KittyStateRegex().Match(response);
        bool useKittyProtocol = match.Success
            && int.Parse(match.Groups["flags"].Value, CultureInfo.CurrentCulture) == 31;

        parsers = [
            new SGRParser(),
            new SS3Parser(),
            new CSIParser(useKittyProtocol),
            new CharParser()
        ];
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
        Console.Write("\e[<u"); // Pop the old Kitty protocol config from the stack
    }

    [GeneratedRegex(@"\x1b\[\?(?<flags>\d+)u")]
    private static partial Regex KittyStateRegex();
}

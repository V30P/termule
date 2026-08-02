using System.Globalization;
using System.Text.RegularExpressions;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;

namespace Termule.Engine.Systems.Input;

// ? Should this all be on a background thread? It would allow us to:
// ?    1. Work continuously to avoid cutting sequences in half (already pretty rare)
// ?    2. Better interpret ambiguous scenarios with time context

/// <summary>
///     System that converts keyboard and mouse input from a <see cref="Terminal"/> to messages
///     on <see cref="Game.Bus"/>.
/// </summary>
public sealed partial class TerminalController : Core.System
{
    private readonly InputParser[] parsers = [
        new SGRParser(),
        new SS3Parser(),
        new CSIParser(),
        new ASCIIParser()
    ];

    private Terminal terminal;
    private bool kittyResponseReceived;

    /// <inheritdoc/>
    protected internal override void Start()
    {
        terminal = GetRequiredSystem<Terminal>();

        Console.Write("\e[?1003h"); // Enable any-motion mouse tracking
        Console.Write("\e[?1006h"); // Enable SGR coordinates for mouse tracking
        Console.Write("\e[>31u"); // Enable full Kitty protocol (if available)

        // Query the Kitty protocol state
        Console.Write("\e[?u");
    }

    /// <inheritdoc/>
    protected internal override void Tick()
    {
        // If we haven't received a response yet, look for Kitty reporting back about its state.
        // When we do hear back, update the parsers about Kitty availability.
        string input = new(terminal.Input);
        if (!kittyResponseReceived)
        {
            Match match = KittyStateRegex().Match(input);
            if (match.Success)
            {
                kittyResponseReceived = true;

                bool useKittyProtocol = match.Success
                    && int.Parse(match.Groups["flags"].Value, CultureInfo.CurrentCulture) == 31;

                for (int i = 0; i < parsers.Length; i++)
                {
                    if (parsers[i] is CSIParser parser)
                    {
                        parser.UseKittyProtocol = useKittyProtocol;
                    }
                }

                input = KittyStateRegex().Replace(input, string.Empty);
            }
        }

        // Run the input through each parser sequentially
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
        Console.Write("\e[<u"); // Pop the old Kitty config from the stack
    }

    [GeneratedRegex(@"\e\[\?(?<flags>\d+)u")]
    private static partial Regex KittyStateRegex();
}

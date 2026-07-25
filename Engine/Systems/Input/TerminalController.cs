using System.Globalization;
using System.Text.RegularExpressions;
using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Input;

/// <summary>
///     System that converts keyboard and mouse input from a <see cref="Terminal"/> to messages
///     on the <see cref="Game"/>'s c<see cref="MessageBus"/>.
/// </summary>
public sealed partial class TerminalController : Core.System
{
    private static readonly Dictionary<PressState, int> PressStateDecayMilis = new()
    {
        [PressState.Pressed] = 500,
        [PressState.Held] = 50
    };

    private readonly Dictionary<Button, ButtonState> keyHoldStates = [];

    private Terminal terminal;

    private enum PressState
    {
        Released,
        Pressed,
        Held
    }

    /// <inheritdoc/>
    protected internal override void Start()
    {
        terminal = GetRequiredSystem<Terminal>();

        Console.Write("\e[?1003h"); // Enable any-motion mouse tracking
        Console.Write("\e[?1006h"); // Enable SGR coordinates for mouse tracking
    }

    /// <inheritdoc/>
    protected internal override void Tick()
    {
        string input = terminal.CollectInput();

        // Parse mouse
        MatchCollection sgrEvents = SGRRegex().Matches(input);
        foreach (Match match in sgrEvents)
        {
            int value = int.Parse(match.Groups[1].Value, CultureInfo.CurrentCulture);

            // Decode movement
            if ((value & 32) != 0)
            {
                VectorInt mousePos = (
                    int.Parse(match.Groups[2].Value, CultureInfo.CurrentCulture) - 1,
                    int.Parse(match.Groups[3].Value, CultureInfo.CurrentCulture) - 1
                );

                Game.Bus.Broadcast(new MouseMoved(mousePos));
            }

            // Parse escape sequence keys

            // Decode buttons
            if ((value & 64) == 0)
            {
                int buttonIndex = value & 3;
                if (buttonIndex == 3)
                {
                    continue;
                }

                Button button = ButtonConversions.MouseButtonIndexToButton(buttonIndex);
                if (match.Groups[4].Value == "M")
                {
                    Game.Bus.Broadcast(new ButtonPressed(button));
                    Game.Bus.Broadcast(new HoldStarted(button));
                }
                else
                {
                    Game.Bus.Broadcast(new HoldStopped(button));
                }
            }
            else
            {
                Button button = ButtonConversions.MouseWheelIndexToButton(value & 3);
                Game.Bus.Broadcast(new ButtonPressed(button));
            }
        }

        input = SGRRegex().Replace(input, string.Empty);

        // Parse keys
        foreach (char character in input)
        {
            Game.Bus.Broadcast(new CharTyped(character));

            if (ButtonConversions.TryConvertCharToButton(character, out Button button))
            {
                Game.Bus.Broadcast(new ButtonPressed(button));

                if (!keyHoldStates.TryGetValue(button, out ButtonState state))
                {
                    keyHoldStates.Add(button, new());
                }

                if (state.PressState == PressState.Released)
                {
                    keyHoldStates[button] = new(PressState.Pressed);
                    Game.Bus.Broadcast(new HoldStarted(button));
                }
                else
                {
                    keyHoldStates[button] = new(PressState.Held);
                }
            }
        }

        // Clear expired key states
        foreach (KeyValuePair<Button, ButtonState> buttonPair in keyHoldStates)
        {
            ButtonState state = buttonPair.Value;
            if (state.PressState == PressState.Released)
            {
                continue;
            }

            double elapsedMilis = (DateTime.Now - state.Time).TotalMilliseconds;
            if (elapsedMilis > PressStateDecayMilis[state.PressState])
            {
                keyHoldStates[buttonPair.Key] = new(PressState.Released);

                Game.Bus.Broadcast(new HoldStarted(buttonPair.Key));
            }
        }
    }

    /// <inheritdoc/>
    protected internal override void CleanUp()
    {
        base.CleanUp();

        Console.Write("\e[?1003l"); // Disable any-motion mouse tracking
        Console.Write("\e[?1006l"); // Disable SGR coordinates for mouse tracking
    }

    [GeneratedRegex(@"\x1b\[<(\d+);(\d+);(\d+)([Mm])")]
    private static partial Regex SGRRegex();

    private struct ButtonState(PressState pressState)
    {
        internal PressState PressState = pressState;
        internal DateTime Time = DateTime.Now;
    }
}

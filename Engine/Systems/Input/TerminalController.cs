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

        // Parse mouse events represented by SGR escape sequences
        MatchCollection sgrEvents = SGRRegex().Matches(input);
        foreach (Match match in sgrEvents)
        {
            int eventCode = int.Parse(match.Groups["event"].Value, CultureInfo.CurrentCulture);

            // Decode movement
            if ((eventCode & 32) != 0)
            {
                VectorInt mousePos = (
                    int.Parse(match.Groups["x"].Value, CultureInfo.CurrentCulture) - 1,
                    int.Parse(match.Groups["y"].Value, CultureInfo.CurrentCulture) - 1
                );

                Game.Bus.Broadcast(new MouseMoved(mousePos));
            }

            // Decode buttons
            if ((eventCode & 64) == 0)
            {
                int buttonIndex = eventCode & 3;

                // This is an outdated indicator that a mouse button was released
                if (buttonIndex == 3)
                {
                    continue;
                }

                Button button = ButtonConversions.FromSGRMouseButton[buttonIndex];
                if (match.Groups["action"].Value == "M")
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
                Button button = ButtonConversions.FromSGRMouseWheel[eventCode & 3];
                Game.Bus.Broadcast(new ButtonPressed(button));
            }
        }
        input = SGRRegex().Replace(input, string.Empty);

        // Parse keys represented by SS3 escape sequences
        foreach (Match match in SS3Regex().Matches(input))
        {
            char command = match.Groups["command"].Value[0];
            if (!ButtonConversions.FromSS3Command.TryGetValue(command, out Button key))
            {
                continue;
            }

            HandleKeyPress(key);
        }
        input = SS3Regex().Replace(input, string.Empty);

        // Parse keys represented by CSI escape sequences
        foreach (Match match in CSIRegex().Matches(input))
        {
            HandleKeyPress(ButtonConversions.FromCSIMatch(match));
        }
        input = CSIRegex().Replace(input, string.Empty);

        // Parse keys represented as single characters
        foreach (char character in input)
        {
            Game.Bus.Broadcast(new CharTyped(character));

            if (ButtonConversions.FromChar.TryGetValue(character, out Button key))
            {
                HandleKeyPress(key);
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

                Game.Bus.Broadcast(new HoldStopped(buttonPair.Key));
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

    [GeneratedRegex(@"\x1b\[<(?<event>\d+);(?<x>\d+);(?<y>\d+)(?<action>[Mm])")]
    private static partial Regex SGRRegex();

    [GeneratedRegex(@"\x1bO(?<command>[@-~])")]
    private static partial Regex SS3Regex();

    [GeneratedRegex(@"\x1b\[(?<params>[0-9;?]*)(?<command>[@-~])")]
    private static partial Regex CSIRegex();

    private void HandleKeyPress(Button key)
    {
        Game.Bus.Broadcast(new ButtonPressed(key));

        if (!keyHoldStates.TryGetValue(key, out ButtonState state))
        {
            keyHoldStates.Add(key, new());
        }

        if (state.PressState == PressState.Released)
        {
            keyHoldStates[key] = new(PressState.Pressed);
            Game.Bus.Broadcast(new HoldStarted(key));
        }
        else
        {
            keyHoldStates[key] = new(PressState.Held);
        }
    }

    private struct ButtonState(PressState pressState)
    {
        internal PressState PressState = pressState;
        internal DateTime Time = DateTime.Now;
    }

    private static class ButtonConversions
    {
        public static readonly Dictionary<int, Button> FromSGRMouseButton = new()
        {
            [0] = Button.LeftMouse,
            [1] = Button.MiddleMouse,
            [2] = Button.RightMouse
        };

        public static readonly Dictionary<int, Button> FromSGRMouseWheel = new()
        {
            [0] = Button.MouseWheelUp,
            [1] = Button.MouseWheelDown,
            [2] = Button.MouseWheelLeft,
            [3] = Button.MouseWheelRight
        };

        public static readonly Dictionary<char, Button> FromSS3Command = new()
        {
            ['A'] = Button.Up,
            ['B'] = Button.Down,
            ['C'] = Button.Right,
            ['D'] = Button.Left,

            ['H'] = Button.Home,
            ['F'] = Button.End,

            ['P'] = Button.F1,
            ['Q'] = Button.F2,
            ['R'] = Button.F3,
            ['S'] = Button.F4,
        };

        public static readonly Dictionary<char, Button> FromChar = new()
        {
            ['\b'] = Button.Backspace,
            ['\x7F'] = Button.Backspace,
            ['\t'] = Button.Tab,
            ['\r'] = Button.Enter,
            ['\n'] = Button.Enter,
            ['\e'] = Button.Escape,
            [' '] = Button.Space,

            ['0'] = Button.D0,
            ['1'] = Button.D1,
            ['2'] = Button.D2,
            ['3'] = Button.D3,
            ['4'] = Button.D4,
            ['5'] = Button.D5,
            ['6'] = Button.D6,
            ['7'] = Button.D7,
            ['8'] = Button.D8,
            ['9'] = Button.D9,

            ['a'] = Button.A,
            ['b'] = Button.B,
            ['c'] = Button.C,
            ['d'] = Button.D,
            ['e'] = Button.E,
            ['f'] = Button.F,
            ['g'] = Button.G,
            ['h'] = Button.H,
            ['i'] = Button.I,
            ['j'] = Button.J,
            ['k'] = Button.K,
            ['l'] = Button.L,
            ['m'] = Button.M,
            ['n'] = Button.N,
            ['o'] = Button.O,
            ['p'] = Button.P,
            ['q'] = Button.Q,
            ['r'] = Button.R,
            ['s'] = Button.S,
            ['t'] = Button.T,
            ['u'] = Button.U,
            ['v'] = Button.V,
            ['w'] = Button.W,
            ['x'] = Button.X,
            ['y'] = Button.Y,
            ['z'] = Button.Z,

            ['A'] = Button.A,
            ['B'] = Button.B,
            ['C'] = Button.C,
            ['D'] = Button.D,
            ['E'] = Button.E,
            ['F'] = Button.F,
            ['G'] = Button.G,
            ['H'] = Button.H,
            ['I'] = Button.I,
            ['J'] = Button.J,
            ['K'] = Button.K,
            ['L'] = Button.L,
            ['M'] = Button.M,
            ['N'] = Button.N,
            ['O'] = Button.O,
            ['P'] = Button.P,
            ['Q'] = Button.Q,
            ['R'] = Button.R,
            ['S'] = Button.S,
            ['T'] = Button.T,
            ['U'] = Button.U,
            ['V'] = Button.V,
            ['W'] = Button.W,
            ['X'] = Button.X,
            ['Y'] = Button.Y,
            ['Z'] = Button.Z,

            ['!'] = Button.Exclamation,
            ['"'] = Button.DoubleQuote,
            ['#'] = Button.Hash,
            ['$'] = Button.Dollar,
            ['%'] = Button.Percent,
            ['&'] = Button.Ampersand,
            ['\''] = Button.Apostrophe,
            ['('] = Button.LeftParen,
            [')'] = Button.RightParen,
            ['*'] = Button.Asterisk,
            ['+'] = Button.Plus,
            [','] = Button.Comma,
            ['-'] = Button.Minus,
            ['.'] = Button.Period,
            ['/'] = Button.Slash,
            [':'] = Button.Colon,
            [';'] = Button.Semicolon,
            ['<'] = Button.LessThan,
            ['='] = Button.Equals,
            ['>'] = Button.GreaterThan,
            ['?'] = Button.Question,
            ['@'] = Button.At,
            ['['] = Button.LeftBracket,
            ['\\'] = Button.Backslash,
            [']'] = Button.RightBracket,
            ['^'] = Button.Caret,
            ['_'] = Button.Underscore,
            ['`'] = Button.Grave,
            ['{'] = Button.LeftBrace,
            ['|'] = Button.Pipe,
            ['}'] = Button.RightBrace,
            ['~'] = Button.Tilde,
        };

        private static readonly Dictionary<char, Button> FromCSICommand = new()
        {
            ['A'] = Button.Up,
            ['B'] = Button.Down,
            ['C'] = Button.Right,
            ['D'] = Button.Left,

            ['H'] = Button.Home,
            ['F'] = Button.End,
        };

        private static readonly Dictionary<int, Button> FromCSITildeIndex = new()
        {
            [1] = Button.Home,
            [2] = Button.Insert,
            [3] = Button.Delete,
            [4] = Button.End,
            [5] = Button.PageUp,
            [6] = Button.PageDown,

            [11] = Button.F1,
            [12] = Button.F2,
            [13] = Button.F3,
            [14] = Button.F4,
            [15] = Button.F5,
            [17] = Button.F6,
            [18] = Button.F7,
            [19] = Button.F8,
            [20] = Button.F9,
            [21] = Button.F10,
            [23] = Button.F11,
            [24] = Button.F12
        };

        public static Button FromCSIMatch(Match match)
        {
            char command = match.Groups["command"].Value[0];
            if (command == '~')
            {
                string indexString = match.Groups["params"].Value.Split(';').Last();
                return FromCSITildeIndex[int.Parse(indexString, CultureInfo.CurrentCulture)];
            }

            return FromCSICommand[command];
        }
    }
}

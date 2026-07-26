using System.Globalization;
using System.Text.RegularExpressions;

namespace Termule.Engine.Systems.Input;

internal sealed partial class KittyParser : InputParser
{
    private static readonly Dictionary<int, Button> KittyCodepointToButton = new()
    {
        [8] = Button.Backspace,
        [9] = Button.Tab,
        [13] = Button.Enter,
        [27] = Button.Escape,
        [32] = Button.Space,
        [127] = Button.Backspace,

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

        [57344] = Button.Escape,
        [57345] = Button.Enter,
        [57346] = Button.Tab,
        [57347] = Button.Backspace,
        [57348] = Button.Insert,
        [57349] = Button.Delete,
        [57350] = Button.Left,
        [57351] = Button.Right,
        [57352] = Button.Up,
        [57353] = Button.Down,
        [57354] = Button.PageUp,
        [57355] = Button.PageDown,
        [57356] = Button.Home,
        [57357] = Button.End,
        [57358] = Button.CapsLock,
        [57359] = Button.ScrollLock,
        [57360] = Button.NumLock,
        [57361] = Button.PrintScreen,
        [57362] = Button.Pause,
        [57363] = Button.Menu,

        [57364] = Button.F1,
        [57365] = Button.F2,
        [57366] = Button.F3,
        [57367] = Button.F4,
        [57368] = Button.F5,
        [57369] = Button.F6,
        [57370] = Button.F7,
        [57371] = Button.F8,
        [57372] = Button.F9,
        [57373] = Button.F10,
        [57374] = Button.F11,
        [57375] = Button.F12,
        [57376] = Button.F13,
        [57377] = Button.F14,
        [57378] = Button.F15,
        [57379] = Button.F16,
        [57380] = Button.F17,
        [57381] = Button.F18,
        [57382] = Button.F19,
        [57383] = Button.F20,
        [57384] = Button.F21,
        [57385] = Button.F22,
        [57386] = Button.F23,
        [57387] = Button.F24,
        [57388] = Button.F25,
        [57389] = Button.F26,
        [57390] = Button.F27,
        [57391] = Button.F28,
        [57392] = Button.F29,
        [57393] = Button.F30,
        [57394] = Button.F31,
        [57395] = Button.F32,
        [57396] = Button.F33,
        [57397] = Button.F34,
        [57398] = Button.F35,

        [57399] = Button.Keypad0,
        [57400] = Button.Keypad1,
        [57401] = Button.Keypad2,
        [57402] = Button.Keypad3,
        [57403] = Button.Keypad4,
        [57404] = Button.Keypad5,
        [57405] = Button.Keypad6,
        [57406] = Button.Keypad7,
        [57407] = Button.Keypad8,
        [57408] = Button.Keypad9,
        [57409] = Button.KeypadDecimal,
        [57410] = Button.KeypadDivide,
        [57411] = Button.KeypadMultiply,
        [57412] = Button.KeypadSubtract,
        [57413] = Button.KeypadAdd,
        [57414] = Button.KeypadEnter,
        [57415] = Button.KeypadEqual,
        [57416] = Button.KeypadSeparator,
        [57417] = Button.KeypadLeft,
        [57418] = Button.KeypadRight,
        [57419] = Button.KeypadUp,
        [57420] = Button.KeypadDown,
        [57421] = Button.KeypadPageUp,
        [57422] = Button.KeypadPageDown,
        [57423] = Button.KeypadHome,
        [57424] = Button.KeypadEnd,
        [57425] = Button.KeypadInsert,
        [57426] = Button.KeypadDelete,
        [57427] = Button.KeypadBegin,

        [57428] = Button.MediaPlay,
        [57429] = Button.MediaPause,
        [57430] = Button.MediaPlayPause,
        [57431] = Button.MediaReverse,
        [57432] = Button.MediaStop,
        [57433] = Button.MediaFastForward,
        [57434] = Button.MediaRewind,
        [57435] = Button.MediaTrackNext,
        [57436] = Button.MediaTrackPrevious,
        [57437] = Button.MediaRecord,
        [57438] = Button.LowerVolume,
        [57439] = Button.RaiseVolume,
        [57440] = Button.MuteVolume,

        [57441] = Button.LeftShift,
        [57442] = Button.LeftControl,
        [57443] = Button.LeftAlt,
        [57444] = Button.LeftSuper,
        [57445] = Button.LeftHyper,
        [57446] = Button.LeftMeta,
        [57447] = Button.RightShift,
        [57448] = Button.RightControl,
        [57449] = Button.RightAlt,
        [57450] = Button.RightSuper,
        [57451] = Button.RightHyper,
        [57452] = Button.RightMeta,
        [57453] = Button.ISOLevel3Shift,
        [57454] = Button.ISOLevel5Shift,
    };

    internal override IEnumerable<InputMessage> Parse(string input)
    {
        foreach (Match match in KittyRegex().Matches(input))
        {
            string[] parameters = [.. match.Groups["params"].Value.Split(';')];

            // Parse codepoint
            // Alternate values may be provided separated by colons, so we take the first (default)
            string codepointString = parameters[0].Split(':').First();
            int codepoint = int.Parse(codepointString, CultureInfo.CurrentCulture);
            Button key = KittyCodepointToButton[codepoint];

            if (parameters.Length == 1)
            {
                continue;
            }

            // Parse mods
            int action = 1;
            if (parameters[1] != string.Empty)
            {
                // Mods section includes: 1. Modifier mask and 2. event
                string[] mods = parameters[1].Split(':');

                if (mods.Length > 1)
                {
                    action = int.Parse(mods[1], CultureInfo.CurrentCulture);
                }
            }

            if (action < 3)
            {
                yield return new ButtonPressed(key);

                // Only indicate a hold when the press is actually from the user, not the
                // terminal repeating a held key
                if (action == 1)
                {
                    yield return new HoldStarted(key);
                }
            }
            else
            {
                yield return new HoldStopped(key);
            }

            if (parameters.Length == 2)
            {
                continue;
            }

            // Parse text
            yield return new CharTyped(
                char.ConvertFromUtf32(
                    int.Parse(parameters[2], CultureInfo.CurrentCulture)
                )[0]
            );
        }

        Remainder = KittyRegex().Replace(input, string.Empty);
    }

    [GeneratedRegex(@"\x1b\[>?(?<params>[0-9;:?]*)u")]
    private static partial Regex KittyRegex();
}

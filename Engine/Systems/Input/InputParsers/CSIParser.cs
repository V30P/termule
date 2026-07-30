using System.Globalization;
using System.Text.RegularExpressions;

namespace Termule.Engine.Systems.Input;

internal sealed partial class CSIParser(bool useKittyProtocol) : ASCIIParser
{
    private static readonly Dictionary<int, Button> CSITildeCodepointToButton = new()
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

    private static readonly Dictionary<char, Button> CSICommandToButton = new()
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
        ['S'] = Button.F4
    };

    private static readonly Dictionary<int, Button> KittyCodepointToButton = new()
    {
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

    // This will only be used when the Kitty protocol is active
    private readonly Dictionary<Button, bool> buttonIsDown = [];

    internal override IEnumerable<InputMessage> Parse(string input)
    {
        foreach (Match match in CSIRegex().Matches(input))
        {
            if (TryExtractButton(match, out Button button))
            {
                // If Kitty isn't available, fall back to an instantaneous press
                if (!useKittyProtocol)
                {
                    yield return new ButtonDown(button);
                    yield return new ButtonUp(button);
                }

                string[] parameters = [.. match.Groups["params"].Value.Split(';')];

                // Parse the action itself
                // If no specific Kitty action is provided, default is press
                int action = 1;
                if (parameters.Length >= 2 && parameters[1] != string.Empty)
                {
                    string[] mods = parameters[1].Split(':');
                    if (mods.Length > 1)
                    {
                        action = int.Parse(mods[1], CultureInfo.CurrentCulture);
                    }
                }

                // Emit messages based on button state
                /*
                * Technically, there is an action value of 2 that indicates repeating due to
                * holding, but it seems like often times 1 is sent instead so we can't
                * rely on that distinction, that's why we track state manually
                */
                _ = buttonIsDown.TryAdd(button, false);
                if (action == 1 && !buttonIsDown[button])
                {
                    yield return new ButtonDown(button);

                    buttonIsDown[button] = true;
                }
                else if (action == 3 && buttonIsDown[button])
                {
                    yield return new ButtonUp(button);

                    buttonIsDown[button] = false;
                }

                // Parse the resulting text character if one is provided
                if (parameters.Length >= 3 && parameters[2] != string.Empty)
                {
                    string textString = parameters[2].Split(':').First();
                    char text = char.ConvertFromUtf32(
                        int.Parse(textString, CultureInfo.CurrentCulture)
                    )[0];

                    yield return new CharTyped(text);
                }
            }
        }

        Remainder = CSIRegex().Replace(input, string.Empty);
    }

    private static bool TryExtractButton(Match match, out Button button)
    {
        button = default;

        char command = match.Groups["command"].Value[0];
        int codepoint = GetCodepoint(match);
        return command switch
        {
            '~' => CSITildeCodepointToButton.TryGetValue(codepoint, out button),
            'u' => ASCIIToButton.TryGetValue(codepoint, out button)
                || KittyCodepointToButton.TryGetValue(codepoint, out button),
            _ => CSICommandToButton.TryGetValue(command, out button)
        };

        static int GetCodepoint(Match match)
        {
            return int.Parse(
                match.Groups["params"].Value.Split(';').First().Split(':').First(),
                CultureInfo.CurrentCulture
            );
        }
    }

    [GeneratedRegex(@"\e\[(?<params>[0-9;:?]*)(?<command>[@-~])")]
    private static partial Regex CSIRegex();
}

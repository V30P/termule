using System.Globalization;
using System.Text.RegularExpressions;

namespace Termule.Engine.Systems.Input;

internal sealed partial class CSIParser : InputParser
{
    private static readonly Dictionary<int, Button> CSITildeIndexToButton = new()
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
    };

    internal override IEnumerable<InputMessage> Parse(string input)
    {
        foreach (Match match in CSIRegex().Matches(input))
        {
            char command = match.Groups["command"].Value[0];
            Button key;
            if (command == '~')
            {
                int index = int.Parse(
                    match.Groups["params"].Value.Split(';').Last(), CultureInfo.CurrentCulture
                );

                if (CSITildeIndexToButton.TryGetValue(index, out key))
                {
                    yield return new ButtonPressed(key);
                    continue;
                }
            }

            if (CSICommandToButton.TryGetValue(command, out key))
            {
                yield return new ButtonPressed(key);
            }
        }

        Remainder = CSIRegex().Replace(input, string.Empty);
    }

    [GeneratedRegex(@"\x1b\[(?<params>[0-9;?]*)(?<command>[@-~])")]
    private static partial Regex CSIRegex();
}

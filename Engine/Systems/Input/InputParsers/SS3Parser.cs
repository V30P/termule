using System.Text.RegularExpressions;

namespace Termule.Engine.Systems.Input;

internal sealed partial class SS3Parser : InputParser
{
    private static readonly Dictionary<char, Button> SS3CommandToButton = new()
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

    internal override IEnumerable<InputMessage> Parse(string input)
    {
        foreach (Match match in SS3Regex().Matches(input))
        {
            char command = match.Groups["command"].Value[0];
            if (!SS3CommandToButton.TryGetValue(command, out Button key))
            {
                continue;
            }

            yield return new ButtonPressed(key);
        }

        Remainder = SS3Regex().Replace(input, string.Empty);
    }

    [GeneratedRegex(@"\x1bO(?<command>[@-~])")]
    private static partial Regex SS3Regex();
}

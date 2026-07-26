using System.Globalization;
using System.Text.RegularExpressions;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Input;

internal sealed partial class SGRParser : InputParser
{
    private static readonly Dictionary<int, Button> SGRMouseButtonIndexToButton = new()
    {
        [0] = Button.LeftMouse,
        [1] = Button.MiddleMouse,
        [2] = Button.RightMouse
    };

    private static readonly Dictionary<int, Button> SGRMouseWheelIndexToButton = new()
    {
        [0] = Button.MouseWheelUp,
        [1] = Button.MouseWheelDown,
        [2] = Button.MouseWheelLeft,
        [3] = Button.MouseWheelRight
    };

    internal override IEnumerable<InputMessage> Parse(string input)
    {
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

                yield return new MouseMoved(mousePos);
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

                Button button = SGRMouseButtonIndexToButton[buttonIndex];
                if (match.Groups["action"].Value == "M")
                {
                    yield return new ButtonPressed(button);
                    yield return new HoldStarted(button);
                }
                else
                {
                    yield return new HoldStopped(button);
                }
            }
            else
            {
                Button button = SGRMouseWheelIndexToButton[eventCode & 3];
                yield return new ButtonPressed(button);
            }
        }

        Remainder = SGRRegex().Replace(input, string.Empty);
    }

    [GeneratedRegex(@"\x1b\[<(?<event>\d+);(?<x>\d+);(?<y>\d+)(?<action>[Mm])")]
    private static partial Regex SGRRegex();
}

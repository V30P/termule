using System.Globalization;
using System.Text.RegularExpressions;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Input;

internal sealed partial class SGRParser : InputParser
{
    private static readonly Button[] SGRMouseButtonIndexToButton = [
        Button.LeftMouse,
        Button.MiddleMouse,
        Button.RightMouse
    ];

    private static readonly Button[] SGRMouseWheelIndexToButton = [
        Button.MouseWheelUp,
        Button.MouseWheelDown,
        Button.MouseWheelLeft,
        Button.MouseWheelRight
    ];

    internal override IEnumerable<InputMessage> Parse(string input)
    {
        MatchCollection sgrEvents = SGRRegex().Matches(input);
        foreach (Match match in sgrEvents)
        {
            int eventCode = int.Parse(match.Groups["event"].Value, CultureInfo.CurrentCulture);

            // Parse movement
            if ((eventCode & 32) != 0)
            {
                VectorInt mousePos = (
                    int.Parse(match.Groups["x"].Value, CultureInfo.CurrentCulture) - 1,
                    int.Parse(match.Groups["y"].Value, CultureInfo.CurrentCulture) - 1
                );

                yield return new MouseMoved(mousePos);
            }

            // Parse buttons
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
                    yield return new ButtonDown(button);
                }
                else
                {
                    yield return new ButtonUp(button);
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

    [GeneratedRegex(@"\e\[<(?<event>\d+);(?<x>\d+);(?<y>\d+)(?<action>[Mm])")]
    private static partial Regex SGRRegex();
}

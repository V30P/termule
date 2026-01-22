using System.Text;
using System.Text.RegularExpressions;
using Termule.Types;

namespace Termule.Systems.Display;

public abstract partial class TerminalDisplay : Display
{
    private Content _state;

    private static readonly Dictionary<Color?, int> _backgroundColorCodes = new()
    {
        [Color.Default] = 49,
        [Color.Black] = 40,
        [Color.Red] = 41,
        [Color.Green] = 42,
        [Color.Yellow] = 43,
        [Color.Blue] = 44,
        [Color.Magenta] = 45,
        [Color.Cyan] = 46,
        [Color.White] = 47,
        [Color.BrightBlack] = 100,
        [Color.BrightRed] = 101,
        [Color.BrightGreen] = 102,
        [Color.BrightYellow] = 103,
        [Color.BrightBlue] = 104,
        [Color.BrightMagenta] = 105,
        [Color.BrightCyan] = 106,
        [Color.BrightWhite] = 107
    };

    private static readonly Dictionary<Color?, int> _foregroundColorCodes = new()
    {
        [Color.Default] = 39,
        [Color.Black] = 30,
        [Color.Red] = 31,
        [Color.Green] = 32,
        [Color.Yellow] = 33,
        [Color.Blue] = 34,
        [Color.Magenta] = 35,
        [Color.Cyan] = 36,
        [Color.White] = 37,
        [Color.BrightBlack] = 90,
        [Color.BrightRed] = 91,
        [Color.BrightGreen] = 92,
        [Color.BrightYellow] = 93,
        [Color.BrightBlue] = 94,
        [Color.BrightMagenta] = 95,
        [Color.BrightCyan] = 96,
        [Color.BrightWhite] = 97
    };

    protected override void Start()
    {
        Console.CursorVisible = false;

        Console.Write("\x1b[?1049h"); // Alternate buffer
        Console.Write("\x1b[?1003h"); // Any-motion mouse tracking
        Console.Write("\x1b[?1006h"); // SGR coordinates for mouse tracking

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            Game.Stop();
        };
    }

    protected override void Update()
    {
        // Get mouse position from the most recent SGR event
        MatchCollection sgrEvents = sgrRegex().Matches(CollectStandardInput());
        if (sgrEvents.Count > 0)
        {
            Match lastSGREvent = sgrEvents[^1];
            MousePos =
            (
                int.Parse(lastSGREvent.Groups[1].Value) - 1,
                int.Parse(lastSGREvent.Groups[2].Value) - 1
            );
        }
    }


    protected override void Stop()
    {
        Console.ResetColor();
        Console.CursorVisible = true;

        Console.Write("\x1b[?1049l");
        Console.Write("\x1b[?1003l");
        Console.Write("\x1b[?1006l");
    }

    internal override void Draw(Content content)
    {
        if
        (
            Console.WindowTop != 0
            || Console.BufferWidth != Console.WindowWidth || Console.BufferHeight != Console.WindowHeight
            || content.Size != _state?.Size
            || Console.WindowWidth != Size.X || Console.WindowHeight != Size.Y
        )
        {
            Console.ResetColor();
            Console.Clear();

            Size = (Console.WindowWidth, Console.WindowHeight);
            _state = null;
        }

        StringBuilder output = new();
        for (int x = 0; x < content.Size.X; x++)
        {
            for (int y = 0; y < content.Size.Y; y++)
            {
                if (_state?.EqualsAt(content, (x, y)) == true)
                {
                    continue;
                }

                Cell cell = content.At(x, y);
                output.Append
                (
                    $"\x1b[{y + 1};{x + 1}H" + // Go to the position
                    $"\x1b[{GetBackgroundColorCode(cell.Color)}m" + // Apply the background color 
                    $"\x1b[{GetForegroundColorCode(cell.CharColor)}m" + // Apply the foreground color 
                    (cell.Char != default(char) ? cell.Char : ' ') // Write the character 
                );
            }
        }

        Console.Write(output);
        _state = content;
    }

    private static int GetBackgroundColorCode(Color? color)
    {
        return color != null ? _backgroundColorCodes[color] : 49;
    }

    private static int GetForegroundColorCode(Color? color)
    {
        return color != null ? _foregroundColorCodes[color] : 39;
    }

    internal abstract string CollectStandardInput();

    [GeneratedRegex(@"\x1b\[<\d+;(\d+);(\d+)[Mm]")]
    protected static partial Regex sgrRegex();
}

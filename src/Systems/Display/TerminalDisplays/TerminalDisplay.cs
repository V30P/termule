using System.Text;
using System.Text.RegularExpressions;
using Termule.Types;

namespace Termule.Systems.Display.Terminal;

public abstract partial class TerminalDisplay : Display
{
    private Content _state;

    private static readonly Dictionary<BasicColor, string> _backgroundColorCodes = new()
    {
        [BasicColor.Black] = "40",
        [BasicColor.Red] = "41",
        [BasicColor.Green] = "42",
        [BasicColor.Yellow] = "43",
        [BasicColor.Blue] = "44",
        [BasicColor.Magenta] = "45",
        [BasicColor.Cyan] = "46",
        [BasicColor.White] = "47",
        [BasicColor.Default] = "49",
        [BasicColor.BrightBlack] = "100",
        [BasicColor.BrightRed] = "101",
        [BasicColor.BrightGreen] = "102",
        [BasicColor.BrightYellow] = "103",
        [BasicColor.BrightBlue] = "104",
        [BasicColor.BrightMagenta] = "105",
        [BasicColor.BrightCyan] = "106",
        [BasicColor.BrightWhite] = "107"
    };

    private static readonly Dictionary<BasicColor, string> _foregroundColorCodes = new()
    {
        [BasicColor.Black] = "30",
        [BasicColor.Red] = "31",
        [BasicColor.Green] = "32",
        [BasicColor.Yellow] = "33",
        [BasicColor.Blue] = "34",
        [BasicColor.Magenta] = "35",
        [BasicColor.Cyan] = "36",
        [BasicColor.White] = "37",
        [BasicColor.Default] = "39",
        [BasicColor.BrightBlack] = "90",
        [BasicColor.BrightRed] = "91",
        [BasicColor.BrightGreen] = "92",
        [BasicColor.BrightYellow] = "93",
        [BasicColor.BrightBlue] = "94",
        [BasicColor.BrightMagenta] = "95",
        [BasicColor.BrightCyan] = "96",
        [BasicColor.BrightWhite] = "97"
    };

    internal TerminalDisplay() { }

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

    internal abstract string CollectStandardInput();

    [GeneratedRegex(@"\x1b\[<\d+;(\d+);(\d+)[Mm]")]
    protected static partial Regex sgrRegex();


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

    private static string GetBackgroundColorCode(Color color)
    {
        return color.Full is FullColor f ?
            $"48;2;{f.R};{f.G};{f.B}" : _backgroundColorCodes[color.Basic];
    }

    private static string GetForegroundColorCode(Color color)
    {
        return color.Full is FullColor f ?
            $"38;2;{f.R};{f.G};{f.B}" : _foregroundColorCodes[color.Basic];
    }
}
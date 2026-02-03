namespace Termule.Systems.Display;

using System.Text;
using System.Text.RegularExpressions;
using Types;

public abstract partial class TerminalDisplay : Display
{
    private static readonly Dictionary<BasicColor, string> BackgroundColorCodes = new()
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
        [BasicColor.BrightWhite] = "107",
    };

    private static readonly Dictionary<BasicColor, string> ForegroundColorCodes = new()
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
        [BasicColor.BrightWhite] = "97",
    };

    private Content state;

    internal TerminalDisplay()
    {
    }

    internal override void Draw(Content content)
    {
        if
        (
            Console.WindowTop != 0
            || Console.BufferWidth != Console.WindowWidth || Console.BufferHeight != Console.WindowHeight
            || content.Size != this.state?.Size
            || Console.WindowWidth != this.Size.X || Console.WindowHeight != this.Size.Y
        )
        {
            Console.ResetColor();
            Console.Clear();

            this.Size = (Console.WindowWidth, Console.WindowHeight);
            this.state = null;
        }

        StringBuilder output = new();
        for (int x = 0; x < content.Size.X; x++)
        {
            for (int y = 0; y < content.Size.Y; y++)
            {
                if (this.state?.EqualsAt(content, (x, y)) == true)
                {
                    continue;
                }

                Cell cell = content.At(x, y);
                output.Append(
                    $"\x1b[{y + 1};{x + 1}H" + // Go to the position
                    $"\x1b[{GetBackgroundColorCode(cell.Color)}m" + // Apply the background color
                    $"\x1b[{GetForegroundColorCode(cell.CharColor)}m" + // Apply the foreground color
                    (cell.Char != default(char) ? cell.Char : ' ')); // Write the character
            }
        }

        Console.Write(output);
        this.state = content;
    }

    protected override void Start()
    {
        Console.CursorVisible = false;
        Console.Write("\x1b[?1049h"); // Enable alternate buffer

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            this.Game.Stop();
        };
    }

    protected override void Stop()
    {
        Console.ResetColor();

        Console.CursorVisible = true;
        Console.Write("\x1b[?1049l");
    }

    private static string GetBackgroundColorCode(Color color)
    {
        return color.Full is FullColor f ?
            $"48;2;{f.R};{f.G};{f.B}" : BackgroundColorCodes[color.Basic];
    }

    private static string GetForegroundColorCode(Color color)
    {
        return color.Full is FullColor f ?
            $"38;2;{f.R};{f.G};{f.B}" : ForegroundColorCodes[color.Basic];
    }
}
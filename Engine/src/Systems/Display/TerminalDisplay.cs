namespace Termule.Systems.Display;

using System.Text;
using System.Text.RegularExpressions;
using Types;

/// <summary>
/// Display class for terminal-based output.
/// </summary>
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

    internal TerminalDisplay()
    {
    }

    internal sealed override void DrawBuffer()
    {
        if
        (
            Console.WindowTop != 0
            || Console.BufferWidth != Console.WindowWidth || Console.BufferHeight != Console.WindowHeight
            || this.Buffer.Size != this.Screen?.Size
            || Console.WindowWidth != this.Size.X || Console.WindowHeight != this.Size.Y
        )
        {
            Console.ResetColor();
            Console.Clear();

            this.Size = (Console.WindowWidth, Console.WindowHeight);
        }

        StringBuilder output = new();
        for (int x = 0; x < this.Buffer.Size.X; x++)
        {
            for (int y = 0; y < this.Buffer.Size.Y; y++)
            {
                if (this.Screen?.EqualsAt(this.Buffer, (x, y)) == true)
                {
                    continue;
                }

                Cell cell = this.Buffer.At(x, y);

                // Go to position
                output.Append("\x1b[");
                output.Append(y + 1);
                output.Append(';');
                output.Append(x + 1);
                output.Append('H');

                // Apply the cell color
                output.Append("\x1b[");
                AppendForegroundColorCode(cell.Color, output);
                output.Append('m');

                // Apply the character color
                output.Append("\x1b[");
                AppendBackgroundColorCode(cell.CharColor, output);
                output.Append('m');

                // Write the character
                output.Append(cell.Char != default(char) ? cell.Char : ' ');
            }
        }

        Console.Write(output);
    }

    /// <summary>
    /// Prepares the terminal environment for drawing.
    /// </summary>
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

    /// <summary>
    /// Cleans and resets the terminal environment.
    /// </summary>
    protected override void Stop()
    {
        Console.ResetColor();

        Console.CursorVisible = true;
        Console.Write("\x1b[?1049l");
    }

    private static void AppendForegroundColorCode(Color color, StringBuilder output)
    {
        if (color.Full is FullColor fullColor)
        {
            output.Append("48;2;");
            output.Append(fullColor.R);
            output.Append(';');
            output.Append(fullColor.G);
            output.Append(';');
            output.Append(fullColor.B);
        }
        else
        {
            output.Append(BackgroundColorCodes[color.Basic]);
        }
    }

    private static void AppendBackgroundColorCode(Color color, StringBuilder output)
    {
        if (color.Full is FullColor fullColor)
        {
            output.Append("38;2;");
            output.Append(fullColor.R);
            output.Append(';');
            output.Append(fullColor.G);
            output.Append(';');
            output.Append(fullColor.B);
        }
        else
        {
            output.Append(ForegroundColorCodes[color.Basic]);
        }
    }
}
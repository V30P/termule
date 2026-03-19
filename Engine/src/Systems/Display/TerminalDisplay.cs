namespace Termule.Systems.Display;

using System.Text;
using System.Text.RegularExpressions;
using Types;

/// <summary>
/// Display class for terminal-based output.
/// </summary>
public abstract class TerminalDisplay : Display
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

    private readonly StringBuilder builder = new();

    private Color currentColor = default;
    private Color currentCharColor = default;

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

        bool skipping = true;
        for (int y = 0; y < this.Size.Y; y++)
        {
            for (int x = 0; x < this.Size.X; x++)
            {
                if (this.Screen.EqualsAt(this.Buffer, (x, y)) == true)
                {
                    skipping = true;
                    continue;
                }

                // Go to position
                if (skipping)
                {
                    this.AppendMove(x, y);
                    skipping = false;
                }

                Cell cell = this.Buffer.At(x, y);

                // Apply color changes if necessary
                if (cell.Color != this.currentColor || cell.CharColor != this.currentCharColor)
                {
                    this.builder.Append("\x1b[");

                    if (cell.Color != this.currentColor)
                    {
                        this.AppendBackgroundColorCode(cell.Color);
                        this.currentColor = cell.Color;
                    }

                    if (cell.CharColor != this.currentCharColor)
                    {
                        this.builder.Append(';');
                        this.AppendForegroundColorCode(cell.CharColor);
                        this.currentCharColor = cell.CharColor;
                    }

                    this.builder.Append('m');
                }

                // Write the character
                this.builder.Append(cell.Char != default(char) ? cell.Char : ' ');
            }
        }

        Console.Write(this.builder);
        this.builder.Clear();
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

    private void AppendMove(int x, int y)
    {
        this.builder.Append("\x1b[");
        this.builder.Append(y + 1);
        this.builder.Append(';');
        this.builder.Append(x + 1);
        this.builder.Append('H');
    }

    private void AppendBackgroundColorCode(Color color)
    {
        if (color.Full is FullColor fullColor)
        {
            this.builder.Append("48;2;");
            this.builder.Append(fullColor.R);
            this.builder.Append(';');
            this.builder.Append(fullColor.G);
            this.builder.Append(';');
            this.builder.Append(fullColor.B);
        }
        else
        {
            this.builder.Append(BackgroundColorCodes[color.Basic]);
        }
    }

    private void AppendForegroundColorCode(Color color)
    {
        if (color.Full is FullColor fullColor)
        {
            this.builder.Append("38;2;");
            this.builder.Append(fullColor.R);
            this.builder.Append(';');
            this.builder.Append(fullColor.G);
            this.builder.Append(';');
            this.builder.Append(fullColor.B);
        }
        else
        {
            this.builder.Append(ForegroundColorCodes[color.Basic]);
        }
    }
}
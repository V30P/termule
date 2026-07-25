using System.Text;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Display system for terminal-based output.
/// </summary>
public abstract class Terminal : DisplaySystem
{
    // Stop built strings from going to the LOH
    private const int BuilderLimit = 42_500;
    private const int FlushLimit = 42_000;

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
        [BasicColor.BrightWhite] = "107"
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
        [BasicColor.BrightWhite] = "97"
    };

    private readonly StringBuilder builder = new(BuilderLimit);

    private Color currentGlyphColor;
    private Color currentColor;

    internal Terminal()
    {
        Size = (Console.WindowWidth, Console.WindowHeight);
    }

    internal abstract string CollectInput();

    /// <inheritdoc/>
    protected internal override void Start()
    {
        Console.CursorVisible = false;
        Console.Write("\e[?1049h"); // Enable alternate buffer

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            Game.Stop();
        };
    }

    /// <inheritdoc/>
    protected internal override void CleanUp()
    {
        Console.CursorVisible = true;
        Console.Write("\e[?1049l"); // Disable alternate buffer
    }

    private protected sealed override void PrintBuffer()
    {
        // Handle window resizing
        bool screenCleared = false;
        if (
            Console.WindowTop != 0
            || Console.BufferWidth != Console.WindowWidth
            || Console.BufferHeight != Console.WindowHeight
            || Buffer.Size != Screen?.Size
            || Console.WindowWidth != Size.X || Console.WindowHeight != Size.Y)
        {
            Console.ResetColor();
            Console.Clear();

            Size = (Console.WindowWidth, Console.WindowHeight);
            screenCleared = true;
        }

        bool skipping = true;
        for (int y = 0; y < Size.Y; y++)
        {
            for (int x = 0; x < Size.X; x++)
            {
                if (!screenCleared && Screen[x, y] == Buffer[x, y])
                {
                    skipping = true;
                    continue;
                }

                // Go to position
                if (skipping)
                {
                    AppendMove(x, y);
                    skipping = false;
                }

                Cell cell = Buffer[x, y];

                // Apply color changes if necessary
                if (cell.Color != currentColor || cell.GlyphColor != currentGlyphColor)
                {
                    _ = builder.Append("\e[");

                    if (cell.Color != currentColor)
                    {
                        AppendBackgroundColorCode(cell.Color);
                        currentColor = cell.Color;
                    }

                    if (cell.GlyphColor != currentGlyphColor)
                    {
                        if (builder[^1] != '[')
                        {
                            _ = builder.Append(';');
                        }

                        AppendForegroundColorCode(cell.GlyphColor);
                        currentGlyphColor = cell.GlyphColor;
                    }

                    _ = builder.Append('m');
                }

                // Write the glyph
                _ = builder.Append(cell.Glyph != '\0' ? cell.Glyph : ' ');

                if (builder.Length > FlushLimit)
                {
                    FlushBuilder();
                }
            }
        }

        FlushBuilder();
    }

    private static int To256(float value)
    {
        return (int) (value * 255);
    }

    private void FlushBuilder()
    {
        foreach (ReadOnlyMemory<char> chunk in builder.GetChunks())
        {
            Console.Write(chunk);
        }

        _ = builder.Clear();
    }

    private void AppendMove(int x, int y)
    {
        _ = builder.Append("\e[");
        _ = builder.Append(y + 1);
        _ = builder.Append(';');
        _ = builder.Append(x + 1);
        _ = builder.Append('H');
    }

    private void AppendBackgroundColorCode(Color color)
    {
        if (color.Full is { } fullColor)
        {
            _ = builder.Append("48;2;");
            _ = builder.Append(To256(fullColor.R));
            _ = builder.Append(';');
            _ = builder.Append(To256(fullColor.G));
            _ = builder.Append(';');
            _ = builder.Append(To256(fullColor.B));
        }
        else
        {
            _ = builder.Append(BackgroundColorCodes[color.Basic]);
        }
    }

    private void AppendForegroundColorCode(Color color)
    {
        if (color.Full is { } fullColor)
        {
            _ = builder.Append("38;2;");
            _ = builder.Append(To256(fullColor.R));
            _ = builder.Append(';');
            _ = builder.Append(To256(fullColor.G));
            _ = builder.Append(';');
            _ = builder.Append(To256(fullColor.B));
        }
        else
        {
            _ = builder.Append(ForegroundColorCodes[color.Basic]);
        }
    }
}

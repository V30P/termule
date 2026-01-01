using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Termule.Rendering;

public static class Display
{
    public static VectorInt Size { get; private set; }
    private static Content _state;

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

    static Display()
    {
        Console.CursorVisible = false;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Configure the terminal driver to not echo
            Process.Start("stty", "-echo -icanon min 1 time 0")?.WaitForExit();
        }

        Game.Stopped += Reset;
    }

    private static void Reset()
    {
        Clear();
        Console.CursorVisible = true;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Reset terminal driver configuration
            Process.Start("stty", "sane")?.WaitForExit();
        }
    }

    private static void Clear()
    {
        Console.ResetColor();
        Console.Clear();
    }

    internal static void Draw(Content content)
    {
        if
        (
            Console.WindowTop != 0
            || Console.BufferWidth != Console.WindowWidth || Console.BufferHeight != Console.WindowHeight
            || content.Size != _state?.Size
            || Console.WindowWidth != Size.X || Console.WindowHeight != Size.Y
        )
        {
            Clear();
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
                    $"\u001b[{y + 1};{x + 1}H" + // Go to the position
                    $"\u001b[{GetBackgroundColorCode(cell.Color)}m" + // Apply the background color 
                    $"\u001b[{GetForegroundColorCode(cell.CharColor)}m" + // Apply the foreground color 
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
}
using System.Text;

namespace Termule.Rendering;

internal static class Display
{
    private static Content _currentContent;
    private static VectorInt _lastWindowSize;

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
        Console.Clear();
        Console.CursorVisible = false;

        Game.Stopped += () =>
        {
            Console.Clear();
            Console.ResetColor();
            Console.CursorVisible = true;
        };
    }

    internal static void Draw(Content content)
    {
        VectorInt windowSize = (Console.WindowWidth, Console.WindowHeight);
        if (windowSize != _lastWindowSize)
        {
            Console.Clear();
            _currentContent = null;
        }

        StringBuilder output = new();
        for (int x = 0; x < content.Size.X; x++)
        {
            for (int y = 0; y < content.Size.Y; y++)
            {
                if (content.EqualsAt(_currentContent, (x, y)))
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
        _currentContent = content;
        _lastWindowSize = windowSize;
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

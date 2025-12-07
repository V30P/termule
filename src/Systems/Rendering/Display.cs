namespace Termule.Rendering;

internal static class Display
{
    private static Frame _lastFrame = new((0, 0));
    private static VectorInt _lastWindowSize;

    private static readonly Dictionary<Color?, int> _backgroundColorCodes = new()
    {
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
        CleanOutput();
        Console.CursorVisible = false;

        Game.Stopped += () =>
        {
            CleanOutput();
            Console.CursorVisible = true;
        };
    }

    private static void CleanOutput()
    {
        Console.ResetColor();
        Console.Clear();

        _lastFrame = new Frame((0, 0));
    }

    internal static void Draw(Frame frame)
    {
        VectorInt windowSize = (Console.WindowWidth, Console.WindowHeight);
        if (windowSize != _lastWindowSize)
        {
            CleanOutput();
        }

        string output = null;
        for (int x = 0; x < frame.Image.Size.X; x++)
        {
            for (int y = 0; y < frame.Image.Size.Y; y++)
            {
                if (!frame.EqualsAt(_lastFrame, (x, y)))
                {
                    output +=
                    $"\u001b[{y + 1};{x + 1}H" + // Go to the position
                    $"\u001b[{GetBackgroundColorCode(frame.Image.Color[x, y])}m" + // Apply the background color 
                    $"\u001b[{GetForegroundColorCode(frame.Image.TextColor[x, y])}m" + // Apply the foreground color 
                    (frame.Image.Text[x, y] ?? ' '); // Write the character 
                }
            }
        }

        Console.Write(output);
        _lastFrame = frame;
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

namespace Termule.Rendering;

public static partial class Display
{
    private static Frame _lastFrame = new((0, 0));
    private static VectorInt _lastWindowSize;

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

    public static void Draw(Frame frame)
    {
        VectorInt windowSize = (Console.WindowWidth, Console.WindowHeight);
        if (windowSize != _lastWindowSize)
        {
            CleanOutput();
        }

        string output = null;
        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                if (!frame.EqualsAt(_lastFrame, (x, y)))
                {
                    output +=
                    $"\u001b[{y + 1};{x + 1}H" + // Go to the position
                    $"\u001b[{(int)frame.Colors[x, y]}m" + // Apply the background color 
                    frame.Text[x, y]; // Add the character 
                }
            }
        }

        Console.Write(output);
        _lastFrame = frame;
        _lastWindowSize = windowSize;
    }
}

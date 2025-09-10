namespace Termule.Rendering;

public static partial class Display
{
    static Frame lastFrame = new Frame(0, 0);
    static VectorInt lastWindowSize;

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

    static void CleanOutput()
    {
        Console.ResetColor();
        Console.Clear();

        lastFrame = new Frame(0, 0);
    }

    public static void Draw(Frame frame)
    {
        VectorInt windowSize = (Console.WindowWidth, Console.WindowHeight);
        if (windowSize != lastWindowSize)
        {
            CleanOutput();
        }

        string output = null;
        for (int x = 0; x < frame.size.x; x++)
        {
            for (int y = 0; y < frame.size.y; y++)
            {
                if (!frame.EqualsAt(lastFrame, x, y))
                {
                    output +=
                    $"\u001b[{y + 1};{x + 1}H" + // Go to the position
                    $"\u001b[{(int) frame.GetColor(x, y)}m" + // Apply the background color 
                    frame.GetText(x, y); // Add the character 
                }
            }
        }

        Console.Write(output);
        lastFrame = frame;
        lastWindowSize = windowSize;
    }
}

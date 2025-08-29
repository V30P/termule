namespace Termule.Rendering;

public static class RenderSystem
{
    internal static readonly List<Renderer> renderers = [];

    internal static Frame frame { get; private set; }

    public static void DrawFrame(Vector viewCenter, (int x, int y) viewSize)
    {
        frame = new Frame(viewSize.x, viewSize.y);
        foreach (Renderer renderer in renderers)
        {
            renderer.Render(frame, viewCenter + new Vector(-viewSize.x / 2, viewSize.y / 2), viewSize);
        }

        string renderedFrame = "\u001b[?25l\u001b[0;0H"; // Hide the cursor, go to (0, 0)
        for (int y = 0; y < frame.sizeY; y++)
        {
            for (int x = 0; x < frame.sizeX; x++)
            {
                renderedFrame += $"\u001b[{(int) frame.image[x, y]}m{frame.text[x, y]}"; // Switch to the correct background color and print a space
            }

            renderedFrame += "\u001b[E"; // Go to the start of the next line
        }
        renderedFrame += "\u001b[?25h"; // Unhide the cursor

        Console.Write(renderedFrame);
    }
}
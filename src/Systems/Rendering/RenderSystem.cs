namespace Termule.Rendering;

public static class RenderSystem
{
    public static readonly Vector viewportCenter = new Vector(0, 0);
    public static readonly int sizeX = Console.WindowWidth, sizeY = Console.WindowHeight;

    public static readonly List<Renderer> renderers = [];

    static RenderSystem()
    {
        Console.CursorVisible = false;
    }

    public static Frame GetFrame()
    {
        Frame frame = new Frame();
        foreach (Renderer renderer in new List<Renderer>(renderers))
        {
            renderer.RenderTo(frame);
        }

        return frame;
    }

    internal static void Render(Frame frame)
    {
        string renderedFrame = "\u001b[0;0H"; //Go to (0, 0)
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                renderedFrame += $"\u001b[{(int)frame.backgroundColor[x, y]}m ";
            }

            renderedFrame += "\u001b[E"; //Go to the start of the next line
        }

        Console.Write(renderedFrame);
    }
}


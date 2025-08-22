namespace Termule.Rendering;

public class RenderSystem : Component
{
    internal readonly int sizeX = Console.WindowWidth, sizeY = Console.WindowHeight;
    internal readonly List<Renderer> renderers = [];

    public RenderSystem()
    {
        ConfigureConsole();

        Ticked += RenderToWindow;
        Destroyed += ResetConsole;
    }

    static void ConfigureConsole()
    {
        Console.CursorVisible = false;
    }

    void RenderToWindow()
    {
        Frame frame = new Frame(this);
        foreach (Renderer renderer in renderers)
        {
            renderer.RenderTo(frame);
        }

        Console.Write(frame.ToString());
    }

    static void ResetConsole()
    {
        Console.CursorVisible = true;
    }
}
namespace Termule.Rendering;

internal class RenderSystem : Component
{
    internal readonly int sizeX = Console.WindowWidth, sizeY = Console.WindowHeight;
    internal readonly List<Renderer> renderers = [];

    internal readonly Window window = new Window(Window.ReadMode.NewlineTerminated);

    internal RenderSystem()
    {
        Ticked += RenderToWindow;
        Destroyed += window.Close;
    }

    void RenderToWindow()
    {
        Frame frame = new Frame(this);
        foreach (Renderer renderer in renderers)
        {
            renderer.RenderTo(frame);
        }

        window.writer.WriteLine(frame.ToString());
    }
}


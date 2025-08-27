namespace Termule.Rendering;

public static class RenderSystem
{
    internal static readonly int sizeX = Console.WindowWidth, sizeY = Console.WindowHeight;
    internal static readonly List<Renderer> renderers = [];

    internal static void Render()
    {
        Frame frame = new Frame();
        foreach (Renderer renderer in renderers)
        {
            renderer.RenderTo(frame);
        }

        Console.Write(frame.ToString());
    }
}
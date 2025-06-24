namespace Termule.Rendering;

internal class RenderSystem()
{
    internal readonly int sizeX = Console.WindowWidth, sizeY = Console.WindowHeight;
    internal readonly List<Renderer> renderers = [];

    internal Frame GetFrame()
    {
        Frame frame = new Frame(this);
        foreach (Renderer renderer in new List<Renderer>(renderers))
        {
            renderer.RenderTo(frame);
        }

        return frame;
    }
}


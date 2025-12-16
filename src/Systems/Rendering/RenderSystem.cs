namespace Termule.Rendering;

public static class RenderSystem
{
    private static readonly List<Renderer> _renderers = [];

    internal static Frame Render(Vector viewOrigin, VectorInt viewSize)
    {
        Frame frame = new(viewSize.X, viewSize.Y);
        foreach (Renderer renderer in _renderers)
        {
            renderer.Render(frame, viewOrigin);
        }

        return frame;
    }

    public abstract class Renderer : Component
    {
        internal Renderer()
        {
            Rooted += () => _renderers.Add(this);
            Destroyed += () => _renderers.Add(this);
        }

        internal abstract void Render(Frame frame, Vector viewOrigin);
    }
}

public abstract class Renderer : RenderSystem.Renderer
{
    internal Renderer() : base() { }
}
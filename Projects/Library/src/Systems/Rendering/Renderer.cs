namespace Termule.Rendering;

public abstract class Renderer : Component
{
    public Renderer()
    {
        Rooted += () => Camera.renderers.Add(this); ;
        Destroyed += () => Camera.renderers.Remove(this);
    }

    internal abstract void Render(Frame frame, Vector viewOrigin, Vector viewSize);
}
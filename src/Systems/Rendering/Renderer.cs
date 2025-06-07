namespace Termule.Rendering;

public abstract class Renderer : Behavior
{
    public Renderer()
    {
        RenderSystem.renderers.Add(this);

        Destroyed += () => RenderSystem.renderers.Remove(this);
    }

    internal abstract void RenderTo(Frame frame);
}
namespace Termule.Rendering;

public abstract class Renderer : Component
{
    public Renderer()
    {
        Rooted += Register;
        Destroyed += Unregister;
    }

    void Register()
    {
        RenderSystem.renderers.Add(this);   
    }

    void Unregister()
    {
        RenderSystem.renderers.Remove(this);
    }

    internal abstract void RenderTo(Frame frame);
}
namespace Termule.Rendering;

public abstract class Renderer : Component
{
    RenderSystem renderSystem;

    public Renderer()
    {
        Spawned += Register;
        Destroyed += Unregister;
    }

    void Register()
    {
        renderSystem = game.Get<RenderSystem>();
        renderSystem.renderers.Add(this);   
    }

    void Unregister()
    {
        renderSystem.renderers.Remove(this);
    }

    internal abstract void RenderTo(Frame frame);
}
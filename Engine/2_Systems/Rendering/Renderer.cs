namespace Termule.Rendering;

public abstract class Renderer : Component
{
    public Renderer()
    {
        Spawned += Register;
        Destroyed += Unregister;
    }

    void Register() => game.Get<RenderSystem>().renderers.Add(this);
    void Unregister() => game.Get<RenderSystem>().renderers.Remove(this);

    internal abstract void RenderTo(Frame frame);
}
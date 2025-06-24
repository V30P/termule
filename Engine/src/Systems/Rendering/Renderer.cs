namespace Termule.Rendering;

public abstract class Renderer : Behavior
{
    public Renderer()
    {
        Spawned += Register;
    }

    void Register()
    {
        game.renderSystem.renderers.Add(this);
        Destroyed += () => game.renderSystem.renderers.Remove(this);
    }

    internal abstract void RenderTo(Frame frame);
}
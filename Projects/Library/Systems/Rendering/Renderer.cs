namespace Termule.Rendering;

public abstract class Renderer : Component
{
    private bool _rooted;

    public Layer Layer
    {
        get => _layer;

        set
        {
            if (_rooted)
            {
                Layer.Register(this);
            }

            _layer = value;
        }
    }
    private Layer _layer = RenderSystem.DefaultLayer;

    public Renderer()
    {
        Rooted += OnRooted;
        Destroyed += () => Layer.Unregister(this);
    }

    private void OnRooted()
    {
        _rooted = true;
        Layer.Register(this);
    }

    internal abstract void Render(Frame frame, Vector viewOrigin);
}
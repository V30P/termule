namespace Termule.Components;

using Core;
using Systems.RenderSystem;
using Types;

public abstract class Renderer : Component
{
    public Renderer()
    {
        this.Registered += () => ((IHostLayer)this.Layer).Register(this);
        this.Unregistered += () => ((IHostLayer)this.Layer).Unregister(this);
    }

    public Layer Layer
    {
        get => field ?? this.GetRequiredSystem<RenderSystem>().DefaultLayer;

        set
        {
            IHostLayer previousLayer = field;
            IHostLayer newLayer = field = value;

            if (this.IsRegistered)
            {
                previousLayer.Unregister(this);
                field = value;
                newLayer.Register(this);
            }
        }
    }

    protected internal abstract void Render(Frame frame, Vector viewOrigin);
}
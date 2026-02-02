using Termule.Core;
using Termule.Systems.RenderSystem;
using Termule.Types;

namespace Termule.Components;

public abstract class Renderer : Component
{
    private bool _registeredToGame;

    public Layer Layer
    {
        get => field ?? GetRequiredSystem<RenderSystem>().DefaultLayer;

        set
        {
            IHostLayer previousLayer = field;
            IHostLayer newLayer = field = value;

            if (_registeredToGame)
            {
                previousLayer.Unregister(this);
                field = value;
                newLayer.Register(this);
            }
        }
    }

    public Renderer()
    {
        Registered += OnRegistered;
        Unregistered += OnUnregistered;
    }

    private void OnRegistered()
    {
        _registeredToGame = true;
        ((IHostLayer)Layer).Register(this);
    }

    private void OnUnregistered()
    {
        _registeredToGame = false;
        ((IHostLayer)Layer).Unregister(this);
    }

    protected internal abstract void Render(Frame frame, Vector viewOrigin);
}
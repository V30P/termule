using Termule.Core;
using Termule.Types;

namespace Termule.Systems.RenderSystem;

public abstract class RenderSystem : Core.System
{
    protected readonly List<Renderer> Renderers = [];

    internal abstract Frame Render(Vector viewOrigin, VectorInt viewSize);

    public abstract class Renderer : Component
    {
        internal Renderer()
        {
            Registered += Register;
            Unregistered += Unregister;
        }

        private void Register()
        {
            Game.Systems.Get<RenderSystem>().Renderers.Add(this);
        }

        private void Unregister()
        {
            Game.Systems.Get<RenderSystem>().Renderers.Remove(this);
        }

        internal abstract void Render(Frame frame, Vector viewOrigin);
    }
}
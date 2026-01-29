using Termule.Core;
using Termule.Types;

namespace Termule.Systems.RenderSystem;

public sealed class RenderSystem : Core.System
{
    private readonly List<Renderer> _renderers = [];

    internal Frame Render(Vector viewOrigin, VectorInt viewSize)
    {
        Frame frame = new(viewSize.X, viewSize.Y);
        foreach (Renderer renderer in _renderers)
        {
            renderer.Render(frame, viewOrigin);
        }

        return frame;
    }

    public abstract class Renderer : Component
    {
        internal Renderer()
        {
            Registered += Register;
            Unregistered += Unregister;
        }

        private void Register()
        {
            GetRequiredSystem<RenderSystem>()._renderers.Add(this);
        }

        private void Unregister()
        {
            GetRequiredSystem<RenderSystem>()._renderers.Remove(this);
        }

        protected internal abstract void Render(Frame frame, Vector viewOrigin);
    }
}
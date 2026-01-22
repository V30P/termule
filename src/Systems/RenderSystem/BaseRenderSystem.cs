using Termule.Types;

namespace Termule.Systems.RenderSystem;

public sealed class BaseRenderSystem : RenderSystem
{
    internal BaseRenderSystem() { }

    internal override Frame Render(Vector viewOrigin, VectorInt viewSize)
    {
        Frame frame = new(viewSize.X, viewSize.Y);
        foreach (Renderer renderer in Renderers)
        {
            renderer.Render(frame, viewOrigin);
        }

        return frame;
    }
}
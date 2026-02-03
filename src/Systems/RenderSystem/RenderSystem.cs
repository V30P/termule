namespace Termule.Systems.RenderSystem;

using Components;
using Types;

public sealed class RenderSystem : Core.System
{
    public Layer[] Layers
    {
        private get;

        init
        {
            if (value?.Length == 0)
            {
                throw new ArgumentException($"{nameof(Layers)} cannot be null or empty");
            }

            field = value;
        }
    }

    = [new SimpleLayer()];

    public Layer DefaultLayer => this.Layers[0];

    internal Frame Render(Vector viewOrigin, Content background)
    {
        Frame frame = new(background);
        foreach (Layer layer in this.Layers)
        {
            foreach (Renderer renderer in layer)
            {
                renderer.Render(frame, viewOrigin);
            }
        }

        return frame;
    }
}
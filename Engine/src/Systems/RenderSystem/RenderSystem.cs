namespace Termule.Systems.RenderSystem;

using Components;
using Types;

/// <summary>
/// The <see cref="System"/> responsible for getting the rendering contributions according to Layer order.
/// </summary>
public sealed class RenderSystem : Core.System
{
    /// <summary>
    /// Gets or initializes the Layers that Renderers should exist in.
    /// </summary>
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

    /// <summary>
    /// Gets the DefaultLayer that Renderers without a specified Layer should exist in.
    /// </summary>
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
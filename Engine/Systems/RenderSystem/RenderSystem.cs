using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Systems.RenderSystem;

/// <summary>
///     System responsible for collecting rendering contributions according to layer order.
/// </summary>
public sealed class RenderSystem : Core.System
{
    /// <summary>
    ///     Gets or initializes the rendering layers.
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
    ///     Gets the default layer for renderers without an explicit layer.
    /// </summary>
    public Layer DefaultLayer => Layers[0];

    internal void Render(Vector viewOrigin, FrameBuffer frame)
    {
        foreach (var layer in Layers)
        foreach (var renderer in layer)
        {
            renderer.Render(frame, viewOrigin);
        }
    }
}
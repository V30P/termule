using System.Collections.ObjectModel;
using Termule.Engine.Components;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Rendering;

/// <summary>
///     System responsible for building <see cref="FrameBuffer"/>s from <see cref="Renderer"/>s
///     in <see cref="Layer"/> > order.
/// </summary>
public sealed class RenderSystem : Core.System
{
    /// <summary>
    ///     Gets or initializes the rendering layers.
    /// </summary>
    public ReadOnlyCollection<Layer> Layers
    {
        get;

        init
        {
            if (value is null or { Count: 0 })
            {
                throw new ArgumentException($"{nameof(Layers)} cannot be null or empty");
            }

            field = value;
        }
    } = [new SimpleLayer()];

    /// <summary>
    ///     Gets the default layer for renderers without an explicit layer.
    /// </summary>
    public Layer DefaultLayer => Layers[0];

    internal void Render(Vector viewOrigin, FrameBuffer frame)
    {
        foreach (Layer layer in Layers)
        {
            foreach (Renderer renderer in layer)
            {
                renderer.Render(frame, viewOrigin);
            }
        }
    }
}

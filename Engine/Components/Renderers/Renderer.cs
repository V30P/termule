using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.RenderSystem;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Base component class for contributing to frames produced by the render system.
/// </summary>
public abstract class Renderer : Component
{
    private Layer layer;

    /// <summary>
    ///     Gets or sets the render <see cref="Layer" />.
    ///     If set to <c>null</c>, the default <see cref="Layer" /> will be used.
    /// </summary>
    public Layer Layer
    {
        get => layer;

        set
        {
            if (!IsRegistered)
            {
                layer = value;
                return;
            }

            value ??= GetRequiredSystem<RenderSystem>().DefaultLayer;

            layer.Unregister(this);
            layer = value;
            layer.Register(this);
        }
    }


    /// <summary>
    ///     Initializes a new instance of the <see cref="Renderer" /> class.
    /// </summary>
    protected Renderer()
    {
        Registered += RegisterToLayer;
        Unregistered += UnregisterFromLayer;
    }

    /// <summary>
    ///     Renders to the provided <see cref="FrameBuffer" /> by contributing <see cref="FrameBuffer" /> changes.
    /// </summary>
    /// <param name="frame">The target frame to contribute to.</param>
    /// <param name="viewOrigin">The origin of the view in game space.</param>
    protected internal abstract void Render(FrameBuffer frame, Vector viewOrigin);

    private void RegisterToLayer()
    {
        layer ??= GetRequiredSystem<RenderSystem>().DefaultLayer;
        layer.Register(this);
    }

    private void UnregisterFromLayer()
    {
        layer.Unregister(this);
    }
}
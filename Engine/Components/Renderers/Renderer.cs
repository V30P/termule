using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Base component for rendering into <see cref="IRenderTarget"/>s via the
///     <see cref="RenderSystem"/>.
/// </summary>
public abstract class Renderer : Component
{
    private Layer layer;

    /// <summary>
    ///     Gets or sets this renderer's layer. If set to <see langword="null"/>, the
    ///     <see cref="RenderSystem"/>'s default layer will used.
    /// </summary>
    public Layer Layer
    {
        get => layer;

        set
        {
            if (!Activated)
            {
                layer = value;
                return;
            }

            value ??= GetRequiredSystem<RenderSystem>().DefaultLayer;

            layer.Remove(this);
            layer = value;
            layer.Add(this);
        }
    }

    /// <summary>
    ///     Renders to the provided <see cref="IRenderTarget" />.
    /// </summary>
    /// <param name="target">The target to render to.</param>
    /// <param name="viewOrigin">The origin of the view in world-space.</param>
    protected internal abstract void Render(IRenderTarget target, Vector viewOrigin);

    /// <inheritdoc />
    protected override void Activate()
    {
        layer ??= GetRequiredSystem<RenderSystem>().DefaultLayer;
        layer.Add(this);
    }

    /// <inheritdoc />
    protected override void Deactivate()
    {
        layer.Remove(this);
    }
}

namespace Termule.Components;

using Core;
using Systems.RenderSystem;
using Types;

/// <summary>
/// Base component class for contributing to frames produced by the render system.
/// </summary>
public abstract class Renderer : Component
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Renderer"/> class.
    /// </summary>
    public Renderer()
    {
        this.Registered += () => ((IHostLayer)this.Layer).Register(this);
        this.Unregistered += () => ((IHostLayer)this.Layer).Unregister(this);
    }

    /// <summary>
    /// Gets or sets the render <see cref="Layer"/>.
    /// If set to <c>null</c>, the default <see cref="Layer"/> will be used.
    /// </summary>
    public Layer Layer
    {
        get => field ?? this.GetRequiredSystem<RenderSystem>().DefaultLayer;

        set
        {
            IHostLayer previousLayer = field;
            if (this.IsRegistered)
            {
                previousLayer.Unregister(this);
                field = value;
                ((IHostLayer)this.Layer).Register(this);
            }
        }
    }

    /// <summary>
    /// Renders to the provided <see cref="Frame"/> by contributing <see cref="Cell"/> changes.
    /// </summary>
    /// <param name="frame">The target frame to contribute to.</param>
    /// <param name="viewOrigin">The origin of the view in game space.</param>
    protected internal abstract void Render(Frame frame, Vector viewOrigin);
}
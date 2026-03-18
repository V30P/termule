namespace Termule.Components;

using Core;
using Systems.RenderSystem;
using Types;

/// <summary>
/// Base component class for contributing to frames produced by the render system.
/// </summary>
public abstract class Renderer : Component
{
    private IHostLayer layer;

    /// <summary>
    /// Initializes a new instance of the <see cref="Renderer"/> class.
    /// </summary>
    public Renderer()
    {
        this.Registered += this.RegisterToLayer;
        this.Unregistered += this.UnregisterFromLayer;
    }

    /// <summary>
    /// Gets or sets the render <see cref="Layer"/>.
    /// If set to <c>null</c>, the default <see cref="Layer"/> will be used.
    /// </summary>
    public Layer Layer
    {
        get => (Layer)this.layer;

        set
        {
            if (!this.IsRegistered)
            {
                this.layer = value;
                return;
            }

            value ??= this.GetRequiredSystem<RenderSystem>().DefaultLayer;

            this.layer.Unregister(this);
            this.layer = value;
            this.layer.Register(this);
        }
    }

    /// <summary>
    /// Renders to the provided <see cref="Frame"/> by contributing <see cref="Cell"/> changes.
    /// </summary>
    /// <param name="frame">The target frame to contribute to.</param>
    /// <param name="viewOrigin">The origin of the view in game space.</param>
    protected internal abstract void Render(Frame frame, Vector viewOrigin);

    private void RegisterToLayer()
    {
        this.layer ??= this.GetRequiredSystem<RenderSystem>().DefaultLayer;
        this.layer.Register(this);
    }

    private void UnregisterFromLayer()
    {
        this.layer.Unregister(this);
    }
}
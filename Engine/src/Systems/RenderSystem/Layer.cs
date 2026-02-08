namespace Termule.Systems.RenderSystem;

using System.Collections;
using Components;

/// <summary>
/// The base class for layers used to specify the rendering order of contained <see cref="Renderer"/>s.
/// </summary>
/// <param name="comparer">The comparer to use for ordering renderers.</param>
public abstract class Layer(IComparer<Renderer> comparer) : IHostLayer, IEnumerable<Renderer>
{
    private readonly List<Renderer> renderers = [];
    private readonly IComparer<Renderer> comparer = comparer;
    private bool rendererAdded;

    /// <summary>
    /// Initializes a new instance of the <see cref="Layer"/> class.
    /// </summary>
    /// <param name="comparison">The comparison used to create the comparer for renderers.</param>
    public Layer(Comparison<Renderer> comparison)
    : this(Comparer<Renderer>.Create(comparison))
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether a change has occurred that necessitates re-sorting.
    /// </summary>
    protected bool Dirty { get; set; }

    void IHostLayer.Register(Renderer renderer)
    {
        this.renderers.Add(renderer);
        this.rendererAdded = true;

        this.OnRendererRegistered(renderer);
    }

    void IHostLayer.Unregister(Renderer renderer)
    {
        this.renderers.Remove(renderer);

        this.OnRendererUnregistered(renderer);
    }

    /// <inheritdoc/>
    public IEnumerator<Renderer> GetEnumerator()
    {
        if (this.rendererAdded || this.Dirty)
        {
            this.renderers.Sort(this.comparer);
            this.Dirty = false;
        }

        return this.renderers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Invoked when a new Renderer is added.
    /// </summary>
    /// <param name="renderer">The renderer that was added.</param>
    protected virtual void OnRendererRegistered(Renderer renderer)
    {
    }

    /// <summary>
    /// Invoked when a renderer is removed.
    /// </summary>
    /// <param name="renderer">The renderer that was removed.</param>
    protected virtual void OnRendererUnregistered(Renderer renderer)
    {
    }
}

internal interface IHostLayer
{
    internal void Register(Renderer renderer);

    internal void Unregister(Renderer renderer);
}

/// <summary>
/// A basic layer implementation that provides registration-order-based sorting.
/// </summary>
public sealed class SimpleLayer : Layer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleLayer"/> class.
    /// </summary>
    public SimpleLayer()
        : base((r1, r2) => r1.ElementID.CompareTo(r2.ElementID))
    {
    }
}

using System.Collections;
using Termule.Engine.Components;

namespace Termule.Engine.Systems.RenderSystem;

/// <summary>
///     Base class for that specifies the rendering order of contained renderers.
/// </summary>
/// <param name="comparer">The comparer to use for ordering renderers.</param>
public abstract class Layer(IComparer<Renderer> comparer) : IEnumerable<Renderer>
{
    private readonly List<Renderer> renderers = [];

    private bool rendererAdded;

    /// <summary>
    ///     Gets or sets a value indicating whether a change has occurred that requires re-sorting.
    /// </summary>
    private bool Dirty { get; set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Layer" /> class.
    /// </summary>
    /// <param name="comparison">The comparison used to create the comparer for renderers.</param>
    protected Layer(Comparison<Renderer> comparison)
        : this(Comparer<Renderer>.Create(comparison))
    {
    }

    /// <inheritdoc />
    public IEnumerator<Renderer> GetEnumerator()
    {
        if (!rendererAdded && !Dirty)
        {
            return renderers.GetEnumerator();
        }

        renderers.Sort(comparer);
        Dirty = false;

        return renderers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal void Register(Renderer renderer)
    {
        renderers.Add(renderer);
        rendererAdded = true;

        OnRendererRegistered(renderer);
    }

    internal void Unregister(Renderer renderer)
    {
        renderers.Remove(renderer);

        OnRendererUnregistered(renderer);
    }

    /// <summary>
    ///     Invoked when a new renderer is added.
    /// </summary>
    /// <param name="renderer">The renderer that was added.</param>
    protected virtual void OnRendererRegistered(Renderer renderer)
    {
    }

    /// <summary>
    ///     Invoked when a renderer is removed.
    /// </summary>
    /// <param name="renderer">The renderer that was removed.</param>
    protected virtual void OnRendererUnregistered(Renderer renderer)
    {
    }
}
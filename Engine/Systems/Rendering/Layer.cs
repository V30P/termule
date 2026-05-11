using System.Collections;
using Termule.Engine.Components;

namespace Termule.Engine.Systems.Rendering;

/// <summary>
///     Base class for that specifies the rendering order of contained renderers.
/// </summary>
/// <param name="comparer">The comparer to use for ordering renderers.</param>
public abstract class Layer(IComparer<Renderer> comparer) : IEnumerable<Renderer>
{
    private readonly List<Renderer> renderers = [];

    /// <summary>
    ///     Gets or sets a value indicating whether a change has occurred that requires re-sorting.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    protected bool Dirty;

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
        if (Dirty)
        {
            renderers.Sort(comparer);
            Dirty = false;
        }

        return renderers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal void Add(Renderer renderer)
    {
        renderers.Add(renderer);
        OnRendererAdded(renderer);
    }

    internal void Remove(Renderer renderer)
    {
        renderers.Remove(renderer);
        OnRendererRemoved(renderer);
    }

    /// <summary>
    ///     Invoked when a new renderer is added. This should be used to update dirtiness.
    /// </summary>
    /// <param name="renderer">The renderer that was added.</param>
    protected virtual void OnRendererAdded(Renderer renderer)
    {
        Dirty = true;
    }

    /// <summary>
    ///     Invoked when a renderer is removed. This should be used to update dirtiness.
    /// </summary>
    /// <param name="renderer">The renderer that was removed.</param>
    protected virtual void OnRendererRemoved(Renderer renderer)
    {
        Dirty = true;
    }
}
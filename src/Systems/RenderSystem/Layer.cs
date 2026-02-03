namespace Termule.Systems.RenderSystem;

using System.Collections;
using Components;

public abstract class Layer(IComparer<Renderer> comparer) : IHostLayer, IEnumerable<Renderer>
{
    private readonly List<Renderer> renderers = [];
    private readonly IComparer<Renderer> comparer = comparer;
    private bool rendererAdded;

    public Layer(Comparison<Renderer> comparison)
    : this(Comparer<Renderer>.Create(comparison))
    {
    }

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

    protected virtual void OnRendererRegistered(Renderer renderer)
    {
    }

    protected virtual void OnRendererUnregistered(Renderer renderer)
    {
    }
}

internal interface IHostLayer
{
    internal void Register(Renderer renderer);

    internal void Unregister(Renderer renderer);
}

public sealed class SimpleLayer : Layer
{
    public SimpleLayer()
        : base((r1, r2) => r1.ElementID.CompareTo(r2.ElementID))
    {
    }
}

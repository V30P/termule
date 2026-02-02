using System.Collections;
using Termule.Components;

namespace Termule.Systems.RenderSystem;

public abstract class Layer(IComparer<Renderer> comparer) : IHostLayer, IEnumerable<Renderer>
{
    private readonly List<Renderer> _renderers = [];
    private readonly IComparer<Renderer> _comparer = comparer;

    protected abstract bool Dirty { get; }
    private bool _rendererAdded;

    public Layer(Comparison<Renderer> comparison) : this(Comparer<Renderer>.Create(comparison)) { }

    void IHostLayer.Register(Renderer renderer)
    {
        _renderers.Add(renderer);
        _rendererAdded = true;

        OnRendererRegistered(renderer);
    }

    protected virtual void OnRendererRegistered(Renderer renderer) { }

    void IHostLayer.Unregister(Renderer renderer)
    {
        _renderers.Remove(renderer);

        OnRendererUnregistered(renderer);
    }

    protected virtual void OnRendererUnregistered(Renderer renderer) { }

    public IEnumerator<Renderer> GetEnumerator()
    {
        if (_rendererAdded || Dirty)
        {
            _renderers.Sort(_comparer);
            _rendererAdded = false;
        }

        return _renderers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal interface IHostLayer
{
    internal void Register(Renderer renderer);
    internal void Unregister(Renderer renderer);
}

public sealed class SimpleLayer : Layer
{
    protected override bool Dirty => false;

    public SimpleLayer() : base((r1, r2) => r1.ElementID.CompareTo(r2.ElementID)) { }
}

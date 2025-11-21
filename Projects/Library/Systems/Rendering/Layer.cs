using System.Collections;

namespace Termule.Rendering;

public class Layer(IComparer<Renderer> comparer = null) : IEnumerable<Renderer>
{
    private static readonly Comparer<Renderer> _defaultComparer =
        Comparer<Renderer>.Create((r1, r2) => r1.GetHashCode().CompareTo(r2.GetHashCode()));
    private readonly SortedList<Renderer, Renderer> _renderers = new(comparer ?? _defaultComparer);

    internal void Register(Renderer renderer)
    {
        _renderers.Add(renderer, renderer);
    }

    internal void Unregister(Renderer renderer)
    {
        _renderers.Remove(renderer);
    }

    public IEnumerator<Renderer> GetEnumerator()
    {
        return _renderers.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
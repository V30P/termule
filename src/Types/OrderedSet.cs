using System.Collections;

namespace Termule;

public sealed class OrderedSet<T> : IEnumerable<T>
{
    private readonly HashSet<T> _set = [];
    private readonly List<T> _list = [];

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        if (_set.Add(item))
        {
            _list.Add(item);
        }
    }

    public void Remove(T item)
    {
        _set.Remove(item);
        _list.Remove(item);
    }
}
using System.Collections;

namespace Termule.Systems.Controller;

public sealed class BindMap : IEnumerable<KeyValuePair<string, Bind>>
{
    internal Controller Controller
    {
        set
        {
            _controller = value;

            foreach (Bind bind in _binds.Values)
            {
                bind.SetController(value);
            }
        }
    }
    private Controller _controller;

    private readonly Dictionary<string, Bind> _binds = [];

    public Bind this[string name]
    {
        get => _binds[name];

        set
        {
            _binds[name].SetController(null);
            _binds[name] = value;
            value.SetController(_controller);
        }
    }

    public void Add(string name, Bind bind)
    {
        _binds.Add(name, bind);
        bind.SetController(_controller);
    }

    public void Remove(string name)
    {
        Bind bind = _binds[name];
        _binds.Remove(name);
        bind.SetController(null);
    }

    IEnumerator<KeyValuePair<string, Bind>> IEnumerable<KeyValuePair<string, Bind>>.GetEnumerator()
    {
        return _binds.GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, Bind>>)this).GetEnumerator();
    }
}
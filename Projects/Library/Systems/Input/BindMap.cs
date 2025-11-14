using System.Collections;

namespace Termule.Input;

public class BindMap : IEnumerable<KeyValuePair<string, Bind>>
{
    internal bool Active
    {
        get => _active;

        set
        {
            _active = value;

            foreach (Bind control in _controls.Values)
            {
                control.Active = Active;
            }
        }
    }
    internal bool _active;

    private readonly Dictionary<string, Bind> _controls = [];

    public Bind this[string name]
    {
        get => _controls[name];

        set
        {
            _controls[name].Active = false;
            _controls[name] = value;
            value.Active = Active;
        }
    }

    public void Add(string name, Bind control)
    {
        _controls.Add(name, control);
        control.Active = Active;
    }

    public void Remove(string name)
    {
        Bind control = _controls[name];
        _controls.Remove(name);
        control.Active = false;
    }

    public IEnumerator<KeyValuePair<string, Bind>> GetEnumerator()
    {
        return _controls.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
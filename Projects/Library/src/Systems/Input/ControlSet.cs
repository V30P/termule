using System.Collections;

namespace Termule.Input;

public class ControlSet : IEnumerable<KeyValuePair<string, Control>>
{
    internal bool active
    {
        get => _active;

        set
        {
            _active = value;
            
            foreach (Control control in nameToControl.Values)
            {
                control.active = active;
            }
        }
    }
    internal bool _active;

    readonly Dictionary<string, Control> nameToControl = [];

    public Control this[string name]
    {
        get => nameToControl[name];

        set
        {
            nameToControl[name].active = false;
            nameToControl[name] = value;
            value.active = active;
        }
    }

    public void Add(string name, Control control)
    {
        nameToControl.Add(name, control);
        control.active = active;
    }

    public void Remove(string name)
    {
        Control control = nameToControl[name];
        nameToControl.Remove(name);
        control.active = false;
    }

    public IEnumerator<KeyValuePair<string, Control>> GetEnumerator() => nameToControl.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
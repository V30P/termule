namespace Termule.Input;

public class InputSystem : Component
{
    public ControlSet controls
    {
        get => _controls;

        set
        {
            if (controls != null) controls.active = false;
            _controls = value;
            if (controls != null) controls.active = true;
        }
    }
    ControlSet _controls;

    Dictionary<string, object> values = [];

    public InputSystem()
    {
        Ticked += GatherControlValues;
    }

    void GatherControlValues()
    {
        values = [];
        foreach (KeyValuePair<string, Control> controlPair in controls)
        {
            values.Add(controlPair.Key, controlPair.Value.GetValue());
        }
    }

    public T Get<T>(string name) => (T) values[name];
}
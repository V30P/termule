namespace Termule.Input;

public static class InputSystem
{
    public static ControlSet controls
    {
        get => _controls;

        set
        {
            if (controls != null) controls.active = false;
            _controls = value;
            if (controls != null) controls.active = true;
        }
    }
    static ControlSet _controls;

    static Dictionary<string, object> values = [];

    public static void GetInputs()
    {
        values = [];
        foreach (KeyValuePair<string, Control> controlPair in controls)
        {
            values.Add(controlPair.Key, controlPair.Value.GetValue());
        }
    }

    public static T Get<T>(string name) => (T) values[name];
}
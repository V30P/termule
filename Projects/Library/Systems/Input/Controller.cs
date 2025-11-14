namespace Termule.Input;

public static class Controller
{
    public static BindMap Controls
    {
        get => _binds;

        set
        {
            Controls.Active = false;
            _binds = value;
            Controls.Active = true;
        }
    }
    private static BindMap _binds = [];

    private static Dictionary<string, object> _values = [];

    public static void UpdateValues()
    {
        _values = [];
        foreach (KeyValuePair<string, Bind> controlPair in Controls)
        {
            _values.Add(controlPair.Key, controlPair.Value.GetValue());
        }
    }

    public static T Get<T>(string name)
    {
        return (T)_values[name];
    }
}
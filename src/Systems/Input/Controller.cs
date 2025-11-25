namespace Termule.Input;

public static class Controller
{
    public static BindMap Binds
    {
        get => _binds;

        set
        {
            Binds.Active = false;
            _binds = value;
            Binds.Active = true;
        }
    }
    private static BindMap _binds = [];

    private static Dictionary<string, object> _values = [];

    public static void UpdateValues()
    {
        _values = [];
        foreach (KeyValuePair<string, Bind> controlPair in Binds)
        {
            _values.Add(controlPair.Key, controlPair.Value.GetValue());
        }
    }

    public static T Get<T>(string name)
    {
        return (T)_values[name];
    }
}
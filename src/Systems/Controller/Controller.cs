namespace Termule.Systems.Controller;

public abstract class Controller : Core.System
{
    public BindMap Binds
    {
        set
        {
            _binds = value;
            _binds.Controller = this;
        }
    }
    private BindMap _binds = [];

    private Dictionary<string, object> _values = [];

    private Controller() { }

    public TValue Get<TValue>(string name)
    {
        return (TValue)_values[name];
    }

    public abstract class GenericController<TBind> : Controller where TBind : Bind
    {
        internal GenericController() { }

        protected override void Update()
        {
            _values = [];
            foreach (KeyValuePair<string, Bind> bindPair in _binds)
            {
                _values.Add(bindPair.Key, bindPair.Value.GetValue());
            }
        }
    }
}

public abstract class Controller<TBind> : Controller.GenericController<TBind> where TBind : Bind
{
    internal Controller() { }
}
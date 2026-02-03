namespace Termule.Systems.Controller;

public abstract class Controller : Core.System
{
    private Dictionary<string, object> values = [];

    private Controller()
    {
    }

    public BindMap Binds
    {
        private get => field;

        set
        {
            field = value;
            field.Controller = this;
        }
    }

    = [];

    public TValue Get<TValue>(string name)
    {
        return (TValue)this.values[name];
    }

    public abstract class GenericController<TBind> : Controller
        where TBind : Bind
    {
        internal GenericController()
        {
        }

        protected override void Update()
        {
            this.values = [];
            foreach (KeyValuePair<string, Bind> bindPair in this.Binds)
            {
                this.values.Add(bindPair.Key, bindPair.Value.GetValue());
            }
        }
    }
}

public abstract class Controller<TBind> : Controller.GenericController<TBind>
    where TBind : Bind
{
    internal Controller()
    {
    }
}
namespace Termule.Systems.Controller;

/// <summary>
///     Non-generic base system class for controllers.
/// </summary>
public abstract class Controller : Core.System
{
    /// <summary>
    ///     Gets or sets the <see cref="BindMap" /> that this controller should use.
    /// </summary>
    public BindMap Binds
    {
        protected get;

        set
        {
            field = value;
            field.Controller = this;
        }
    }

        = [];

    /// <summary>
    ///     Gets or sets the values retrieved from <see cref="Bind" />s last tick.
    /// </summary>
    protected Dictionary<string, object> Values { get; set; } = [];

    internal Controller()
    {
    }

    /// <summary>
    ///     Gets the value of the bind with the given name from the last tick.
    /// </summary>
    /// <typeparam name="TValue">The type of value to get.</typeparam>
    /// <param name="name">The name of the bind to get the value for.</param>
    /// <returns> The value of the specified bind. </returns>
    public TValue Get<TValue>(string name)
    {
        return (TValue)Values[name];
    }
}

/// <summary>
///     Generic base system class that collects the values from <see cref="Bind" />s.
/// </summary>
/// <typeparam name="TBind">The type of bind this controller uses.</typeparam>
public abstract class Controller<TBind> : Controller
    where TBind : Bind
{
    internal Controller()
    {
    }

    /// <summary>
    ///     Updates the value associated with each <see cref="Bind" />.
    /// </summary>
    protected internal override void Tick()
    {
        Values = [];
        foreach (KeyValuePair<string, Bind> bindPair in Binds)
        {
            Values.Add(bindPair.Key, bindPair.Value.GetValue());
        }
    }
}
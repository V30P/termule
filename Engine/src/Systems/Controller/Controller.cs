namespace Termule.Systems.Controller;

/// <summary>
/// The non-generic base class for Controllers.
/// </summary>
public abstract class Controller : Core.System
{
    internal Controller()
    {
    }

    /// <summary>
    /// Gets or Sets the BindMap that this controller should use.
    /// </summary>
    public BindMap Binds
    {
        protected get => field;

        set
        {
            field = value;
            field.Controller = this;
        }
    }

    = [];

    /// <summary>
    /// Gets or sets the values retrieved from Binds last frame.
    /// </summary>
    protected Dictionary<string, object> Values { get; set; } = [];

    /// <summary>
    /// Gets the value provided by the Bind of given name last frame.
    /// </summary>
    /// <typeparam name="TValue"> The type of value to get. </typeparam>
    /// <param name="name">The name of the bind to get the value for.</param>
    /// <returns> The value of the specified Bind. </returns>
    public TValue Get<TValue>(string name)
    {
        return (TValue)this.Values[name];
    }
}

/// <summary>
/// The base <see cref="System"/> class for Controllers that manage <see cref="Bind"/>s.
/// </summary>
/// <typeparam name="TBind"> The type of Bind this Controller uses. </typeparam>
public abstract class Controller<TBind> : Controller
    where TBind : Bind
{
    internal Controller()
    {
    }

    /// <summary>
    /// Updates the value associated with each Bind.
    /// </summary>
    protected override void Update()
    {
        this.Values = [];
        foreach (KeyValuePair<string, Bind> bindPair in this.Binds)
        {
            this.Values.Add(bindPair.Key, bindPair.Value.GetValue());
        }
    }
}
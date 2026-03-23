using System.ComponentModel;

namespace Termule.Engine.Systems.Controller;

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
    } = [];
    
    
    /// <summary>
    ///     Updates the value associated with each <see cref="Bind" />.
    /// </summary>
    protected internal override void Tick()
    {
        Binds.PollValues();
    }

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
        if (!Binds.TryGetValue(name, out var value))
        {
            throw new ArgumentException($"No bind named '{name}' exists.");
        }

        return value is not TValue typedValue ? throw new ArgumentException($"A bind named '{name}' exists, but it is not of type '{typeof(TValue)}'.") : typedValue;
    }
}
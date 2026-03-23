using System.Collections;

namespace Termule.Engine.Systems.Controller;

/// <summary>
///     Collection to hold <see cref="Bind" />s for a <see cref="Systems.Controller.Controller" />.
/// </summary>
public sealed class BindMap : IEnumerable<KeyValuePair<string, Bind>>
{
    private readonly Dictionary<string, Bind> binds = [];
    private readonly Dictionary<string, object> values = [];

    internal Controller Controller
    {
        private get;

        set
        {
            field = value;

            foreach (var bind in binds.Values)
            {
                bind.SetController(value);
            }
        }
    }

    /// <summary>
    ///     Gets or sets the bind associated with the provided <paramref name="name" />.
    /// </summary>
    /// <param name="name">The name of the bind to look for.</param>
    /// <returns>The bind with name <paramref name="name" />.</returns>
    public Bind this[string name]
    {
        get => binds[name];

        set
        {
            binds[name].SetController(null);
            binds[name] = value;
            value.SetController(Controller);
        }
    }

    IEnumerator<KeyValuePair<string, Bind>> IEnumerable<KeyValuePair<string, Bind>>.GetEnumerator()
    {
        return binds.GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, Bind>>)this).GetEnumerator();
    }

    /// <summary>
    ///     Add a <see cref="Bind" /> with provided <paramref name="name" />.
    /// </summary>
    /// <param name="name">The name to add the bind under.</param>
    /// <param name="bind">The bind to add.</param>
    public void Add(string name, Bind bind)
    {
        bind.SetController(Controller);
        
        binds.Add(name, bind);
        values.Add(name, bind.GetValue());
    }

    /// <summary>
    ///     Remove the <see cref="Bind" /> with provided <paramref name="name" />.
    /// </summary>
    /// <param name="name">The name to remove the <see cref="Bind" /> for.</param>
    public void Remove(string name)
    {
        var bind = binds[name];
        binds.Remove(name);
        bind.SetController(null);
    }

    internal void PollValues()
    {
        foreach (var bindPair in binds)
        {
            values[bindPair.Key] = bindPair.Value.GetValue();
        }
    }

    internal bool TryGetValue(string name, out object value)
    {
        if (values.TryGetValue(name, out var existingValue))
        {
            value = existingValue;
            return true;
        }

        value = null;
        return false;
    }
}
using System.Collections;

namespace Termule.Engine.Systems.Input;

/// <summary>
///     Collection to manage <see cref="Bind" />s for a <see cref="Systems.Input.Keyboard" />.
/// </summary>
public sealed class BindMap : IEnumerable<KeyValuePair<string, Bind>>
{
    private readonly Dictionary<string, Bind> binds = [];
    private readonly Dictionary<string, object> values = [];

    internal Keyboard Keyboard
    {
        private get;

        set
        {
            field = value;

            foreach (Bind bind in binds.Values)
            {
                bind.Keyboard = value;
            }
        }
    }

    /// <summary>
    ///     Gets or sets the bind associated with the provided <paramref name="name" />.
    /// </summary>
    /// <param name="name">The name of the bind.</param>
    /// <returns>The corresponding bind.</returns>
    public Bind this[string name]
    {
        get => binds[name];

        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (binds.GetValueOrDefault(name) != null)
            {
                Remove(name);
            }

            Add(name, value);
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
    ///     Add a bind with provided <paramref name="name" />.
    /// </summary>
    /// <param name="name">The name to add the bind under.</param>
    /// <param name="bind">The bind to add.</param>
    public void Add(string name, Bind bind)
    {
        if (binds.ContainsKey(name))
        {
            throw new ArgumentException($"A bind with name '{name}' already exists.");
        }

        if (binds.ContainsValue(bind))
        {
            string existingName = binds.Where(p => p.Value == bind).Select(p => p.Key).First();
            throw new ArgumentException($"Bind '{bind}' is already added under the name '{existingName}'.");
        }

        bind.Keyboard = Keyboard;
        binds.Add(name, bind);
        values.Add(name, bind.GetValue());
    }

    /// <summary>
    ///     Remove the bind with provided <paramref name="name" />.
    /// </summary>
    /// <param name="name">The name to remove the bind for.</param>
    public void Remove(string name)
    {
        Bind bind = binds[name];
        binds.Remove(name);
        bind.Keyboard = null;
    }

    internal void PollValues()
    {
        foreach (KeyValuePair<string, Bind> bindPair in binds)
        {
            values[bindPair.Key] = bindPair.Value.GetValue();
        }
    }

    internal bool TryGetValue(string name, out object value)
    {
        if (values.TryGetValue(name, out object existingValue))
        {
            value = existingValue;
            return true;
        }

        value = null;
        return false;
    }
}
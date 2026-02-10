namespace Termule.Systems.Controller;

using System.Collections;

/// <summary>
/// Collection to hold <see cref="Bind"/>s for a <see cref="Systems.Controller.Controller"/>.
/// </summary>
public sealed class BindMap : IEnumerable<KeyValuePair<string, Bind>>
{
    private readonly Dictionary<string, Bind> binds = [];

    internal Controller Controller
    {
        private get;

        set
        {
            field = value;

            foreach (Bind bind in this.binds.Values)
            {
                bind.SetController(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets the bind associated with the provided <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the bind to look for.</param>
    /// <returns>The bind with name <paramref name="name"/>.</returns>
    public Bind this[string name]
    {
        get => this.binds[name];

        set
        {
            this.binds[name].SetController(null);
            this.binds[name] = value;
            value.SetController(this.Controller);
        }
    }

    /// <summary>
    /// Add a <see cref="Bind"/> with provided <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name to add the bind under.</param>
    /// <param name="bind">The bind to add.</param>
    public void Add(string name, Bind bind)
    {
        this.binds.Add(name, bind);
        bind.SetController(this.Controller);
    }

    /// <summary>
    /// Remove the <see cref="Bind"/> with provided <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name to remove the <see cref="Bind"/> for.</param>
    public void Remove(string name)
    {
        Bind bind = this.binds[name];
        this.binds.Remove(name);
        bind.SetController(null);
    }

    IEnumerator<KeyValuePair<string, Bind>> IEnumerable<KeyValuePair<string, Bind>>.GetEnumerator()
    {
        return this.binds.GetEnumerator();
    }

    /// <inheritdoc/>
    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, Bind>>)this).GetEnumerator();
    }
}
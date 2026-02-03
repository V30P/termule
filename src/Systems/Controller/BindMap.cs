namespace Termule.Systems.Controller;

using System.Collections;

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

    public void Add(string name, Bind bind)
    {
        this.binds.Add(name, bind);
        bind.SetController(this.Controller);
    }

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

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, Bind>>)this).GetEnumerator();
    }
}
using System.Collections;

namespace Termule;

public class GameObject : Component, IEnumerable<Component>
{
    readonly List<Component> components = [];

    public GameObject()
    {
        Ticked += TickComponents;
        Destroyed += DestroyComponents;
    }

    void TickComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.Tick();
        }
    }

    void DestroyComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.Destroy();
        }
    }

    public IEnumerator<Component> GetEnumerator()
    {
        lock (components) return components.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Component component)
    {
        lock (components)
        {
            components.Add(component);
            component.gameObject = this;
        }
    }

    public void Add(params Component[] components)
    {
        lock (components)
        {
            foreach (Component component in components)
            {
                Add(component);
            }
        }
    }

    public void Remove(Component component)
    {
        lock (components) components.Remove(component);
        component.gameObject = null;
    }

    public T Get<T>() where T : Component
    {
        foreach (Component component in components)
        {
            if (component is T foundByForce)
            {
                return foundByForce;
            }
        }

        return null;
    }
}
using System.Collections;

namespace Termule;

public class GameObject : Component, IEnumerable<Component>
{
    private readonly List<Component> _components = [];

    public GameObject()
    {
        Rooted += RootComponents;
        Ticked += TickComponents;
        Destroyed += DestroyComponents;
    }

    private void RootComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.IsRooted = true;
        }
    }

    private void TickComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.Tick();
        }
    }

    private void DestroyComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.Destroy();
        }
    }

    public IEnumerator<Component> GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Component component)
    {
        _components.Add(component);

        component.GameObject = this;
        component.IsRooted = IsRooted;
    }

    public void Add(params Component[] components)
    {
        foreach (Component component in components)
        {
            Add(component);
        }
    }

    public void Remove(Component component)
    {
        _components.Remove(component);
        component.GameObject = null;
    }

    public T Get<T>() where T : Component
    {
        // This can be optimized if need be
        foreach (Component component in _components)
        {
            if (component is T found)
            {
                return found;
            }
        }

        return null;
    }
}
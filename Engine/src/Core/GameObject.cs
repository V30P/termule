namespace Termule;

public sealed class GameObject : Component
{
    internal readonly Dictionary<string, Component> components = [];
    public Component this[string name] => components[name];

    public event Action<Component> ComponentAdded;
    public event Action<Component> ComponentRemoved;

    public GameObject()
    {
        Moved += MoveComponents;
        Updated += UpdateComponents;
        Destroyed += DestroyComponents;
    }

    void MoveComponents()
    {
        foreach (Component component in new Dictionary<string, Component>(components).Values)
        {
            component.path = $"{path}/{component.name}";
        }
    }

    void UpdateComponents()
    {
        foreach (Component component in new List<Component>(components.Values))
        {
            component.Update();
        }
    }

    void DestroyComponents()
    {
        foreach (Component component in new List<Component>(components.Values))
        {
            component.Destroy();
        }
    }

    internal void AddComponent(Component component)
    {
        components.Add(component.name, component);
        ComponentAdded?.Invoke(component);
    }

    internal void RemoveComponent(Component component)
    {
        components.Remove(component.name);
        ComponentRemoved?.Invoke(component);
    }

    public T GetComponent<T>() where T : Component
    {
        foreach (Component child in components.Values)
        {
            if (child is T found)
            {
                return found;
            }
        }

        return null;
    }

    //Finds the closest component of type T in the components of ancestors
    public T FindInAncestors<T>() where T : Component
    {
        GameObject ancestor = gameObject;
        while (ancestor != null)
        {
            foreach (Component component in ancestor.components.Values)
            {
                if (component is T found)
                {
                    return found;
                }
            }

            ancestor = ancestor.gameObject;
        }

        return null;
    }
}
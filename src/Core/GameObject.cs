using System.Collections;

namespace Termule.Core;

public class GameObject : Component, IEnumerable<Component>
{
    private readonly List<Component> _components = [];
    private readonly Dictionary<Type, List<Component>> _typesToComponents = [];

    public GameObject()
    {
        Registered += RegisterComponents;
        Ticked += TickComponents;
        Unregistered += UnregisterComponents;
    }

    private void RegisterComponents()
    {
        foreach (Component component in this.ToArray())
        {
            Game.Register(component);
        }
    }

    private protected void TickComponents()
    {
        foreach (IHostedComponent component in this.ToArray())
        {
            component.Tick();
        }
    }

    private void UnregisterComponents()
    {
        foreach (Component component in this.ToArray())
        {
            Game.Unregister(component);
        }
    }

    public void Add(Component component)
    {
        AddComponent(component);
        Game?.Register(component);
    }

    // Adds several components, then registers them simultaneously
    // This is useful when components depend on eachother at registration
    public void Add(params Component[] components)
    {
        foreach (Component component in components)
        {
            AddComponent(component);
        }

        foreach (Component component in components)
        {
            Game?.Register(component);
        }
    }

    private void AddComponent(Component component)
    {
        IHostedComponent hostedComponent = component;
        _components.Add(component);
        hostedComponent.GameObject = this;

        foreach (Type type in GetImplementedTypes(component))
        {
            if (!_typesToComponents.TryGetValue(type, out List<Component> componentList))
            {
                componentList = [];
                _typesToComponents.Add(type, componentList);
            }

            componentList.Add(component);
        }
    }

    public void Remove(Component component)
    {
        bool wasRemoved = _components.Remove(component);
        if (wasRemoved)
        {
            Game?.Unregister(component);
            ((IHostedComponent)component).GameObject = null;

            foreach (Type type in GetImplementedTypes(component))
            {
                List<Component> componentList = _typesToComponents[type];
                componentList.Remove(component);
            }
        }
    }

    private static List<Type> GetImplementedTypes(object o)
    {
        Type type = o.GetType();
        List<Type> implementedTypes = [type, .. o.GetType().GetInterfaces()];

        for (Type ancestor = type.BaseType; ancestor != null; ancestor = ancestor.BaseType)
        {
            implementedTypes.Add(ancestor);
        }

        return implementedTypes;
    }

    public IEnumerable<TComponent> GetAll<TComponent>() where TComponent : Component
    {
        return _typesToComponents.TryGetValue(typeof(TComponent), out List<Component> components) ?
            components.Cast<TComponent>() : [];
    }

    public TComponent Get<TComponent>() where TComponent : Component
    {
        return GetAll<TComponent>()?.FirstOrDefault();
    }

    public IEnumerator<Component> GetEnumerator()
    {
        return _components.Cast<Component>().OrderBy(c => c is GameObject).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
namespace Termule.Core;

using global::System.Collections;

public class GameObject : Component, IEnumerable<Component>
{
    private readonly List<Component> components = [];
    private readonly Dictionary<Type, List<Component>> typesToComponents = [];

    public GameObject()
    {
        this.Registered += this.RegisterComponents;
        this.Ticked += this.TickComponents;
        this.Unregistered += this.UnregisterComponents;
    }

    public void Add(Component component)
    {
        this.AddComponent(component);
        this.Game?.Register(component);
    }

    // Adds several components, then registers them simultaneously
    // This is useful when components depend on eachother at registration
    public void Add(params Component[] components)
    {
        foreach (Component component in components)
        {
            this.AddComponent(component);
        }

        foreach (Component component in components)
        {
            this.Game?.Register(component);
        }
    }

    public void Remove(Component component)
    {
        ArgumentNullException.ThrowIfNull(component);
        if (component.GameObject != this)
        {
            throw new InvalidOperationException($"Cannot remove Component '{component.GetType().Name}' since it is not part of this GameObject");
        }

        bool wasRemoved = this.components.Remove(component);
        if (wasRemoved)
        {
            this.Game?.Unregister(component);
            ((IHostedComponent)component).GameObject = null;

            foreach (Type type in GetImplementedTypes(component))
            {
                List<Component> componentList = this.typesToComponents[type];
                componentList.Remove(component);
            }
        }
    }

    public IEnumerable<TComponent> GetAll<TComponent>()
        where TComponent : Component
    {
        return this.typesToComponents.TryGetValue(typeof(TComponent), out List<Component> components) ?
            components.Cast<TComponent>() : [];
    }

    public TComponent Get<TComponent>()
        where TComponent : Component
    {
        return this.GetAll<TComponent>()?.FirstOrDefault();
    }

    public IEnumerator<Component> GetEnumerator()
    {
        return this.components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
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

    private void RegisterComponents()
    {
        foreach (Component component in this.ToArray())
        {
            this.Game.Register(component);
        }
    }

    private void TickComponents()
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
            this.Game.Unregister(component);
        }
    }

    private void AddComponent(Component component)
    {
        ArgumentNullException.ThrowIfNull(component);
        if (component.GameObject != null)
        {
            throw new InvalidOperationException($"Component '{component.GetType().Name}' is already part of a GameObject");
        }

        IHostedComponent hostedComponent = component;
        this.components.Add(component);
        hostedComponent.GameObject = this;

        foreach (Type type in GetImplementedTypes(component))
        {
            if (!this.typesToComponents.TryGetValue(type, out List<Component> componentList))
            {
                componentList = [];
                this.typesToComponents.Add(type, componentList);
            }

            componentList.Add(component);
        }
    }
}
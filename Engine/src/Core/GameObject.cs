namespace Termule.Core;

using global::System.Collections;

/// <summary>
/// A <see cref="Component"/> that groups other components.
/// </summary>
public class GameObject : Component, IEnumerable<Component>
{
    private readonly List<Component> components = [];
    private readonly Dictionary<Type, List<Component>> typesToComponents = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="GameObject"/> class.
    /// </summary>
    public GameObject()
    {
        this.Registered += this.RegisterComponents;
        this.Ticked += this.TickComponents;
        this.Unregistered += this.UnregisterComponents;
    }

    /// <summary>
    /// Adds the <see cref="Component"/> to this <see cref="GameObject"/>.
    /// </summary>
    /// <param name="component">The <see cref="Component"/> to add.</param>
    public void Add(Component component)
    {
        this.AddComponent(component);
        this.Game?.Register(component);
    }

    /// <summary>
    /// Adds several components at once.
    /// </summary>
    /// <param name="components">The components to add.</param>
    /// <remarks>
    /// This is useful when components depend on each other at registration.
    /// </remarks>
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

    /// <summary>
    /// Removes the <see cref="Component"/> from this <see cref="GameObject"/>.
    /// </summary>
    /// <param name="component">The <see cref="Component"/> to remove.</param>
    /// <exception cref="InvalidOperationException">Thrown if the provided component is not part of this GameObject.</exception>
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

    /// <summary>
    /// Gets all Components of type <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for. </typeparam>
    /// <returns> The collection of all matching components. </returns>
    public IEnumerable<TComponent> GetAll<TComponent>()
        where TComponent : Component
    {
        return this.typesToComponents.TryGetValue(typeof(TComponent), out List<Component> components) ?
            components.Cast<TComponent>() : [];
    }

    /// <summary>
    /// Gets a Component  of type <typeparamref name="TComponent"/>.
    /// </summary>
    /// <typeparam name="TComponent"> The type of component to look for. </typeparam>
    /// <returns> The component if one is found or null. </returns>
    public TComponent Get<TComponent>()
        where TComponent : Component
    {
        return this.GetAll<TComponent>()?.FirstOrDefault();
    }

    /// <inheritdoc/>
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
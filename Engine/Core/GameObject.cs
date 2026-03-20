using System.Collections;

namespace Termule.Core;

/// <summary>
///     Component that groups other components.
/// </summary>
public class GameObject : Component, IEnumerable<Component>
{
    private readonly List<Component> components = [];
    private readonly Dictionary<Type, List<Component>> typesToComponents = [];

    /// <summary>
    ///     Initializes a new instance of the <see cref="GameObject" /> class.
    /// </summary>
    public GameObject()
    {
        Registered += RegisterComponents;
        Ticked += TickComponents;
        Unregistered += UnregisterComponents;
    }

    /// <inheritdoc />
    public IEnumerator<Component> GetEnumerator()
    {
        return components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///     Adds the provided <paramref name="component" /> to this game object.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public void Add(Component component)
    {
        AddComponent(component);
        Game?.Register(component);
    }

    /// <summary>
    ///     Adds several components at once.
    /// </summary>
    /// <param name="componentsToAdd">The components to add.</param>
    /// <remarks>
    ///     This is useful when components depend on each other at registration.
    /// </remarks>
    public void Add(params Component[] componentsToAdd)
    {
        foreach (var component in componentsToAdd)
        {
            AddComponent(component);
        }

        foreach (var component in componentsToAdd)
        {
            Game?.Register(component);
        }
    }

    /// <summary>
    ///     Removes the provided <paramref name="component" /> from this game object.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the provided <paramref name="component" /> is not part of this game object.
    /// </exception>
    public void Remove(Component component)
    {
        ArgumentNullException.ThrowIfNull(component);
        if (component.GameObject != this)
        {
            throw new InvalidOperationException(
                $"Cannot remove Component '{component.GetType().Name}' since it is not part of this GameObject");
        }

        var wasRemoved = components.Remove(component);
        if (!wasRemoved)
        {
            return;
        }

        Game?.Unregister(component);
        component.SetGameObject(null);

        foreach (var componentList in GetImplementedTypes(component).Select(type => typesToComponents[type]))
        {
            componentList.Remove(component);
        }
    }

    /// <summary>
    ///     Gets a component of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for.</typeparam>
    /// <returns>The component if one is found or <c>null</c>.</returns>
    public TComponent Get<TComponent>()
    {
        return GetAll<TComponent>().FirstOrDefault();
    }

    /// <summary>
    ///     Gets all components of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for.</typeparam>
    /// <returns>The collection of all matching components.</returns>
    public IEnumerable<TComponent> GetAll<TComponent>()
    {
        return typesToComponents.TryGetValue(typeof(TComponent), out var matchingComponents)
            ? matchingComponents.Cast<TComponent>()
            : [];
    }

    private static List<Type> GetImplementedTypes(object o)
    {
        var type = o.GetType();
        List<Type> implementedTypes = [type, .. o.GetType().GetInterfaces()];

        for (var ancestor = type.BaseType; ancestor != null; ancestor = ancestor.BaseType)
        {
            implementedTypes.Add(ancestor);
        }

        return implementedTypes;
    }

    private void RegisterComponents()
    {
        foreach (var component in this.ToArray())
        {
            Game.Register(component);
        }
    }

    private void TickComponents()
    {
        foreach (var component in this.ToArray())
        {
            component.Tick();
        }
    }

    private void UnregisterComponents()
    {
        foreach (var component in this.ToArray())
        {
            Game.Unregister(component);
        }
    }

    private void AddComponent(Component component)
    {
        ArgumentNullException.ThrowIfNull(component);
        if (component.GameObject != null)
        {
            throw new InvalidOperationException(
                $"Component '{component.GetType().Name}' is already part of a GameObject");
        }

        components.Add(component);
        component.SetGameObject(this);

        foreach (var type in GetImplementedTypes(component))
        {
            if (!typesToComponents.TryGetValue(type, out var componentList))
            {
                componentList = [];
                typesToComponents.Add(type, componentList);
            }

            componentList.Add(component);
        }
    }
}
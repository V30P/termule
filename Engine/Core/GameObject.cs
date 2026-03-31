using System.Collections;

namespace Termule.Engine.Core;

/// <summary>
///     Component that groups other components.
/// </summary>
public class GameObject : Component, IEnumerable<Component>
{
    private readonly List<Component> components = [];
    private readonly Dictionary<Type, List<Component>> typesToComponents = [];
    private readonly List<Component> tickingComponents = [];

    private bool tickingDirty;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GameObject" /> class.
    /// </summary>
    public GameObject()
    {
        Registered += OnRegistered;
        Ticked += OnTick;
        Unregistered += OnUnregistered;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="GameObject" /> class with the provided components.
    /// </summary>
    /// <param name="components"> The components that the GameObject should contain. </param>
    public GameObject(params Component[] components) : this()
    {
        Add(components);
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
    ///     Adds the provided components to this game object.
    /// </summary>
    /// <param name="componentsToAdd">The components to add.</param>
    public void Add(params Component[] componentsToAdd)
    {
        foreach (Component component in componentsToAdd)
        {
            ArgumentNullException.ThrowIfNull(component);
            if (component.GameObject != null)
            {
                throw new ArgumentException(
                    $"Component '{component.GetType().Name}' is already part of a GameObject");
            }

            components.Add(component);
            tickingDirty = true;
            component.SetGameObject(this);

            foreach (Type type in GetImplementedTypes(component))
            {
                if (!typesToComponents.TryGetValue(type, out List<Component> componentList))
                {
                    componentList = [];
                    typesToComponents.Add(type, componentList);
                }

                componentList.Add(component);
            }
        }

        // Register all components simultaneously to handle dependencies
        foreach (Component component in componentsToAdd)
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

        components.Remove(component);
        component.SetGameObject(null);
        tickingDirty = true;

        Game?.Unregister(component);

        foreach (List<Component> componentList in
                 GetImplementedTypes(component).Select(type => typesToComponents[type]))
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
        if (typesToComponents.TryGetValue(typeof(TComponent), out List<Component> matchingComponents))
        {
            return (TComponent)(object)matchingComponents.FirstOrDefault();
        }

        return default;
    }

    /// <summary>
    ///     Gets all components of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for.</typeparam>
    /// <returns>The collection of all matching components.</returns>
    public IEnumerable<TComponent> GetAll<TComponent>()
    {
        return typesToComponents.TryGetValue(typeof(TComponent), out List<Component> matchingComponents)
            ? matchingComponents.Cast<TComponent>()
            : [];
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

    private void OnRegistered()
    {
        foreach (Component component in components)
        {
            Game.Register(component);
        }
    }

    private void OnTick()
    {
        // Rebuild the ticking list if necessary
        if (tickingDirty)
        {
            tickingComponents.Clear();
            foreach (Component component in components)
            {
                tickingComponents.Add(component);
            }

            tickingDirty = false;
        }

        foreach (Component component in tickingComponents)
        {
            // Handles the case where a component is removed during Tick
            if (component.GameObject != this)
            {
                continue;
            }

            component.Tick();
        }
    }

    private void OnUnregistered()
    {
        foreach (Component component in components)
        {
            Game.Unregister(component);
        }
    }
}
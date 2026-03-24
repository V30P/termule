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
        Registered += OnRegister;
        Ticked += OnTick;
        Unregistered += OnUnregister;
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
        foreach (var component in componentsToAdd)
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

        // Register all components simultaneously to handle dependencies
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

        components.Remove(component);
        component.SetGameObject(null);
        tickingDirty = true;

        Game?.Unregister(component);

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

    private void OnRegister()
    {
        foreach (var component in components)
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
            foreach (var component in components)
            {
                tickingComponents.Add(component);
            }

            tickingDirty = false;
        }

        foreach (var component in tickingComponents)
        {
            // Handles the case where a component is removed during Tick
            if (component.GameObject != this)
            {
                continue;
            }

            component.Tick();
        }
    }

    private void OnUnregister()
    {
        foreach (var component in components)
        {
            Game.Unregister(component);
        }
    }
}
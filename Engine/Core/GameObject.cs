using System.Collections;
using Termule.Engine.Core.Messaging;

namespace Termule.Engine.Core;

/// <summary>
///     A component that groups together child components.
/// </summary>
public sealed class GameObject : Component, IEnumerable<Component>
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
        Bus = new LocalMessageBus(this);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="GameObject" /> class with the provided
    ///     components.
    /// </summary>
    /// <param name="components">The components that the GameObject should contain.</param>
    public GameObject(params Component[] components) : this()
    {
        Add(components);
    }

    /// <summary>
    ///     Gets the local message bus.
    /// </summary>
    public LocalMessageBus Bus { get; }

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
    /// <param name="components">The components to add.</param>
    public void Add(params Component[] components)
    {
        foreach (Component component in components)
        {
            ArgumentNullException.ThrowIfNull(component);
            if (component.GameObject != null)
            {
                throw new ArgumentException(
                    $"Component '{component.GetType().Name}' is already part of a GameObject"
                );
            }

            this.components.Add(component);
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

        // Activate components simultaneously
        foreach (Component component in components)
        {
            Game?.Activate(component);
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
                $"Cannot remove Component '{component.GetType().Name}'"
                + "since it is not part of this GameObject"
            );
        }

        Game?.Deactivate(component);

        _ = components.Remove(component);
        component.SetGameObject(null);
        tickingDirty = true;

        IEnumerable<List<Component>> typedComponentLists = GetImplementedTypes(component)
            .Select(type => typesToComponents[type]);
        foreach (List<Component> componentList in typedComponentLists)
        {
            _ = componentList.Remove(component);
        }
    }

    /// <summary>
    ///     Gets a component of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for.</typeparam>
    /// <returns>The component if one is found or <see langword="null"/>.</returns>
    public TComponent Get<TComponent>()
    {
        bool componentExists = typesToComponents.TryGetValue(
            typeof(TComponent),
            out List<Component> matchingComponents
        );

        return componentExists ? (TComponent) (object) matchingComponents.FirstOrDefault() : default;
    }

    /// <summary>
    ///     Gets all components of type <typeparamref name="TComponent" />.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for.</typeparam>
    /// <returns>The collection of all matching components.</returns>
    public IEnumerable<TComponent> GetAll<TComponent>()
    {
        bool componentExists = typesToComponents.TryGetValue(
            typeof(TComponent),
            out List<Component> matchingComponents
        );

        return componentExists ? matchingComponents.Cast<TComponent>() : [];
    }

    /// <inheritdoc />
    protected internal override void Tick()
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

    /// <inheritdoc />
    protected override void Activate()
    {
        foreach (Component component in components.ToArray())
        {
            Game.Activate(component);
        }

        Game.Activate(Bus);
    }

    /// <inheritdoc />
    protected override void Deactivate()
    {
        foreach (Component component in components.ToArray())
        {
            Game.Deactivate(component);
        }

        Game.Deactivate(Bus);
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
}

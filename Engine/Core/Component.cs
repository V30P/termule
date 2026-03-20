using Termule.Exceptions;

namespace Termule.Core;

/// <summary>
///     Game element that can be added to <see cref="GameObject" />s to provide behavior every tick.
///     Components can be added, moved, and removed at runtime.
/// </summary>
public abstract class Component : GameElement
{
    /// <summary>
    ///     Invoked every time the game loop runs.
    /// </summary>
    protected event Action Ticked;

    /// <summary>
    ///     Gets the <see cref="GameObject" /> that this component is part of.
    /// </summary>
    public GameObject GameObject { get; private set; }

    /// <summary>
    ///     Removes this component from its GameObject.
    /// </summary>
    public void Destroy()
    {
        if (GameObject == null)
        {
            throw new InvalidOperationException("Cannot destroy a component with no GameObject.");
        }

        GameObject.Remove(this);
    }

    internal void SetGameObject(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    /// <summary>
    ///     Tries to get a component of type <typeparamref name="TComponent" /> from the containing game object.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for.</typeparam>
    /// <returns>The game object's instance of <typeparamref name="TComponent" />.</returns>
    /// <exception cref="MissingComponentException{TComponent}">Thrown if no matching component is found.</exception>
    protected TComponent GetRequiredComponent<TComponent>()
        where TComponent : Component
    {
        return GameObject.Get<TComponent>() is not { } component
            ? throw new MissingComponentException<TComponent>(this)
            : component;
    }

    internal void Tick()
    {
        Ticked?.Invoke();
    }
}
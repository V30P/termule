using Termule.Engine.Exceptions;

namespace Termule.Engine.Core;

/// <summary>
///     Game element that can be added to <see cref="GameObject" />s to provide local behavior or
///     data. Components can be added, moved, and removed at runtime.
/// </summary>
public abstract class Component : GameElement
{
    private GameObject gameObject;

    /// <summary>
    ///     Gets or sets the game object that this component is part of.
    /// </summary>
    public GameObject GameObject
    {
        get => gameObject;

        set
        {
            gameObject?.Remove(this);
            value?.Add(this);
        }
    }

    /// <summary>
    ///     Removes this component from its <see cref="GameObject"/> .
    /// </summary>
    public void Destroy()
    {
        GameObject?.Remove(this);
    }

    internal void SetGameObject(GameObject value)
    {
        gameObject = value;
    }

    /// <summary>
    ///     Called once per tick.
    /// </summary>
    protected internal virtual void Tick()
    {
    }

    /// <summary>
    ///     Tries to get a component of type <typeparamref name="TComponent" /> from the containing
    ///     game object, throwing if it cannot be found.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to look for.</typeparam>
    /// <returns>The game object's instance of <typeparamref name="TComponent" />.</returns>
    /// <exception cref="MissingComponentException{TComponent}">
    ///     Thrown if no matching component is found.
    /// </exception>
    protected TComponent GetRequiredComponent<TComponent>() where TComponent : Component
    {
        return GameObject.Get<TComponent>()
               ?? throw new MissingComponentException<TComponent>(this);
    }
}

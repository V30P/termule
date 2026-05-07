using Termule.Engine.Exceptions;

namespace Termule.Engine.Core;

/// <summary>
///     Game element that can be added to <see cref="GameObject" />s to provide behavior every tick.
///     Components can be added, moved, and removed at runtime.
/// </summary>
public abstract class Component : GameElement
{
    private GameObject gameObject;

    /// <summary>
    ///     Gets the <see cref="GameObject" /> that this component is part of.
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
    ///     Removes this component from its GameObject.
    /// </summary>
    public void Destroy()
    {
        GameObject?.Remove(this);
    }

    /// <summary>
    ///     Called once per frame.
    /// </summary>
    protected internal virtual void Tick()
    {
    }

    internal void SetGameObject(GameObject value)
    {
        gameObject = value;
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
        return GameObject.Get<TComponent>() ?? throw new MissingComponentException<TComponent>(this);
    }
}

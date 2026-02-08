namespace Termule.Core;

/// <summary>
/// A <see cref="GameElement"/> that can be added to <see cref="GameObject"/>s in order to provide behavior on tick every frame.
/// </summary>
public abstract class Component : GameElement, IHostedComponent
{
    /// <summary>
    /// Invoked once per frame.
    /// </summary>
    protected event Action Ticked;

    /// <summary>
    /// Gets the <see cref="GameObject"/> that this component is part of.
    /// </summary>
    public GameObject GameObject { get; private set; }

    GameObject IHostedComponent.GameObject { set => this.GameObject = value; }

    /// <summary>
    /// Removes this component from its GameObject.
    /// </summary>
    public void Destroy()
    {
        this.GameObject.Remove(this);
    }

    void IHostedComponent.Tick()
    {
        this.Tick();
    }

    /// <summary>
    /// Tries to get a component of type <typeparamref name="TComponent"/> from the containing GameObject.
    /// </summary>
    /// <typeparam name="TComponent"> The type of component to look for. </typeparam>
    /// <returns> The GameObject's instance of <typeparamref name="TComponent"/>. </returns>
    /// <exception cref="MissingComponentException{TComponent}"> Thrown if no matching component is found.</exception>
    protected TComponent GetRequiredComponent<TComponent>()
        where TComponent : Component
    {
        return this.GameObject.Get<TComponent>() is not TComponent component ?
            throw new MissingComponentException<TComponent>(this) : component;
    }

    private void Tick()
    {
        this.Ticked?.Invoke();
    }
}

internal interface IHostedComponent
{
    GameObject GameObject { set; }

    void Tick();
}
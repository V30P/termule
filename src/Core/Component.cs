namespace Termule.Core;

public abstract class Component : GameElement, IHostedComponent
{
    protected event Action Ticked;

    public GameObject GameObject { get; private set; }

    GameObject IHostedComponent.GameObject { set => this.GameObject = value; }

    public void Destroy()
    {
        this.GameObject.Remove(this);
    }

    void IHostedComponent.Tick()
    {
        this.Tick();
    }

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
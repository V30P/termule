namespace Termule.Core;

public abstract class Component : GameElement, IHostedComponent
{
    public GameObject GameObject { get; private set; }
    GameObject IHostedComponent.GameObject { set => GameObject = value; }

    protected event Action Ticked;

    private void Tick()
    {
        Ticked?.Invoke();
    }

    public void Destroy()
    {
        GameObject.Remove(this);
    }

    void IHostedComponent.Tick()
    {
        Tick();
    }

    protected TComponent GetRequiredComponent<TComponent>() where TComponent : Component
    {
        return GameObject.Get<TComponent>() is not TComponent component ?
            throw new MissingComponentException<TComponent>(this) : component;
    }
}

internal interface IHostedComponent
{
    GameObject GameObject { set; }

    void Tick();
}
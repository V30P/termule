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
}

internal interface IHostedComponent
{
    GameObject GameObject { set; }

    void Tick();
}
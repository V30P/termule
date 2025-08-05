namespace Termule;

public abstract class Component
{
    public readonly string name;
    internal protected Game game { get; private set; }
    public IComposite composite { get; internal set; }
    
    protected event Action Spawned;
    protected event Action Ticked;
    protected event Action Destroyed;

    public Component(string name = null)
    {
        this.name = name ?? GetType().Name;
    }

    internal void Spawn(Game game)
    {
        this.game = game;
        Spawned?.Invoke();
    }

    internal void Tick()
    {
        Ticked?.Invoke();
    }

    public void Destroy()
    {
        composite.Remove(this);
        Destroyed?.Invoke();
    }
}
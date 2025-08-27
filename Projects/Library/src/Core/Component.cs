namespace Termule;

public abstract class Component
{
    public GameObject gameObject { get; internal set; }

    bool spawned;

    protected event Action Rooted;
    protected event Action Ticked;
    protected event Action Destroyed;

    internal void Tick()
    {
        if (!spawned)
        {
            Rooted?.Invoke();
            spawned = true;
        }

        Ticked?.Invoke();
    }

    public void Destroy()
    {
        gameObject.Remove(this);
        Destroyed?.Invoke();
    }
}
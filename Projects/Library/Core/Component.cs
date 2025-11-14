namespace Termule;

public abstract class Component
{
    public GameObject GameObject { get; internal set; }

    private bool _hasBeenTicked;

    protected event Action Rooted;
    protected event Action Ticked;
    protected event Action Destroyed;

    internal void Tick()
    {
        if (!_hasBeenTicked)
        {
            Rooted?.Invoke();
            _hasBeenTicked = true;
        }

        Ticked?.Invoke();
    }

    public void Destroy()
    {
        GameObject.Remove(this);
        Destroyed?.Invoke();
    }
}
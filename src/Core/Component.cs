namespace Termule;

public abstract class Component
{
    public GameObject GameObject { get; internal set; }

    // Set this to true when a Component (or its GameObject) are added to the Game
    internal bool IsRooted
    {
        private protected get => _isRooted;

        set
        {
            if (value)
            {
                _isRooted = true;
                Rooted?.Invoke();
            }
        }
    }
    private bool _isRooted;

    protected event Action Rooted;
    protected event Action Ticked;
    protected event Action Destroyed;

    internal void Tick()
    {
        Ticked?.Invoke();
    }

    public void Destroy()
    {
        GameObject.Remove(this);
        Destroyed?.Invoke();
    }
}
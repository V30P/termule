namespace Termule;

public class GameObject : Component, IComposite
{
    Game IComposite.game => game;
    public Dictionary<string, Component> components => _components;
    readonly Dictionary<string, Component> _components = [];

    public GameObject(string name) : base(name)
    {
        Spawned += SpawnComponents;
        Ticked += UpdateComponents;
        Destroyed += DestroyComponents;
    }

    void SpawnComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.Spawn(game);
        }
    }

    void UpdateComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.Tick();
        }
    }

    void DestroyComponents()
    {
        foreach (Component component in this.ToArray())
        {
            component.Destroy();
        }
    }
}
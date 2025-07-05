namespace Termule;

public abstract class Component
{
    protected Game game;

    public string path
    {
        get => _path;

        set
        {
            //Determine the new parent
            GameObject newParent = game.root;
            if (value != null)
            {
                string[] splitPath = value.Split('/');
                for (int i = 0; i < splitPath.Length - 1; i++)
                {
                    if (newParent[splitPath[i]] is GameObject gameObject)
                    {
                        newParent = gameObject;
                    }
                    else
                    {
                        throw new Exception("Components must be part of a GameObject");
                    }
                }
            }

            //Apply the move
            _path = value;
            _name = path.Split('/')[^1];

            gameObject?.RemoveComponent(this);
            _gameObject = newParent;
            gameObject?.AddComponent(this); //Parent will only be null when the root is being spawned

            Moved?.Invoke();
        }
    }
    string _path;

    public string name { get => _name; set => path = $"{gameObject.path}/{value}"; }
    string _name;
    public GameObject gameObject { get => _gameObject; set => path = $"{(value != null ? $"{value.path}/" : null)}{name}"; }
    GameObject _gameObject;

    public event Action Spawned;
    public event Action Moved;
    public event Action Updated;
    public event Action Destroyed;

    bool isDestroyed = false;

    public static bool operator ==(Component c, object o) => ReferenceEquals(c, o) || (c.isDestroyed && o == null);
    public static bool operator !=(Component c, object o) => !(c == o);
    public override bool Equals(object o) => this == o;
    public override int GetHashCode() => base.GetHashCode();

    public static T Spawn<T>(Game game, string path) where T : Component, new()
    {
        T component = new T
        {
            game = game,
            path = path
        };

        component.Spawned?.Invoke();
        return component;
    }

    //Static spawn methods
    public static T Spawn<T>(GameObject gameObject, string name) where T : Component, new()
    => Spawn<T>(gameObject.game, gameObject != null ? $"{gameObject.path}/{name}" : name);
    public static T Spawn<T>(GameObject gameObject) where T : Component, new()
    => Spawn<T>(gameObject.game, gameObject != null ? $"{gameObject.path}/{typeof(T).Name}" : typeof(T).Name);

    //Instance spawn methods
    protected T Spawn<T>(string gameObjectPath, string name) where T : Component, new()
    => Spawn<T>(gameObject.game, gameObjectPath != null ? $"{gameObjectPath}/{name}" : name);

    internal void Update()
    {
        Updated?.Invoke();
    }

    public void Destroy()
    {
        if (this == game.root) throw new Exception("The root GameObject cannot be destroyed");

        isDestroyed = true;
        gameObject.RemoveComponent(this);
        Destroyed?.Invoke();
    }
}
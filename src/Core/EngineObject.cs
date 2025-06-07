namespace Termule.Internals;

public abstract class EngineObject
{
    public string path
    {
        get => _path;

        set
        {
            //Determine the new parent
            GameObject newParent = Game.root;
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
                        throw new Exception("EngineObjects must be part of a GameObject");
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

    public event Action Moved; 
    public event Action Updated;
    public event Action Destroyed;

    bool isDestroyed = false;

    public static bool operator ==(EngineObject c, object o) => ReferenceEquals(c, o) || (c.isDestroyed && o == null);
    public static bool operator !=(EngineObject c, object o) => !(c == o);
    public override bool Equals(object o) => this == o;
    public override int GetHashCode() => base.GetHashCode();

    public static T Spawn<T>(string path) where T : EngineObject, new()
    {
        T component = new T()
        {
            path = path
        };

        return component;
    }
    public static T Spawn<T>(string parentPath, string name) where T : EngineObject, new() => Spawn<T>($"{parentPath}/{name}");
    public static T Spawn<T>(GameObject parent, string name) where T : EngineObject, new() => Spawn<T>($"{parent.path}/{name}");
    public static T Spawn<T>(GameObject parent) where T : EngineObject, new() => Spawn<T>($"{parent.path}/{typeof(T).Name}");

    internal void Update()
    {
        Updated?.Invoke(); 
    }

    public void Destroy()
    {
        if (this == Game.root)
        {
            throw new Exception("The root GameObject cannot be destroyed");
        }

        isDestroyed = true;
        gameObject.RemoveComponent(this);
        Destroyed?.Invoke();
    }
}
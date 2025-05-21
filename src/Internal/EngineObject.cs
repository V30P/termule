namespace Termule.Internals;

public abstract class EngineObject
{
    public string path
    {
        get => _path;

        set
        {
            //Determine the new parent
            GameObject parent = Game.root;
            if (value != null)
            {
                string[] splitPath = value.Split('/');
                for (int i = 0; i < splitPath.Length - 1; i++)
                {
                    if (parent[splitPath[i]] is GameObject gameObject)
                    {
                        parent = gameObject;
                    }
                    else
                    {
                        throw new Exception("EngineObjects must be attached to a GameObject");
                    }
                }
            }

            //Apply the move
            _path = value;
            parentDetachAction?.Invoke();
            parent?.AddChild(this); //Parent will only be null when the root is being spawned
        }
    }
    string _path;

    internal Action parentDetachAction; //Provided by the parent GameObject

    public string name
    {
        get => path.Split('/')[^1];

        set
        {
            string[] splitPath = path.Split('/');

            string newPath = "";
            for (int i = 0; i < splitPath.Length - 1; i++)
            {
                newPath += $"{splitPath[i]}/";
            }
            newPath += value;

            path = newPath;
        }
    }

    bool isDestroyed = false;

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

    public static bool operator ==(EngineObject c, object o) => ReferenceEquals(c, o) || (c.isDestroyed && o == null);
    public static bool operator !=(EngineObject o1, object o2) => !(o1 == o2);
    public override bool Equals(object o) => o is EngineObject c && this == c;
    public override int GetHashCode() => base.GetHashCode();
    public override string ToString() => $"{GetType()} at {path}";

    internal void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate() { }

    public void Destroy()
    {
        isDestroyed = true;
        parentDetachAction.Invoke();

        OnDestroy();
    }

    public virtual void OnDestroy() { }
}
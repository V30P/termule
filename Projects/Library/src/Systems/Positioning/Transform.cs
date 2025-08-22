namespace Termule;

public class Transform : Component
{
    Transform parent;
    readonly List<Transform> children = [];

    public Transform()
    {
        Spawned += OnSpawned;
    }

    void OnSpawned()
    {
        parent = (composite as GameObject).composite?.Get<Transform>();

        foreach (Component component in composite)
        {
            if (component is GameObject componentGameObject
                && componentGameObject.Get<Transform>() is Transform childTransform)
            {
                children.Add(childTransform);
            }
        }
    }

    public Vector pos
    {
        get => _pos;

        set
        {
            Vector difference = value - pos;
            foreach (Transform child in children)
            {
                child.pos += difference;
            }

            _pos = value;
        }
    }
    Vector _pos;

    public Vector localPos { get => pos - parent.pos; set => pos = parent.pos + value; }
}
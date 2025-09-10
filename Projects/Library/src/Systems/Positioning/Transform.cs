namespace Termule;

public class Transform : Component
{
    Transform parent;
    readonly List<Transform> children = [];

    public Transform()
    {
        Rooted += OnSpawned;
    }

    void OnSpawned()
    {
        parent = gameObject.Get<Transform>();

        foreach (Component component in gameObject)
        {
            if (component is GameObject componentGameObject && componentGameObject.Get<Transform>() is Transform childTransform)
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
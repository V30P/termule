namespace Termule;

public class Transform : Component
{
    private Transform _parent;
    private readonly List<Transform> _children = [];

    public Transform()
    {
        Rooted += OnRooted;
    }

    private void OnRooted()
    {
        _parent = GameObject.Get<Transform>();

        foreach (Component component in GameObject)
        {
            if (component is GameObject componentGameObject && componentGameObject.Get<Transform>() is Transform childTransform)
            {
                _children.Add(childTransform);
            }
        }
    }

    public Vector Pos
    {
        get => _pos;

        set
        {
            Vector difference = value - Pos;
            foreach (Transform child in _children)
            {
                child.Pos += difference;
            }

            _pos = value;
        }
    }
    private Vector _pos;

    public Vector LocalPos { get => Pos - _parent.Pos; set => Pos = _parent.Pos + value; }
}
namespace Termule;

public class Transform : Behavior
{
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

    Transform parent;
    readonly List<Transform> children = [];

    public override void OnMove()
    {
        parent?.children.Remove(this);
        parent = gameObject.FindInAncestors<Transform>();
        parent?.children.Add(this);
    }
}
namespace Termule;

public class Transform : Component
{
    [RelativeComponent(Relation.Ancestor)]
    readonly Transform parent = null;
    readonly List<Transform> children = [];

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
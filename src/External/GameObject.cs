namespace Termule;

using Termule.Internals;

public class GameObject : EngineObject
{
    readonly Dictionary<string, EngineObject> children = [];
    public EngineObject this[string s] => children[s];

    public override void OnUpdate()
    {
        foreach (EngineObject child in children.Values)
        {
            child.Update();
        }
    }

    public override void OnDestroy()
    {
        foreach (EngineObject component in children.Values)
        {
            component.Destroy();
        }
    }

    internal void AddChild(EngineObject child)
    {
        children.Add(child.name, child);

        //Allow the child to detach itself
        string childName = child.name;
        child.parentDetachAction = () => children.Remove(childName);
    }
}
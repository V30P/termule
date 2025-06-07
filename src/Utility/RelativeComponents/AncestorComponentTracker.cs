namespace Termule.Internals;

internal class AncestorComponentTracker<T> : ComponentTracker<T> where T : EngineObject
{
    List<GameObject> additionTargetGameObjects = [];
    GameObject removalTargetGameObject;

    public AncestorComponentTracker(EngineObject attachee, string fieldName) : base(attachee, fieldName)
    {
        ChangeGameObject();
        attachee.Moved += ChangeGameObject;
    }

    void ChangeGameObject()
    {
        if (attached.gameObject != null)
        {
            component = attached.gameObject.FindInAncestors<T>();
            UpdateListeners();
        }
    }

    void UpdateListeners()
    {
        //Clear old listeners
        foreach (GameObject gameObject in additionTargetGameObjects)
        {
            gameObject.ComponentAdded -= InspectAncestorAddition;
        }

        if (removalTargetGameObject != null)
        {
            removalTargetGameObject.ComponentRemoved -= InspectAncestorRemoval;
        }

        additionTargetGameObjects = [];
        removalTargetGameObject = null;

        //Add addition listeners
        GameObject ancestor = attached.gameObject.gameObject;
        while (ancestor != null && (component == null || ancestor != component.gameObject))
        {
            ancestor.ComponentAdded += InspectAncestorAddition;
            additionTargetGameObjects.Add(ancestor);

            ancestor = ancestor.gameObject;
        }

        //If there is currently a found component, watch for its removal
        if (component != null)
        {
            ancestor.ComponentRemoved += InspectAncestorRemoval;
            removalTargetGameObject = ancestor;
        }
    }

    void InspectAncestorAddition(EngineObject addedComponent)
    {
        if (addedComponent is T found)
        {
            component = found;
            UpdateListeners();
        }
    }

    void InspectAncestorRemoval(EngineObject removedComponent)
    {
        if (removedComponent == component)
        {
            component = attached.gameObject.FindInAncestors<T>();
            UpdateListeners();
        }
    }
}
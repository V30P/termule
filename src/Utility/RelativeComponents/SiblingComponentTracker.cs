namespace Termule.Internals;

internal class SiblingComponentTracker<T> : ComponentTracker<T> where T : EngineObject
{
    GameObject targetGameObject;

    public SiblingComponentTracker(EngineObject attachee, string fieldName) : base(attachee, fieldName)
    {
        ChangeGameObject();
        attachee.Moved += ChangeGameObject;
    }

    void ChangeGameObject()
    {
        //Clear events from the old target  
        if (targetGameObject != null)
        {
            targetGameObject.ComponentAdded -= InspectSiblingAddition;
            targetGameObject.ComponentRemoved -= InspectSiblingRemoval;
        }

        //Look for the sibling in the new target
        targetGameObject = attached.gameObject;
        if (targetGameObject != null)
        {
            component = targetGameObject.GetComponent<T>();

            if (component == null)
            {
                targetGameObject.ComponentAdded += InspectSiblingAddition;
            }
            else
            {
                targetGameObject.ComponentRemoved += InspectSiblingRemoval;
            }
        }
    }

    void InspectSiblingAddition(EngineObject addedComponent)
    {
        if (addedComponent is T found)
        {
            component = found;

            targetGameObject.ComponentAdded -= InspectSiblingAddition;
            targetGameObject.ComponentRemoved += InspectSiblingRemoval;
        }
    }

    void InspectSiblingRemoval(EngineObject removedComponent)
    {
        if (removedComponent == component)
        {
            component = targetGameObject.GetComponent<T>();

            if (component == null)
            {
                targetGameObject.ComponentRemoved -= InspectSiblingRemoval;
                targetGameObject.ComponentAdded += InspectSiblingAddition;
            }
        }
    }
}
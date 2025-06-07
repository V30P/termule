namespace Termule.Internals;

using System.Reflection;

internal abstract class ComponentTracker<T> where T : EngineObject
{
    const BindingFlags fieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    protected readonly EngineObject attached;
    protected readonly FieldInfo field;

    protected T component
    {
        get => _component;

        set
        {
            _component = value;
            field.SetValue(attached, component, fieldFlags, null, null);
        }
    }
    T _component;

    protected ComponentTracker(EngineObject attachee, string fieldName)
    {
        attached = attachee;
        field = attached.GetType().GetField(fieldName, fieldFlags);
    }
}
using System.Reflection;

namespace Termule;

internal abstract class ComponentTracker<T> where T : Component
{
    const BindingFlags fieldFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    protected readonly Component attached;
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

    protected ComponentTracker(Component attachee, string fieldName)
    {
        attached = attachee;
        field = attached.GetType().GetField(fieldName, fieldFlags);
    }
}
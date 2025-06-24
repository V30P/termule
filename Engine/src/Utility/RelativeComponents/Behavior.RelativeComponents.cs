using System.Reflection;

namespace Termule;

public partial class Behavior
{
    [AttributeUsage(AttributeTargets.Field)]
    protected class RelativeComponent(Relation relation) : Attribute
    {
        internal readonly Relation relation = relation;
    }

    static readonly Dictionary<Relation, Type> componentTrackerTypes = new Dictionary<Relation, Type>
    {
        { Relation.Sibling, typeof(SiblingComponentTracker<>) },
        { Relation.Ancestor, typeof(AncestorComponentTracker<>) }
    };

    void SetUpRelativeComponents()
    {
        IEnumerable<FieldInfo> rcFields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .Where(field => Attribute.IsDefined(field, typeof(RelativeComponent)));
        foreach (FieldInfo rcField in rcFields)
        {
            Type trackerType = componentTrackerTypes[rcField.GetCustomAttribute<RelativeComponent>().relation];
            Type genericTrackerType = trackerType.MakeGenericType(rcField.FieldType);

            Activator.CreateInstance(genericTrackerType, this, rcField.Name);
        }
    }
}
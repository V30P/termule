namespace Termule.Engine.Core;

internal static class Utilities
{
    internal static List<Type> GetTypeHierarchy(object o)
    {
        Type type = o.GetType();
        List<Type> implementedTypes = [type, .. o.GetType().GetInterfaces()];

        for (Type ancestor = type.BaseType; ancestor != null; ancestor = ancestor.BaseType)
        {
            implementedTypes.Add(ancestor);
        }

        return implementedTypes;
    }
}

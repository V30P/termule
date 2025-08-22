using System.Collections;

namespace Termule;

public interface IComposite : IEnumerable<Component>
{
    internal Game game { get; }
    internal Dictionary<string, Component> components { get; }

    IEnumerator<Component> IEnumerable<Component>.GetEnumerator()
    {
        lock (components) return components.Values.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class ICompositeExtensions
{
    public static void Add(this IComposite composite, Component component)
    {
        lock (composite.components)
        {
            composite.components.Add(component.name, component);
            component.composite = composite;

            if (component.game == null && composite.game != null)
            {
                component.Spawn(composite.game);
            }
        }
    }

    public static void Add(this IComposite composite, params Component[] components)
    {
        lock (composite.components)
        {
            foreach (Component component in components)
            {
                composite.Add(component);
            }
        }
    }

    public static void Remove(this IComposite composite, Component component)
    {
        lock (composite.components) composite.components.Remove(component.name);
        component.composite = null;
    }

    public static Component At(this IComposite composite, string path)
    {
        string[] splitPath = path.Split('\\');
        string nextName = splitPath[0];

        if (composite.components.TryGetValue(nextName, out Component nextComponent))
        {
            if (splitPath.Length == 1)
            {
                return nextComponent;
            }
            else
            {
                string remainingPath = path[(nextName.Length + 1)..];

                if (nextComponent is IComposite nextComposite)
                {
                    return nextComposite.At(remainingPath);
                }
            }
        }

        return null;
    }

    public static T Get<T>(this IComposite composite) where T : Component
    {
        if (composite.At(typeof(T).Name) is T foundByName)
        {
            return foundByName;
        }

        foreach (Component component in composite.components.Values)
        {
            if (component is T foundByForce)
            {
                return foundByForce;
            }
        }

        return null;
    }
}
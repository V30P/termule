using System.Reflection;

namespace Termule.Editor;

internal static class Factory
{
    static readonly Func<Type, bool> typeIsParentProducibleType =
    (Type t) => t.BaseType?.BaseType == typeof(NongenericProducible);
    static readonly Func<Type, bool> typeIsProducibleSubtype =
    (Type t) => producibles.Keys.Where(producibleType => t != producibleType && t.IsAssignableTo(producibleType)).Any();
    const BindingFlags infoPropertyFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    static readonly Dictionary<Type, Dictionary<string, Type>> producibles = [];

    static Factory()
    {
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();

        //Store the types that directly inherit from Producible
        foreach (Type parentProducibleType in types.Where(typeIsParentProducibleType))
        {
            producibles.Add(parentProducibleType, []);
        }

        //Store the subtypes that inherit from Producibles under their corresponding Producible
        foreach (Type producibleSubtype in types.Where(typeIsProducibleSubtype))
        {
            PropertyInfo infoProperty = producibleSubtype.GetProperty("info", infoPropertyFlags);
            ProducibleInfo info = (ProducibleInfo)infoProperty.GetValue(Activator.CreateInstance(producibleSubtype));

            producibles[producibleSubtype.BaseType].Add(info.name, producibleSubtype);
        }
    }

    internal static T Make<T>(string input) where T : NongenericProducible
    {
        input = input.Trim();
        string name = input.Split(' ')[0].ToLower();

        //Try to get the dictionary of producibles for the provided type
        if (producibles.TryGetValue(typeof(T), out Dictionary<string, Type> ProducibleSubtypes))
        {
            //Try to get the specific producible that matches the input name
            if (ProducibleSubtypes.TryGetValue(name, out Type producibleSubtype))
            {
                T product = (T) Activator.CreateInstance(producibleSubtype);
                product.args = ParseArgs(input[name.Length..]);

                return product;
            }
        }

        Console.WriteLine($"No {typeof(T).Name} \"{name}\" found");
        return null;
    }

    static string[] ParseArgs(string args)
    {
        List<string> parsedArgs = [];
        bool inArgGroup = false;
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case ' ':
                    if (!inArgGroup)
                    {
                        parsedArgs.Add(null);
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                case '(':
                    if (!inArgGroup)
                    {
                        inArgGroup = true;
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                case ')':
                    if (inArgGroup)
                    {
                        inArgGroup = false;
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                default:
                    parsedArgs[^1] += args[i];
                    break;
            }
        }

        return [.. parsedArgs];
    }
}
using System.Reflection;

namespace Termule.Saddlebag;

internal static class ExecutorFactory
{
    static readonly Dictionary<string, Type> commandToExecutor = [];

    static ExecutorFactory()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.GetCustomAttribute<ExecutorAttribute>() is ExecutorAttribute executorAttribute)
            {
                commandToExecutor.Add(executorAttribute.command, type);
            }
        }
    }

    internal static object MakeExecutor(string[] command)
    {
        string possibleCommandName = null;
        for (int i = 0; i < command.Length; i++)
        {
            possibleCommandName += (possibleCommandName == null ? "" : " ") + command[i];
            
            if (commandToExecutor.TryGetValue(possibleCommandName, out Type executorType))
            {
                object executor = TryConstructExecutor(executorType, command[(i + 1)..]);
                if (executor != null) return executor;
            }
        }

        return null;
    }

    static object TryConstructExecutor(Type executorType, string[] args)
    {
        foreach (ConstructorInfo constructor in executorType.GetConstructors())
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.Length == args.Length)
            {
                return Activator.CreateInstance(executorType, args.Length == 0 ? Array.Empty<object>() : args);
            }
            else if (parameters.Length > 0 && parameters[^1].ParameterType == typeof(string[]))
            {
                object[] constructorArgs = [.. args[..(parameters.Length - 1)], args[(parameters.Length - 1)..]];
                return Activator.CreateInstance(executorType, constructorArgs);
            }
        }

        return null;
    }
}
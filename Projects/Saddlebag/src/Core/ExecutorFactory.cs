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

    internal static object MakeExecutor(string command)
    {
        if (commandToExecutor.TryGetValue(command, out Type executorType))
        {
            return Activator.CreateInstance(executorType, true);
        }

        return null;
    }
}
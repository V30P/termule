using System.Reflection;

namespace Termule.Editor;

internal interface ICommandExecutor
{
    static readonly Func<Type, bool> typeImplementsExecutor =
    (Type t) => typeof(ICommandExecutor).IsAssignableFrom(t) && t.IsClass;

    private static readonly Dictionary<string, Type> nameToExecutorType = [];
     static Dictionary<Type, CommandInfo> infoList = [];

    protected internal static CommandInfo commandInfo { get; }

    static ICommandExecutor()
    {
        Assembly assembly = typeof(ICommandExecutor).Assembly;
        foreach (Type executorType in assembly.GetTypes().Where(typeImplementsExecutor))
        {
            string name = executorType.Name.ToLower();
            nameToExecutorType.Add(name, executorType);

            //Get the partial CommandInfo from the executor
            PropertyInfo commandInfoProperty = executorType.GetProperty("commandInfo", BindingFlags.Public | BindingFlags.Static);
            CommandInfo info = (CommandInfo) commandInfoProperty.GetValue(null);

            //Complete and store the CommandInfo
            info.name = name;
            infoList.Add(executorType, info);
        }
    }

    internal static ICommandExecutor GetExecutor(string command)
    {
        if (nameToExecutorType.TryGetValue(command.ToLower(), out Type executorType))
        {
            return (ICommandExecutor) Activator.CreateInstance(executorType);
        }

        Console.WriteLine($"No command \"{command}\" found");
        return null;
    }

    internal sealed bool ValidateContext(string[] args, Pen pen)
    {
        CommandInfo info = infoList[GetType()];
        if (pen == null && !info.avaliableOutsidePen)
        {
            Console.WriteLine($"Cannot run command \"{info.name}\" outside of a pen");
        }
        else if (pen != null && !info.avaliableInsidePen)
        {
            Console.WriteLine($"Cannot run command \"{info.name}\" inside of a pen");
        }
        else
        {
            return true;
        }

        return false;
    }

    internal sealed void ValidateAndExecute(string[] args, Pen pen)
    {
        if (ValidateContext(args, pen))
        {
            Execute(args, pen);
        }
    }

    internal void Execute(string[] args, Pen pen);
}

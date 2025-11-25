using System.Reflection;

namespace Termule;

internal static class Bootstrapper
{
    public static void Main(string[] args)
    {
        string localGameDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Game");
        MethodInfo entry = Path.Exists(localGameDir) ? FindEntry(localGameDir) : FindEntry(args[0]);

        try
        {
            entry.Invoke(null, null);

            AppDomain.CurrentDomain.ProcessExit += (_, _) => Game.Stop();
            Console.CancelKeyPress += (_, _) => Game.Stop();

            Game.Run();
        }
        catch
        {
            Game.Stop();
            throw;
        }
    }

    private static MethodInfo FindEntry(string directory)
    {
        foreach (string path in Directory.GetFiles(directory, "*.dll"))
        {
            Assembly assembly = Assembly.LoadFrom(path);
            MethodInfo entryMethod = assembly
                .GetTypes()
                .SelectMany(type => type.GetMethods())
                .FirstOrDefault(method => method.GetCustomAttribute<EntryAttribute>() != null);

            if (entryMethod != null)
            {
                return entryMethod;
            }
        }

        return null;
    }
}
using System.Reflection;

namespace Termule;

internal static class Program
{
    internal static string GameDir { get; private set; }

    public static void Main(string[] args)
    {
        string localGameDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Game");
        if (Path.Exists(localGameDir))
        {
            GameDir = localGameDir;
        }
        else
        {
            GameDir = args[0];
            args = args[1..];
        }

        MethodInfo init = FindInitMethod(GameDir);

        try
        {
            init.Invoke(null, init.GetParameters().Length == 0 ? null : [args]);

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

    private static MethodInfo FindInitMethod(string gameDir)
    {
        foreach (string path in Directory.GetFiles(gameDir, "*.dll"))
        {
            Assembly assembly = Assembly.LoadFrom(path);
            MethodInfo entryMethod = assembly
                .GetTypes()
                .SelectMany(type => type.GetMethods())
                .FirstOrDefault(method => method.GetCustomAttribute<InitAttribute>() != null);

            if (entryMethod != null)
            {
                return entryMethod;
            }
        }

        return null;
    }
}
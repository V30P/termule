using System.Reflection;

namespace Termule;

internal static class Program
{
    private static readonly string _localGameDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Game");

    public static void Main(string[] args)
    {
        MethodInfo init;
        if (Path.Exists(_localGameDir))
        {
            init = FindInitMethod(_localGameDir);
        }
        else
        {
            init = FindInitMethod(args[0]);
            args = args[1..];
        }

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
using System.Text.Json;
using Microsoft.Build.Locator;

namespace Termule.Saddlebag;

static class Saddlebag
{
    static void Main(string[] args)
    {
        if (args.Length == 0) return;

        MSBuildLocator.RegisterDefaults();
        Environment.Initialize();

        if (ExecutorFactory.MakeExecutor(args[0]) == null)
        {
            Console.WriteLine($"No executor for command \"{args[0]}\" found");
        }

        Environment.config.Save();
    }
}
using System.Text.Json;
using Microsoft.Build.Locator;

namespace Termule.Saddlebag;

static class Saddlebag
{
    static void Main(string[] args)
    {
        MSBuildLocator.RegisterDefaults();
        ProjectManager.Initialize();

        try
        {
            if (ExecutorFactory.MakeExecutor(args) == null)
            {
                Console.WriteLine($"No command found");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
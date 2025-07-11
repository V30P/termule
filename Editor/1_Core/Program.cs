using Microsoft.Build.Locator;
using Microsoft.Build.Evaluation;

namespace Termule.Editor;

static class Editor
{
    internal static Project project { get; private set; }
    internal static string projectName { get; private set; }
    internal static string projectAssemblyPath { get; private set; }

    static void Main(string[] args)
    {
        MSBuildLocator.RegisterDefaults();

        //Calls to MSBuild must not be in the same method as calls to the MSBuildLocator
        //See: https://learn.microsoft.com/en-us/visualstudio/msbuild/find-and-use-msbuild-versions?view=vs-2022
        if (!TryResolveProject())
        {
            Console.WriteLine("The current directory is not in a Termule project");
            return;
        }

        //Recombine the input so the factory can parse it properly
        string input = null;
        foreach (string arg in args)
        {
            input += arg + " ";
        }

        Factory.Make<CommandExecutor>(input)?.StartExecute(null);
    }

    static bool TryResolveProject()
    {
        DirectoryInfo searchDirectory = new DirectoryInfo(Environment.CurrentDirectory);
        string projectFilePath = null;
        while (searchDirectory != null)
        {
            if (searchDirectory.GetFiles().Where(x => x.Extension == ".csproj").FirstOrDefault() is FileInfo projectFile)
            {
                projectFilePath = projectFile.FullName;
                break;
            }

            searchDirectory = searchDirectory.Parent;
        }

        if (projectFilePath == null)
        {
            return false;
        }

        project = new Project(projectFilePath);
        projectName = project.GetPropertyValue("AssemblyName");
        projectAssemblyPath = project.GetPropertyValue("TargetPath");

        return true;
    }
}

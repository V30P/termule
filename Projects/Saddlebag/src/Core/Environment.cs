using System.Text.Json;
using Microsoft.Build.Evaluation;
using System.Reflection;

namespace Termule.Saddlebag;

internal class ProjectManager
{
    internal static Project project { get; private set; }

    internal static void Initialize()
    {
        DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
        if (currentDirectory.GetFiles().FirstOrDefault(file => file.Extension == ".csproj")?.FullName is string projectFileFullName)
        {
            project = new Project(projectFileFullName);
        }
    }
}
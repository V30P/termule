using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;

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

    internal static ProjectInstance GetProjectInstance() => new ProjectInstance(project.FullPath);
}
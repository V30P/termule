using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;

namespace Termule.Saddlebag;

[Executor("build")]
public class BuildExecutor
{
    internal string outputPath = ProjectManager.project.GetPropertyValue("TargetPath");
    internal bool succeeded;

    public BuildExecutor()
    {
        succeeded = TryBuild();
        Console.WriteLine(succeeded ? $"Build succeeded -> {outputPath}" : "Build failed");
    }

    static bool TryBuild()
    {
        ProjectCollection projectCollection = new ProjectCollection();
        ProjectInstance project = new ProjectInstance(ProjectManager.project.FullPath, null, null, projectCollection);
        BuildParameters buildParameters = new BuildParameters(projectCollection);

        BuildRequestData restoreRequest = new BuildRequestData(project, ["Restore"]);
        if (BuildManager.DefaultBuildManager.Build(buildParameters, restoreRequest).OverallResult == BuildResultCode.Failure) return false;

        BuildRequestData buildRequest = new BuildRequestData(project, ["Build"]);
        return BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest).OverallResult == BuildResultCode.Success;
    }
}
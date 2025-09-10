using Microsoft.Build.Execution;

namespace Termule.Saddlebag;

[Executor("build")]
internal class BuildExecutor
{
    internal readonly string outputPath;
    internal readonly bool succeeded;

    public BuildExecutor()
    {
        succeeded = TryBuild(out outputPath);
        Console.WriteLine(succeeded ? $"Build succeeded -> {outputPath}" : "Build failed");
    }

    static bool TryBuild(out string outputPath)
    {
        outputPath = Paths.ConvertToUnixPath(ProjectManager.project.GetPropertyValue("TargetPath"));

        ProjectInstance projectInstance = ProjectManager.GetProjectInstance();
        BuildParameters buildParameters = new BuildParameters();

        BuildRequestData restoreRequest = new BuildRequestData(projectInstance, ["Restore"]);
        if (BuildManager.DefaultBuildManager.Build(buildParameters, restoreRequest).OverallResult == BuildResultCode.Failure) return false;

        BuildRequestData buildRequest = new BuildRequestData(projectInstance, ["Build"]);
        return BuildManager.DefaultBuildManager.Build(buildParameters, buildRequest).OverallResult == BuildResultCode.Success;
    }
}
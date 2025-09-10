using Microsoft.Build.Execution;

namespace Termule.Saddlebag;

[Executor("publish")]
internal class PublishExecutor
{
    internal string publishPath;
    internal bool succeeded => publishPath != null;

    public PublishExecutor()
    {
        bool succeeded = TryPublish(out publishPath);
        Console.WriteLine(succeeded ? $"Publish succeeded -> {publishPath}" : "Publish failed");
    }

    static bool TryPublish(out string publishPath)
    {
        ProjectInstance projectInstance = ProjectManager.GetProjectInstance();
        BuildRequestData evaluateRequest = new BuildRequestData(projectInstance, ["EvaluatePublishFiles"]);
        BuildManager.DefaultBuildManager.Build(new BuildParameters(), evaluateRequest);

        string publishDirectory = $"{ProjectManager.project.DirectoryPath}/{projectInstance.GetPropertyValue("PublishDir")}";
        publishPath = Paths.ConvertToUnixPath($"{publishDirectory}{Path.GetFileNameWithoutExtension(ProjectManager.project.FullPath)}.exe");

        BuildExecutor buildExecutor = new BuildExecutor();
        if (!buildExecutor.succeeded) return false;

        Directory.CreateDirectory(publishDirectory);
        using FileStream outputFileStream = File.Create(publishPath);

        // Duplicate the bootstrapper
        outputFileStream.Write(File.ReadAllBytes(Paths.publishBootstrapperPath));

        // Add the built project dll
        byte[] projectBytes = File.ReadAllBytes(buildExecutor.outputPath);
        outputFileStream.Write(projectBytes);

        // Add the length of the project so it can be loaded by the bootloader
        outputFileStream.Write(BitConverter.GetBytes(projectBytes.Length));

        return true;
    }
}
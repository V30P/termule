namespace Termule.Saddlebag;

[Executor("build")]
internal class BuildExecutor
{
    internal readonly bool succeeded;
    internal readonly string outputPath = $"{Environment.currentPath}\\{Path.GetFileNameWithoutExtension(Environment.project.FullPath)}.exe";

    internal BuildExecutor()
    {
        succeeded = Builder.TryBuild(Environment.project, outputPath);
        Console.WriteLine(succeeded ? $"Build succeeded -> {outputPath}" : "Build failed");
    }
}
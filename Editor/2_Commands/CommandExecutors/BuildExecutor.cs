namespace Termule.Editor;

internal class BuildExecutor : CommandExecutor
{
    internal override CommandExecutorInfo info => new CommandExecutorInfo
    {
        name = "build",
        avaliableOutsidePen = true
    };

    protected override void Execute()
    {
        Console.WriteLine
        (
            Editor.project.Build() ?
            $"Build successful, path: {Editor.projectAssemblyPath}" : "Build failed"
        );
    }
}
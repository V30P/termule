namespace Termule.Editor;

internal class Build : ICommandExecutor
{
    public static CommandInfo commandInfo => new CommandInfo
    {
        avaliableOutsidePen = true,
    };

    public void Execute(string[] _, Pen __)
    {
        Console.WriteLine
        (
            Editor.project.Build() ?
            $"Build successful, path: {Editor.projectAssemblyPath}" : "Build failed"
        );
    }
}
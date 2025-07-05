namespace Termule.Editor;

internal class WindowExecutor : CommandExecutor
{
    internal override CommandExecutorInfo info => new CommandExecutorInfo
    {
        name = "window",
        avaliableInsidePen = true
    };

    protected override void Execute() => pen.windows.TryAdd(args[0], new Window(Window.ReadMode.Standard));
}
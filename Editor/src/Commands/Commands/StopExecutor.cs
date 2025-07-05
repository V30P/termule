namespace Termule.Editor;

internal class StopExecutor : CommandExecutor
{
    internal override CommandExecutorInfo info => new CommandExecutorInfo
    {
        name = "stop",
        avaliableInsidePen = true
    };

    protected override void Execute() => pen.game.Stop();
}
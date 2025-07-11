namespace Termule.Editor;

internal class EchoExecutor : CommandExecutor
{
    internal override CommandExecutorInfo info => new CommandExecutorInfo
    {
        name = "echo",
        avaliableOutsidePen = true,
        avaliableInsidePen = true
    };

    protected override void Execute() => Console.WriteLine(args[0]);
}
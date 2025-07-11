namespace Termule.Editor;

internal class DebugExecutor : CommandExecutor
{
    internal override CommandExecutorInfo info => new CommandExecutorInfo
    {
        name = "debug",
        avaliableInsidePen = true
    };

    protected override void Execute()
    {
        if (Factory.Make<Debugger>(args[0]) is Debugger debugger)
        {
            Window window = pen.windows[args[1]];
            window.Closed += debugger.Stop;
            
            debugger.StartDebugging(pen.game, pen.windows[args[1]].writer);
        }
    }
}
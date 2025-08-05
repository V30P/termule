using System.Collections.Concurrent;

namespace Termule.Editor;

internal class LogDebugger : Debugger
{
    BlockingCollection<string> logDistributor;

    internal override DebuggerInfo info => new DebuggerInfo
    {
        name = "log"
    };

    internal override void Start()
    {
        logDistributor = game.Get<Logger>().GetDistributor();
        Task.Run(DebugLogs);
    }

    void DebugLogs()
    {
        while (true)
        {
            try
            {
                output.Write(logDistributor.Take());
            }
            catch (InvalidOperationException) // Occurs if the logDistributor IsAddingCompleted
            {
                return;
            }   
        }
    }

    internal override void Stop() => logDistributor.CompleteAdding();
}
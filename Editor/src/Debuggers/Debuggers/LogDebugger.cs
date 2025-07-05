namespace Termule.Editor;

internal class LogDebugger : Debugger, IDisposable
{
    StreamReader logReader;
    bool stop;

    internal override DebuggerInfo info => new DebuggerInfo
    {
        name = "log"
    };

    internal override void Debug()
    {
        logReader = new StreamReader(game.logger.GetStream());
        Task.Run(DebugLog);
    }

    void DebugLog()
    {
        while (!stop)
        {
            int read = logReader.Read();
            if (read != -1)
            {
                writer.Write((char) read);
            }
        }
    }

    internal override void Stop()
    {
        stop = true;
        Dispose();
    }

    public void Dispose() => logReader.Dispose();
}
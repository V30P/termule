namespace Termule.Editor;

internal abstract class Debugger : Producible<DebuggerInfo>
{
    protected Game game;
    protected StreamWriter writer;

    internal void StartDebug(Game game, StreamWriter writer)
    {
        this.game = game;
        this.writer = writer;

        Debug();
    }

    internal abstract void Debug();

    internal virtual void Stop() { }
}
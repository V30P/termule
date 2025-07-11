namespace Termule.Editor;

internal abstract class Debugger : Producible<DebuggerInfo>
{
    protected Game game;
    protected StreamWriter output;

    internal void StartDebugging(Game target, StreamWriter output)
    {
        game = target;
        this.output = output;

        Start();
    }

    internal virtual void Start() { }

    internal virtual void Stop() { }
}
namespace Tests.Utilities;

internal class FakeSystem : Termule.Core.System
{
    internal bool Started { get; private set; }

    internal int TickCount { get; private set; }

    internal bool Stopped { get; private set; }

    protected internal override void Start()
    {
        Started = true;
    }

    protected internal override void Tick()
    {
        TickCount++;
    }

    protected internal override void Stop()
    {
        Stopped = true;
    }
}
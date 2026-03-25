namespace Termule.Tests.Utilities;

public class FakeSystem : Engine.Core.System
{
    public bool Started { get; private set; }

    public int TickCount { get; private set; }

    public bool Stopped { get; private set; }

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
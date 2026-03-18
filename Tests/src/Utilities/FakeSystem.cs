namespace Termule.Tests.Utilities;

using Termule.Core;

internal class FakeSystem : System
{
    internal bool Started { get; private set; }

    internal int TickCount { get; private set; }

    internal bool Stopped { get; private set; }

    protected override void Start()
    {
        this.Started = true;
    }

    protected override void Tick()
    {
        this.TickCount++;
    }

    protected override void Stop()
    {
        this.Stopped = true;
    }
}
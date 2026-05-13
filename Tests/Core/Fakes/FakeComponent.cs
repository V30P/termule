using Termule.Engine.Core;

namespace Termule.Tests.Core;

public class FakeComponent : Component
{
    public int TickCount { get; private set; }

    public int ActivateCount { get; private set; }

    public int DeactivateCount { get; private set; }

    public TComponent CallGetRequiredComponent<TComponent>() where TComponent : Component
    {
        return GetRequiredComponent<TComponent>();
    }

    protected internal override void Tick()
    {
        TickCount++;
    }

    protected override void Activate()
    {
        ActivateCount++;
    }

    protected override void Deactivate()
    {
        DeactivateCount++;
    }
}

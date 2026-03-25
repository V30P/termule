using Termule.Engine.Core;

namespace Termule.Tests.Utilities;

public class FakeComponent : Component
{
    public int TickCount { get; private set; }

    public int RegisterCount { get; private set; }

    public int UnregisterCount { get; private set; }

    public FakeComponent()
    {
        Registered += () => RegisterCount++;
        Ticked += () => TickCount++;
        Unregistered += () => UnregisterCount++;
    }

    public TComponent CallGetRequiredComponent<TComponent>()
        where TComponent : Component
    {
        return GetRequiredComponent<TComponent>();
    }
}
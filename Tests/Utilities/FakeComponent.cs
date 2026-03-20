using Termule.Core;

namespace Tests.Utilities;

internal class FakeComponent : Component
{
    internal int TickCount { get; private set; }

    internal bool RegisteredInvoked { get; private set; }

    internal bool UnregisteredInvoked { get; private set; }

    internal FakeComponent()
    {
        Registered += () => RegisteredInvoked = true;
        Ticked += () => TickCount++;
        Unregistered += () => UnregisteredInvoked = true;
    }

    internal TComponent CallGetRequiredComponent<TComponent>()
        where TComponent : Component
    {
        return GetRequiredComponent<TComponent>();
    }
}
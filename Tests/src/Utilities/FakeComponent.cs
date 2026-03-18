namespace Termule.Tests.Utilities;

using Termule.Core;

internal class FakeComponent : Component
{
    internal FakeComponent()
    {
        this.Registered += () => this.RegisteredInvoked = true;
        this.Ticked += () => this.TickCount++;
        this.Unregistered += () => this.UnregisteredInvoked = true;
    }

    internal int TickCount { get; private set; }

    internal bool RegisteredInvoked { get; private set; }

    internal bool UnregisteredInvoked { get; private set; }

    internal TComponent CallGetRequiredComponent<TComponent>()
        where TComponent : Component
    {
        return this.GetRequiredComponent<TComponent>();
    }
}
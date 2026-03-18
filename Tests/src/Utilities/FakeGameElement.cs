namespace Termule.Tests.Utilities;

using Termule.Core;

internal class FakeGameElement : GameElement
{
    internal FakeGameElement()
    {
        this.Registered += () => this.RegisteredInvoked = true;
        this.Unregistered += () => this.UnregisteredInvoked = true;
    }

    internal Game GameInstance => this.Game;

    internal bool RegisteredInvoked { get; private set; }

    internal bool UnregisteredInvoked { get; private set; }

    internal TSystem CallGetRequiredSystem<TSystem>()
        where TSystem : System
    {
        return this.GetRequiredSystem<TSystem>();
    }
}
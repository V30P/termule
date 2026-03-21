using Termule.Engine.Core;

namespace Termule.Tests.Utilities;

internal class FakeGameElement : GameElement
{
    internal bool RegisteredInvoked { get; private set; }

    internal bool UnregisteredInvoked { get; private set; }

    internal Game GameInstance => Game;

    internal FakeGameElement()
    {
        Registered += () => RegisteredInvoked = true;
        Unregistered += () => UnregisteredInvoked = true;
    }

    internal TSystem CallGetRequiredSystem<TSystem>()
        where TSystem : Engine.Core.System
    {
        return GetRequiredSystem<TSystem>();
    }
}
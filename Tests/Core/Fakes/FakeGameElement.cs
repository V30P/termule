using Termule.Engine.Core;

namespace Termule.Tests.Core.Fakes;

public class FakeGameElement : GameElement
{
    public bool RegisteredInvoked { get; private set; }

    public bool UnregisteredInvoked { get; private set; }

    public Game GameInstance => Game;

    public FakeGameElement()
    {
        Registered += () => RegisteredInvoked = true;
        Unregistered += () => UnregisteredInvoked = true;
    }

    public TSystem CallGetRequiredSystem<TSystem>()
        where TSystem : Engine.Core.System
    {
        return GetRequiredSystem<TSystem>();
    }
}
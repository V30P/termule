using Termule.Engine.Core;

namespace Termule.Tests.Core;

public class FakeGameElement : GameElement
{
    public bool HasBeenActivated { get; private set; }

    public bool HasBeenDeactivated { get; private set; }

    public Game GameInstance => Game;

    public TSystem CallGetRequiredSystem<TSystem>()
        where TSystem : Engine.Core.System
    {
        return GetRequiredSystem<TSystem>();
    }

    protected override void Activate()
    {
        HasBeenActivated = true;
    }

    protected override void Deactivate()
    {
        HasBeenDeactivated = true;
    }
}

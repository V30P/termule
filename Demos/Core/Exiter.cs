using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Input;

namespace Termule.Demos.Core;

internal sealed class Exiter : Component
{
    private Exiter()
    {
    }

    internal static GameObject CreateGameObject()
    {
        return [
            new Exiter(),
            new ComboControl([Button.LeftControl, Button.C]),
        ];
    }

    protected override void Tick()
    {
        if (GetRequiredComponent<ComboControl>().Value)
        {
            Game.Stop();
        }
    }
}

using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Types;

namespace Termule.Demos.Core;

internal class TpsIndicator : Component
{
    private const string TextTemplate = " TPS: {0}";

    private int ticks;
    private float time;

    protected override void Tick()
    {
        base.Tick();

        ticks++;
        time += Game.DeltaTime;
        if (time < 1)
        {
            return;
        }

        GameObject.Get<ContentRenderer<Text>>().Content.Value =
            string.Format(TextTemplate, (int) (ticks / time));
        time = ticks = 0;
    }
}
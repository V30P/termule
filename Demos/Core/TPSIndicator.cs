using System.Globalization;
using System.Text;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Types;

namespace Termule.Demos.Core;

internal sealed class TPSIndicator : Component
{
    private static readonly CompositeFormat TextFormat = CompositeFormat.Parse("TPS: {0}");

    private int ticks;
    private float time;

    private TPSIndicator()
    {
    }

    internal static GameObject CreateGameObject()
    {
        return [
            new TPSIndicator(),
            new Transform(),
            new ContentRenderer<Text>()
            {
                RenderInTargetSpace = true,
                Layer = Program.UILayer
            }
        ];
    }

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
            string.Format(CultureInfo.CurrentCulture, TextFormat, (int) (ticks / time));
        time = ticks = 0;
    }
}

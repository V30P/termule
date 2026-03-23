using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Types.Content;

namespace Termule.Demos.Application;

internal class TpsIndicator : GameObject
{
    private const string TextTemplate = " TPS: {0}";

    private int ticks;
    private float time;

    internal TpsIndicator()
    {
        Add(
            new Transform(),
            new ContentRenderer<Text>
            {
                DisplaySpace = true,
                Content = new Text { Value = string.Format(TextTemplate, "N/A") }
            });

        Ticked += OnTicked;
    }

    private void OnTicked()
    {
        ticks++;
        time += Game.DeltaTime;

        if (time < 1)
        {
            return;
        }

        Get<ContentRenderer<Text>>().Content.Value = string.Format(TextTemplate, (int)(ticks / time));
        time = ticks = 0;
    }
}
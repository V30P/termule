using Termule.Components;
using Termule.Core;
using Termule.Systems.RenderSystem;
using Termule.Types;

internal class TPSIndicator : GameObject
{
    private const string TextTemplate = " TPS: {0}";

    private float time;
    private int ticks;

    internal TPSIndicator()
    {
        Add(
            new Transform(),
            new ContentRenderer<Text>()
            {
                DisplaySpace = true,
                Content = new() { Value = string.Format(TextTemplate, "N/A") },
            });

        Ticked += OnTicked;
    }

    private void OnTicked()
    {
        time += Game.DeltaTime;
        ticks++;

        if (time > 1)
        {
            Get<ContentRenderer<Text>>().Content.Value = string.Format(TextTemplate, (int)(ticks / time));
            time = ticks = 0;
        }
    }
}
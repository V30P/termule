using Termule.Components;
using Termule.Core;
using Termule.Systems.RenderSystem;
using Termule.Types;

internal class FPSIndicator : GameObject
{
    private const string TextTemplate = " FPS: {0}";

    private float time;
    private int frames;

    internal FPSIndicator()
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
        frames++;

        if (time > 1)
        {
            Get<ContentRenderer<Text>>().Content.Value = string.Format(TextTemplate, (int)(frames / time));
            time = frames = 0;
        }
    }
}
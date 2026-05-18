using Termule.Demos.Core;
using Termule.Engine.Components;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Demos.Demos;

internal sealed class Gradient : Demo
{
    protected override void Start()
    {
        World.Add(
            new Transform(),
            new Camera(),
            new GradientRenderer()
        );
    }

    private sealed class GradientRenderer : Renderer
    {
        private float time;

        protected override void Tick()
        {
            time += Game.DeltaTime;
        }

        protected override void Render(FrameBuffer frame, Vector viewOrigin)
        {
            for (int x = 0; x < frame.Size.X; x++)
            {
                for (int y = 0; y < frame.Size.Y; y++)
                {
                    frame.Draw((x, y), GetColor(((float) x / frame.Size.X) + time));
                }
            }
        }

        private static Color GetColor(float phase)
        {
            float segmentPosition = phase % 1 * 6f;
            int segmentIndex = (int) segmentPosition;
            float segmentProgress = segmentPosition - segmentIndex;

            return segmentIndex switch
            {
                0 => (1, segmentProgress, 0),
                1 => (1 - segmentProgress, 1, 0),
                2 => (0, 1, segmentProgress),
                3 => (0, 1 - segmentProgress, 1),
                4 => (segmentProgress, 0, 1),
                _ => (1, 0, 1 - segmentProgress)
            };
        }
    }
}

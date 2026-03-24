using Termule.Demos.Application;
using Termule.Engine.Components;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Demos.Demos;

internal class Gradient : Demo
{
    protected override void Start()
    {
        Root.Add(
            new Transform(),
            new Camera(),
            new GradientRenderer());
    }

    private class GradientRenderer : Renderer
    {
        private float time;

        internal GradientRenderer()
        {
            Ticked += OnTicked;
        }

        protected override void Render(FrameBuffer frame, Vector viewOrigin)
        {
            for (var x = 0; x < frame.Size.X; x++)
            for (var y = 0; y < frame.Size.Y; y++)
            {
                frame.Draw((x, y), GetColor((float)x / frame.Size.X + time));
            }
        }

        private static Color GetColor(float phase)
        {
            var segmentPosition = phase % 1 * 6f;
            var segmentIndex = (int)segmentPosition;
            var segmentProgress = segmentPosition - segmentIndex;

            var intensity = (int)(segmentProgress * 255);
            return segmentIndex switch
            {
                0 => (255, intensity, 0),
                1 => (255 - intensity, 255, 0),
                2 => (0, 255, intensity),
                3 => (0, 255 - intensity, 255),
                4 => (intensity, 0, 255),
                _ => (255, 0, 255 - intensity)
            };
        }

        private void OnTicked()
        {
            time += Game.DeltaTime;
        }
    }
}
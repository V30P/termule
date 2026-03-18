using Termule.Components;
using Termule.Systems.RenderSystem;
using Termule.Types;

internal class Gradient : Demo
{
    protected override void Start()
    {
        Root.Add(
            new Transform(),
            new Camera() { MatchDisplaySize = true },
            new GradientRenderer());
    }

    internal class GradientRenderer : Renderer
    {
        private float time;

        internal GradientRenderer()
        {
            Ticked += OnTicked;
        }

        protected override void Render(Frame frame, Vector viewOrigin)
        {
            var test = Game.Systems.Get<RenderSystem>();

            for (int x = 0; x < frame.Size.X; x++)
            {
                for (int y = 0; y < frame.Size.Y; y++)
                {
                    frame.Contribute(
                        this,
                        (x, y),
                        GetColor(((float)x / frame.Size.X) + time));
                }
            }
        }

        private static Color GetColor(float phase)
        {
            float segmentPosition = (phase % 1) * 6f;
            int segmentIndex = (int)segmentPosition;
            float segmentProgress = segmentPosition - segmentIndex;

            int intensity = (int)(segmentProgress * 255);
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
using Termule.Demos.Core;
using Termule.Engine.Components;
using Termule.Engine.Components.Camera;
using Termule.Engine.Components.Renderers;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Demos.Demos;

internal class Raindrops : Demo
{
    private const float MinCooldown = 0.1f;
    private const float MaxCooldown = 0.5f;

    private readonly Random random = new();

    private float cooldown;

    protected override void Start()
    {
        Root.Add(
            new Transform(),
            new Camera { BackgroundCell = new Cell((0, 0, 0)) });
    }

    protected override void Tick()
    {
        cooldown -= Game.DeltaTime;
        if (cooldown > 0)
        {
            return;
        }

        Raindrop raindrop = new((float)random.NextDouble(), (float)random.NextDouble());
        Root.Add(raindrop);

        cooldown = (float)random.NextDouble() * (MaxCooldown - MinCooldown) + MinCooldown;
    }

    private class Raindrop : GameObject
    {
        private const float Lifespan = 2;

        private readonly Vector pos;

        private float time;

        internal Raindrop(float x, float y)
        {
            pos = (x, y);

            Add(
                new Transform(),
                new CircleRenderer
                {
                    TargetSpace = true,
                    DoubleWide = true
                });

            Ticked += OnTicked;
        }

        private void OnTicked()
        {
            time += Game.DeltaTime;
            if (time > Lifespan)
            {
                Destroy();
                return;
            }

            CircleRenderer circleRenderer = Get<CircleRenderer>();
            circleRenderer.Radius = GetRadius(time);
            circleRenderer.Color = (0, 0, (int)(255 * (1 - time / Lifespan)));

            VectorInt displaySize = Systems.Get<DisplaySystem>().Size;
            Get<Transform>().Pos = (pos.X * displaySize.X, pos.Y * displaySize.Y);
        }

        private float GetRadius(float t)
        {
            return Systems.Get<DisplaySystem>().Size.Magnitude * 0.02f * MathF.Log2(t + 1) + 1;
        }
    }
}
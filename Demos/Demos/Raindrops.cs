using Termule.Demos.Common;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types;

namespace Termule.Demos.Demos;

internal sealed class Raindrops : Demo
{
    private const float MinCooldown = 0.1f;
    private const float MaxCooldown = 0.5f;

    private readonly Random random = new();

    private float cooldown;

    protected override void Start()
    {
        GetRequiredSystem<DisplaySystem>().BackgroundCell = new Cell((0, 0, 0));

        World.Add(
            new Transform(),
            new Camera()
        );
    }

    protected override void Tick()
    {
        cooldown -= Game.DeltaTime;
        if (cooldown > 0)
        {
            return;
        }

        SpawnRaindrop((float) random.NextDouble(), (float) random.NextDouble());
        cooldown = ((float) random.NextDouble() * (MaxCooldown - MinCooldown)) + MinCooldown;
    }

    private void SpawnRaindrop(float x, float y)
    {
        World.Add(
            new GameObject(
                new Transform(),
                new CircleRenderer
                {
                    RenderInTargetSpace = true,
                    DoubleWide = true
                },
                new RaindropController((x, y))
            )
        );
    }

    private sealed class RaindropController(Vector pos) : Component
    {
        private const float Lifespan = 2;
        private float time;

        protected override void Tick()
        {
            time += Game.DeltaTime;
            if (time > Lifespan)
            {
                GameObject.Destroy();
                return;
            }

            CircleRenderer circleRenderer = GetRequiredComponent<CircleRenderer>();
            circleRenderer.Radius = GetRadius(time);
            circleRenderer.Color = (0, 0, 1 - (time / Lifespan));

            VectorInt displaySize = GetRequiredSystem<DisplaySystem>().Size;
            GetRequiredComponent<Transform>().Pos = (pos.X * displaySize.X, pos.Y * displaySize.Y);
        }

        private float GetRadius(float t)
        {
            float displayMagnitude = GetRequiredSystem<DisplaySystem>().Size.Magnitude;
            return (displayMagnitude * 0.02f * MathF.Log2(t + 1)) + 1;
        }
    }
}

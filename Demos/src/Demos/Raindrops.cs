using Termule.Components;
using Termule.Core;
using Termule.Systems.Display;
using Termule.Types;

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
            new Camera()
            {
                MatchDisplaySize = true,
                BackgroundColor = (0, 0, 0),
            });
    }

    protected override void Tick()
    {
        cooldown -= Game.DeltaTime;
        if (cooldown < 0)
        {
            Raindrop raindrop = new((float)random.NextDouble(), (float)random.NextDouble());
            Root.Add(raindrop);

            cooldown = ((float)random.NextDouble() * (MaxCooldown - MinCooldown)) + MinCooldown;
        }
    }

    internal class Raindrop : GameObject
    {
        private const float Lifespan = 2;

        private readonly Vector pos;
        private float time;

        internal Raindrop(float x, float y)
        {
            pos = (x, y);

            Add(
                new Transform(),
                new CircleRenderer()
                {
                    DisplaySpace = true,
                    DoubleWide = true,
                });

            Ticked += OnTicked;
        }

        private void OnTicked()
        {
            time += Game.DeltaTime;
            if (time > Lifespan)
            {
                GameObject.Remove(this);
                return;
            }

            CircleRenderer circleRenderer = Get<CircleRenderer>();
            circleRenderer.Radius = GetRadius(time);
            circleRenderer.Color = (0, 0, (int)(255 * (1 - (time / Lifespan))));

            VectorInt displaySize = Systems.Get<Display>().Size;
            Get<Transform>().Pos = (pos.X * displaySize.X, pos.Y * displaySize.Y);
        }

        private float GetRadius(float time)
        {
            return (Systems.Get<Display>().Size.Magnitude * 0.02f * MathF.Log2(time + 1)) + 1;
        }
    }
}
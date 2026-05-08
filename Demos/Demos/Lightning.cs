using Termule.Demos.Core;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types;

namespace Termule.Demos.Demos;

internal class Lightning : Demo
{
    private const float MinCooldown = 0.25f;
    private const float MaxCooldown = 1;

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

        SpawnBolt();
        cooldown = ((float)random.NextDouble() * (MaxCooldown - MinCooldown)) + MinCooldown;
    }

    private void SpawnBolt()
    {
        Root.Add(new GameObject(
            new Transform(),
            new BoltController(random)
        ));
    }

    private class BoltController(Random random) : Component
    {
        private const float Lifespan = 0.5f;
        private const float BendGenerations = 5;
        private const float BranchChance = 0.2f;
        private const float OffsetToDisplayRatio = 0.1f;

        private float timeRemaining = Lifespan;

        protected override void Activate()
        {
            Vector origin = ((float)random.NextDouble() * Systems.Get<DisplaySystem>().Size.X, 0);
            Vector target = (Systems.Get<DisplaySystem>().Size.X / 2, Systems.Get<DisplaySystem>().Size.Y);

            List<List<Vector>> branches = [[origin, target]];
            float maxOffset = Systems.Get<DisplaySystem>().Size.X * OffsetToDisplayRatio;
            for (int generation = 0; generation < BendGenerations; generation++)
            {
                int branchCount = branches.Count;
                for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
                {
                    List<Vector> line = branches[branchIndex];
                    for (int pointIndex = 0; pointIndex < line.Count - 1; pointIndex += 2)
                    {
                        Vector prev = line[pointIndex];
                        Vector next = line[pointIndex + 1];
                        Vector midpoint = (prev + next) / 2;
                        Vector normal = (next - prev).Normalized.Perpendicular();

                        line.Insert(pointIndex + 1, GenerateDisplacedMidpoint());

                        // Create a new branch off of the previous point
                        if (random.NextDouble() > 1 - BranchChance)
                        {
                            branches.Add([prev, GenerateDisplacedMidpoint()]);
                        }

                        continue;

                        Vector GenerateDisplacedMidpoint()
                        {
                            return midpoint + (normal * (((float)random.NextDouble() * maxOffset * 2) - maxOffset));
                        }
                    }
                }

                maxOffset /= 2;
            }

            foreach (List<Vector> branch in branches)
            {
                GameObject.Add(
                    new LineRenderer { Color = (255, 255, 255), TargetSpace = true, Points = branch }
                );
            }
        }

        protected override void Tick()
        {
            timeRemaining -= Game.DeltaTime;
            if (timeRemaining < 0)
            {
                GameObject.Destroy();
            }
        }
    }
}

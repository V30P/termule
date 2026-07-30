using Termule.Demos.Common;
using Termule.Demos.Core;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Resources;
using Termule.Engine.Types;
using static Termule.Demos.Common.Utilities;

namespace Termule.Demos.Demos;

internal sealed class Shooter : Demo, IMessageListener<Shooter.CharacterController.DiedMessage>
{
    private const float GracePeriodLength = 3;
    private const float GameOverLength = 5;

    private static Image characterSprite;
    private static Image projectileSprite;

    private float gameOverTimeRemaining;
    private float gracePeriodTimeRemaining = GracePeriodLength;

    private int roundNumber = 1;
    private int enemiesRemaining;

    void IMessageListener<CharacterController.DiedMessage>.OnMessage(
        CharacterController.DiedMessage message)
    {
        if (message.Type == typeof(PlayerController))
        {
            // Start game over when the player dies
            gameOverTimeRemaining = GameOverLength;
        }
        else if (message.Type == typeof(EnemyController))
        {
            // Start grace period when the last enemy dies
            enemiesRemaining--;
            if (enemiesRemaining == 0)
            {
                gracePeriodTimeRemaining = GracePeriodLength;
                roundNumber++;
            }
        }
    }

    protected override void Start()
    {
        characterSprite = Systems.Get<ResourceLoader>().Load<Image>("shooter/character");
        projectileSprite = Systems.Get<ResourceLoader>().Load<Image>("shooter/projectile");

        World.Add(
            new Transform(),
            new Camera(),
            new ContentRenderer<Text>
            {
                Centered = true,
                Layer = Program.UILayer
            }
        );

        Game.Bus.Subscribe(this);

        SpawnCharacter<PlayerController>((0, 0));
    }

    protected override void Tick()
    {
        if (gameOverTimeRemaining > 0)
        {
            World.Get<ContentRenderer<Text>>().Content.Value =
                $"""
                     GAME OVER
                 ROUNDS SURVIVED: {roundNumber - 1}
                 """;

            // Stop the game when the game over screen is finished
            gameOverTimeRemaining -= Game.DeltaTime;
            if (gameOverTimeRemaining <= 0)
            {
                Game.Stop();
            }
        }
        else if (gracePeriodTimeRemaining > 0)
        {
            World.Get<ContentRenderer<Text>>().Content.Value = $"ROUND {roundNumber}";

            // Start next round when grace period is finished
            gracePeriodTimeRemaining -= Game.DeltaTime;
            if (gracePeriodTimeRemaining <= 0)
            {
                World.Get<ContentRenderer<Text>>().Content.Value = null;
                for (int i = 0; i < roundNumber; i++)
                {
                    SpawnCharacter<EnemyController>(
                        PointOnRectangle(
                            -Systems.Get<DisplaySystem>().Size / 2,
                            Systems.Get<DisplaySystem>().Size
                        )
                    );

                    enemiesRemaining++;
                }
            }
        }
    }

    private void SpawnCharacter<TController>(Vector pos)
        where TController : CharacterController, new()
    {
        Game.World.Add(new GameObject(
            new Transform { Pos = pos },
            new ContentRenderer<Image> { Centered = true },
            new TController()
        ));
    }

    private abstract class CharacterController : Component
    {
        private const float HitColorLength = 0.05f;

        private int hp = 3;
        private float shotCooldownTimeRemaining;
        private float hitColorTimeRemaining;

        protected Vector MovementDir { get; set; }

        protected Vector Target { get; set; }

        protected abstract Color Color { get; }

        protected abstract Color HitColor { get; }

        protected abstract float Speed { get; }

        protected abstract float ShotCooldownLength { get; }

        internal void Hit()
        {
            hp--;
            hitColorTimeRemaining = HitColorLength;
            if (hp == 0)
            {
                Game.Bus.Broadcast(new DiedMessage(GetType()));
                GameObject.Destroy();
            }
        }

        protected override void Tick()
        {
            Transform transform = GetRequiredComponent<Transform>();
            transform.Pos += MovementDir.Normalized.ScaleToCells() * Speed * Game.DeltaTime;

            ContentRenderer<Image> renderer = GetRequiredComponent<ContentRenderer<Image>>();
            renderer.FlipX = Target.X < transform.Pos.X;

            hitColorTimeRemaining -= Game.DeltaTime;
            renderer.Content = characterSprite.WithColorSwapped(
                BasicColor.White,
                hitColorTimeRemaining < 0 ? Color : HitColor
            );

            shotCooldownTimeRemaining -= Game.DeltaTime;
        }

        protected void ShootAtTarget()
        {
            if (shotCooldownTimeRemaining >= 0)
            {
                return;
            }

            Game.World.Add(new GameObject(
                new Transform { Pos = GetRequiredComponent<IPositionProvider>().Pos },
                new ContentRenderer<Image>
                {
                    Centered = true,
                    Content = new Image(projectileSprite).WithColorSwapped(BasicColor.White, Color)
                },
                new ProjectileController(
                    GetType(),
                    (Target - GetRequiredComponent<IPositionProvider>().Pos).Normalized
                )
            ));

            shotCooldownTimeRemaining = ShotCooldownLength;
        }

        internal readonly struct DiedMessage(Type type)
        {
            public readonly Type Type = type;
        }
    }

    private sealed class PlayerController : CharacterController
    {
        private readonly VectorControl movementControl = new(Button.W, Button.A, Button.S, Button.D);
        private readonly HoldControl fireControl = new(Button.MouseLeft);
        private readonly MouseControl targetControl = new();

        protected override Color Color => BasicColor.Blue;

        protected override Color HitColor => BasicColor.BrightBlue;

        protected override float Speed => 15;

        protected override float ShotCooldownLength => 0.5f;

        protected override void Activate()
        {
            GameObject.Add(movementControl, fireControl, targetControl);
        }

        protected override void Tick()
        {
            base.Tick();

            MovementDir = movementControl.Value;
            Target = World.Get<Camera>().TargetToGamePos(targetControl.Value);

            if (fireControl.Value)
            {
                ShootAtTarget();
            }
        }
    }

    private sealed class EnemyController : CharacterController
    {
        private const float Range = 30;

        protected override Color Color => BasicColor.Red;

        protected override Color HitColor => BasicColor.BrightRed;

        protected override float Speed => 7.5f;

        protected override float ShotCooldownLength => 1;

        protected override void Tick()
        {
            base.Tick();

            if (
                Game.World
                    .GetAll<GameObject>()
                    .FirstOrDefault(g => g.Get<PlayerController>() is not null) is { } player)
            {
                Vector pos = GetRequiredComponent<IPositionProvider>().Pos;
                Target = player.Get<Transform>().Pos;

                Vector displacement = pos - Target;
                if (displacement.Magnitude > Range)
                {
                    MovementDir = Target - pos;
                }
                else
                {
                    MovementDir = (0, 0);
                    ShootAtTarget();
                }
            }
            else
            {
                MovementDir = (0, 0);
            }
        }
    }

    private sealed class ProjectileController(Type sourceType, Vector direction) : Component
    {
        private const float Speed = 30;

        protected override void Tick()
        {
            GetRequiredComponent<Transform>().Pos += direction.Normalized.ScaleToCells()
                * Speed
                * Game.DeltaTime;

            // Detect hits
            IEnumerable<CharacterController> targets = World.GetAll<GameObject>()
                .Select(g => g.Get<CharacterController>())
                .Where(c => c != null && c.GetType() != sourceType);
            foreach (CharacterController target in targets)
            {
                Vector targetPos = target.GameObject.Get<IPositionProvider>().Pos;
                Vector projectilePos = GetRequiredComponent<IPositionProvider>().Pos;

                bool overlappingHorizontally = MathF.Abs(targetPos.X - projectilePos.X)
                    < ((float) characterSprite.Size.X + projectileSprite.Size.X) / 2;
                bool overlappingVertically = MathF.Abs(targetPos.Y - projectilePos.Y)
                    < ((float) characterSprite.Size.Y + projectileSprite.Size.Y) / 2;
                if (overlappingHorizontally && overlappingVertically)
                {
                    target.Hit();
                    GameObject.Destroy();
                    break;
                }
            }
        }
    }
}

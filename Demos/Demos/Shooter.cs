using Termule.Demos.Core;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Resources;
using Termule.Engine.Types;
using static Termule.Demos.Core.Utilities;

namespace Termule.Demos.Demos;

internal class Shooter : Demo, IMessageListener<Shooter.CharacterController.DiedMessage>
{
    private const float GracePeriodLength = 3;
    private const float GameOverLength = 5;

    private readonly Random random = new();

    private static Image characterSprite;
    private static Image projectileSprite;

    private float gameOverTimeRemaining;
    private float gracePeriodTimeRemaining = GracePeriodLength;

    private int roundNumber = 1;
    private int enemiesRemaining;

    void IMessageListener<CharacterController.DiedMessage>.OnMessage(CharacterController.DiedMessage message)
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

        Systems.Get<Keyboard>().Binds = new BindMap
        {
            { "Movement", new VectorBind(Button.W, Button.A, Button.S, Button.D) },
            { "Fire", new ButtonBind(Button.Mouse1) }
        };

        Root.Add(
            new Transform(),
            new Camera(),
            new ContentRenderer<Text> { Centered = true, Layer = Program.UILayer }
        );

        Game.Bus.Subscribe(this);

        SpawnCharacter<PlayerController>((0, 0));
    }

    protected override void Tick()
    {
        if (gameOverTimeRemaining > 0)
        {
            Root.Get<ContentRenderer<Text>>().Content.Value =
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
            Root.Get<ContentRenderer<Text>>().Content.Value = $"ROUND {roundNumber}";

            // Start next round when grace period is finished
            gracePeriodTimeRemaining -= Game.DeltaTime;
            if (gracePeriodTimeRemaining <= 0)
            {
                Root.Get<ContentRenderer<Text>>().Content.Value = null;
                for (int i = 0; i < roundNumber; i++)
                {
                    SpawnCharacter<EnemyController>(PointOnRectangle(
                        random,
                        -Systems.Get<DisplaySystem>().Size / 2,
                        Systems.Get<DisplaySystem>().Size)
                    );

                    enemiesRemaining++;
                }
            }
        }
    }

    private void SpawnCharacter<TController>(Vector pos) where TController : CharacterController, new()
    {
        Game.Root.Add(new GameObject(
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

        protected override void Tick()
        {
            Transform transform = GameObject.Get<Transform>();
            transform.Pos += ScaleVelocity(MovementDir.Normalized * Speed) * Game.DeltaTime;

            hitColorTimeRemaining -= Game.DeltaTime;
            GameObject.Get<ContentRenderer<Image>>().Content =
                (Target.X > transform.Pos.X ? characterSprite : characterSprite.Flipped())
                .WithColorSwapped(BasicColor.White, hitColorTimeRemaining < 0 ? Color : HitColor);

            shotCooldownTimeRemaining -= Game.DeltaTime;
        }

        protected void ShootAtTarget()
        {
            if (shotCooldownTimeRemaining >= 0)
            {
                return;
            }

            Game.Root.Add(new GameObject(
                new Transform { Pos = GameObject.Get<Transform>().Pos },
                new ContentRenderer<Image>
                {
                    Centered = true,
                    Content = new Image(projectileSprite).WithColorSwapped(BasicColor.White, Color)
                },
                new ProjectileController(GetType(), (Target - GameObject.Get<Transform>().Pos).Normalized)
            ));

            shotCooldownTimeRemaining = ShotCooldownLength;
        }

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

        internal record struct DiedMessage(Type Type);
    }

    private class PlayerController : CharacterController
    {
        protected override Color Color => BasicColor.Blue;
        protected override Color HitColor => BasicColor.BrightBlue;
        protected override float Speed => 15;
        protected override float ShotCooldownLength => 0.5f;

        protected override void Tick()
        {
            base.Tick();

            MovementDir = Systems.Get<Keyboard>().Get<Vector>("Movement");
            Target = Root.Get<Camera>().TargetToGamePos(Systems.Get<DisplaySystem>().MousePos);

            if (Systems.Get<Keyboard>().Get<bool>("Fire"))
            {
                ShootAtTarget();
            }
        }
    }

    private class EnemyController : CharacterController
    {
        private const float Range = 30;

        protected override Color Color => BasicColor.Red;
        protected override Color HitColor => BasicColor.BrightRed;
        protected override float Speed => 7.5f;
        protected override float ShotCooldownLength => 1;

        protected override void Tick()
        {
            base.Tick();

            if (Game.Root.GetAll<GameObject>()
                    .FirstOrDefault(g => g.Get<PlayerController>() is not null) is { } player)
            {
                Vector pos = GameObject.Get<Transform>().Pos;
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

    private class ProjectileController(Type sourceType, Vector direction) : Component
    {
        private const float Speed = 30;

        protected override void Tick()
        {
            GameObject.Get<Transform>().Pos += ScaleVelocity(direction * Speed) * Game.DeltaTime;
            foreach (CharacterController character in Root.GetAll<GameObject>()
                         .Select(g => g.Get<CharacterController>())
                         .Where(c => c != null && c.GetType() != sourceType)
                    )
            {
                Vector characterPos = character.GameObject.Get<Transform>().Pos;
                Vector projectilePos = GameObject.Get<Transform>().Pos;

                if (MathF.Abs(characterPos.X - projectilePos.X) <
                    (float)(characterSprite.Size.X + projectileSprite.Size.X) / 2
                    && MathF.Abs(characterPos.Y - projectilePos.Y) <
                    (float)(characterSprite.Size.Y + projectileSprite.Size.Y) / 2)
                {
                    character.Hit();
                    GameObject.Destroy();
                    break;
                }
            }
        }
    }
}

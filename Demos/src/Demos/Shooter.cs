using Termule.Components;
using Termule.Core;
using Termule.Systems.Controller;
using Termule.Systems.Controller.Keyboard;
using Termule.Systems.Display;
using Termule.Systems.ResourceLoader;
using Termule.Types;
using static Utilities;

internal class Shooter : Demo
{
    private const float GracePeriodLength = 3;
    private const float GameOverLength = 5;

    private readonly Random random = new();

    private State state;
    private int roundNumber;
    private float gracePeriodTimeRemaining;
    private float gameOverTimeRemaining;

    private enum State
    {
        GracePeriod,
        Round,
        GameOver,
    }

    protected override void Start()
    {
        Systems.Get<Controller>().Binds = new BindMap()
        {
            { Binds.Movement, new VectorBind(Button.W, Button.A, Button.S, Button.D) },
            { Binds.Fire, new ButtonBind(Button.Mouse1) },
        };

        Root.Add(
            new Transform(),
            new Camera() { MatchDisplaySize = true },
            new Player(),
            new ContentRenderer<Text>()
            {
                Centered = true,
                Layer = Program.UILayer,
            });
    }

    protected override void Update()
    {
        switch (state)
        {
            case State.GracePeriod:
                gracePeriodTimeRemaining -= Game.DeltaTime;
                if (gracePeriodTimeRemaining <= 0)
                {
                    StartRound();
                }

                break;
            case State.Round:
                if (Root.Get<Player>() == null)
                {
                    StartGameOver();
                }
                else if (Root.Get<Enemy>() == null)
                {
                    StartGracePeriod();
                }

                break;
            case State.GameOver:
                gameOverTimeRemaining -= Game.DeltaTime;
                if (gameOverTimeRemaining <= 0)
                {
                    Game.Stop();
                }

                break;
        }
    }

    private void StartGracePeriod()
    {
        Root.Get<ContentRenderer<Text>>().Content.Value = $"ROUND {++roundNumber}";
        gracePeriodTimeRemaining = GracePeriodLength;

        state = State.GracePeriod;
    }

    private void StartRound()
    {
        Root.Get<ContentRenderer<Text>>().Content.Value = null;

        for (int i = 0; i < roundNumber; i++)
        {
            Enemy enemy = [];
            enemy.Get<Transform>().Pos = PointOnRectangle(
                random,
                -Systems.Get<Display>().Size / 2,
                Systems.Get<Display>().Size);
            Root.Add(enemy);
        }

        state = State.Round;
    }

    private void StartGameOver()
    {
        Root.Get<ContentRenderer<Text>>().Content.Value =
        $"""
                GAME OVER
            ROUNDS SURVIVED: {roundNumber}
        """;
        gameOverTimeRemaining = GameOverLength;

        state = State.GameOver;
    }

    internal static class Binds
    {
        internal static readonly string Movement = "Movement";
        internal static readonly string Fire = "Fire";
    }

    internal abstract class Character : GameObject
    {
        private const float HitColorLength = 0.05f;
        private static Image sprite;

        private int hp = 3;
        private float hitColorTimeRemaining;
        private float shotCooldownTimeRemaining;

        internal Character()
        {
            Add(
                new Transform(),
                new ContentRenderer<Content>() { Centered = true });

            Registered += OnRegistered;
            Ticked += OnTicked;
        }

        internal abstract Color Color { get; }

        internal abstract Color HitColor { get; }

        protected abstract float Speed { get; }

        protected abstract float ShotCooldownLength { get; }

        protected Vector MovementDir { get; set; }

        protected Vector Target { get; set; }

        internal void Hit()
        {
            hp--;
            hitColorTimeRemaining = HitColorLength;
            if (hp < 0)
            {
                Destroy();
            }
        }

        protected void ShootAtTarget()
        {
            if (shotCooldownTimeRemaining <= 0)
            {
                Root.Add(new Projectile(this, Get<Transform>().Pos, Target));
                shotCooldownTimeRemaining = ShotCooldownLength;
            }
        }

        private void OnRegistered()
        {
            sprite ??= new(Systems.Get<ResourceLoader>().Load<Content>("Shooter/Character"));
        }

        private void OnTicked()
        {
            Transform transform = Get<Transform>();
            transform.Pos += ScaleVelocity(MovementDir.Normalized * Speed) * Game.DeltaTime;

            hitColorTimeRemaining -= Game.DeltaTime;
            Get<ContentRenderer<Content>>().Content =
                (Target.X > transform.Pos.X ? sprite : sprite.Flipped())
                .WithColorSwapped(BasicColor.White, hitColorTimeRemaining < 0 ? Color : HitColor);

            shotCooldownTimeRemaining -= Game.DeltaTime;
        }
    }

    internal class Player : Character
    {
        public Player()
        {
            Ticked += OnTicked;
        }

        internal override Color Color => BasicColor.Blue;

        internal override Color HitColor => BasicColor.BrightBlue;

        protected override float Speed => 15;

        protected override float ShotCooldownLength => 0.5f;

        private void OnTicked()
        {
            MovementDir = Systems.Get<Controller>().Get<Vector>(Binds.Movement);
            Target = Root.Get<Camera>().DisplayToGamePos(Systems.Get<Display>().MousePos);

            if (Systems.Get<Controller>().Get<bool>(Binds.Fire))
            {
                ShootAtTarget();
            }
        }
    }

    internal class Enemy : Character
    {
        private const float Range = 30;

        public Enemy()
        {
            Ticked += OnTicked;
        }

        internal override Color Color => BasicColor.Red;

        internal override Color HitColor => BasicColor.BrightRed;

        protected override float Speed => 7.5f;

        protected override float ShotCooldownLength => 1;

        private void OnTicked()
        {
            if (Root.Get<Player>() is Player player)
            {
                Vector pos = Get<Transform>().Pos;
                Target = player.Get<Transform>().Pos;

                if ((pos - Target).Magnitude > Range)
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

    private class Projectile : GameObject
    {
        private const float Speed = 30;

        private readonly Type sourceType;
        private readonly Color sourceColor;
        private readonly Vector direction;

        internal Projectile(Character source, Vector position, Vector target)
        {
            sourceType = source.GetType();
            sourceColor = source.Color;
            direction = (target - position).Normalized;

            Add(
                new Transform() { Pos = position },
                new ContentRenderer<Content>() { Centered = true });

            Registered += OnRegistered;
            Ticked += OnTicked;
        }

        private void OnRegistered()
        {
            Get<ContentRenderer<Content>>().Content =
                new Image(Systems.Get<ResourceLoader>().Load<Content>("Shooter/Projectile"))
                .WithColorSwapped(BasicColor.White, sourceColor);
        }

        private void OnTicked()
        {
            Get<Transform>().Pos += ScaleVelocity(direction * Speed) * Game.DeltaTime;

            foreach (Renderer overlapper in Root.Get<Camera>().GetOverlappers(Get<ContentRenderer<Content>>()))
            {
                if
                (
                    overlapper.GameObject is Character character
                    && character.GetType() != sourceType
                )
                {
                    character.Hit();
                    Destroy();
                    return;
                }
            }
        }
    }
}
using Termule.Demos.Application;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Controller;
using Termule.Engine.Systems.Controller.Keyboard;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.ResourceLoader;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;
using static Termule.Demos.Application.Utilities;

namespace Termule.Demos.Demos;

internal class Shooter : Demo
{
    private const float GracePeriodLength = 3;
    private const float GameOverLength = 5;

    private readonly Random random = new();

    private static Image _characterSprite;
    private static Image _projectileSprite;

    private float gameOverTimeRemaining;
    private float gracePeriodTimeRemaining;
    private int roundNumber;

    private State state;

    private enum State
    {
        GracePeriod,
        Round,
        GameOver
    }

    protected override void Start()
    {
        Systems.Get<Controller>().Binds = new BindMap
        {
            { Binds.Movement, new VectorBind(Button.W, Button.A, Button.S, Button.D) },
            { Binds.Fire, new ButtonBind(Button.Mouse1) }
        };

        _characterSprite = new Image(Systems.Get<ResourceLoader>().Load<Content>("Shooter/Character"));
        _projectileSprite = new Image(Systems.Get<ResourceLoader>().Load<Content>("Shooter/Projectile"));

        Root.Add(
            new Transform(),
            new Camera(),
            new Player(),
            new ContentRenderer<Text>
            {
                Centered = true,
                Layer = Program.UiLayer
            });
    }

    protected override void Tick()
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

        for (var i = 0; i < roundNumber; i++)
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
                 ROUNDS SURVIVED: {roundNumber - 1}
             """;
        gameOverTimeRemaining = GameOverLength;

        state = State.GameOver;
    }

    private static class Binds
    {
        internal const string Movement = "Movement";
        internal const string Fire = "Fire";
    }

    private abstract class Character : GameObject
    {
        private const float HitColorLength = 0.05f;

        private float hitColorTimeRemaining;

        private int hp = 3;
        private float shotCooldownTimeRemaining;

        protected Vector MovementDir { get; set; }
        protected Vector Target { get; set; }

        internal abstract Color Color { get; }
        protected abstract Color HitColor { get; }
        protected abstract float Speed { get; }
        protected abstract float ShotCooldownLength { get; }

        protected Character()
        {
            Add(
                new Transform(),
                new ContentRenderer<Content>
                {
                    Centered = true,
                    Content = _characterSprite
                });

            Ticked += OnTicked;
        }

        internal void Hit()
        {
            hp--;
            hitColorTimeRemaining = HitColorLength;
            if (hp == 0)
            {
                Destroy();
            }
        }

        protected void ShootAtTarget()
        {
            if (shotCooldownTimeRemaining >= 0)
            {
                return;
            }

            Root.Add(new Projectile(this, Get<Transform>().Pos, Target));
            shotCooldownTimeRemaining = ShotCooldownLength;
        }

        private void OnTicked()
        {
            var transform = Get<Transform>();
            transform.Pos += ScaleVelocity(MovementDir.Normalized * Speed) * Game.DeltaTime;

            hitColorTimeRemaining -= Game.DeltaTime;
            Get<ContentRenderer<Content>>().Content =
                (Target.X > transform.Pos.X ? _characterSprite : _characterSprite.Flipped())
                .WithColorSwapped(BasicColor.White, hitColorTimeRemaining < 0 ? Color : HitColor);

            shotCooldownTimeRemaining -= Game.DeltaTime;
        }
    }

    private class Player : Character
    {
        internal override Color Color => BasicColor.Blue;
        protected override Color HitColor => BasicColor.BrightBlue;
        protected override float Speed => 15;
        protected override float ShotCooldownLength => 0.5f;

        public Player()
        {
            Ticked += OnTicked;
        }

        private void OnTicked()
        {
            MovementDir = Systems.Get<Controller>().Get<Vector>(Binds.Movement);
            Target = Root.Get<Camera>().TargetToGamePos(Systems.Get<Display>().MousePos);

            if (Systems.Get<Controller>().Get<bool>(Binds.Fire))
            {
                ShootAtTarget();
            }
        }
    }

    private class Enemy : Character
    {
        private const float Range = 30;

        internal override Color Color => BasicColor.Red;
        protected override Color HitColor => BasicColor.BrightRed;
        protected override float Speed => 7.5f;
        protected override float ShotCooldownLength => 1;

        public Enemy()
        {
            Ticked += OnTicked;
        }

        private void OnTicked()
        {
            if (Root.Get<Player>() is { } player)
            {
                var pos = Get<Transform>().Pos;
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

        private readonly Vector direction;
        private readonly Type sourceType;

        internal Projectile(Character source, Vector position, Vector target)
        {
            sourceType = source.GetType();
            direction = (target - position).Normalized;

            Add(new Transform { Pos = position }, new ContentRenderer<Content>
            {
                Centered = true,
                Content = new Image(_projectileSprite).WithColorSwapped(BasicColor.White, source.Color)
            });

            Ticked += OnTicked;
        }

        private void OnTicked()
        {
            Get<Transform>().Pos += ScaleVelocity(direction * Speed) * Game.DeltaTime;

            foreach (var character in Root.GetAll<Character>())
            {
                if (character.GetType() == sourceType)
                {
                    continue;
                }

                var characterPos = character.Get<Transform>().Pos;
                var projectilePos = Get<Transform>().Pos;

                if (MathF.Abs(characterPos.X - projectilePos.X) <
                    (float)(_characterSprite.Size.X + _projectileSprite.Size.X) / 2
                    && MathF.Abs(characterPos.Y - projectilePos.Y) <
                    (float)(_characterSprite.Size.Y + _projectileSprite.Size.Y) / 2)
                {
                    character.Hit();
                    Destroy();
                    return;
                }
            }
        }
    }
}
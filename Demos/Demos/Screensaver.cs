using Termule.Demos.Common;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Resources;
using Termule.Engine.Types;

namespace Termule.Demos.Demos;

internal sealed class Screensaver : Demo
{
    protected override void Start()
    {
        World.Add(
            new Transform(),
            new Camera(),
            new GameObject(
                new Transform(),
                new ContentRenderer<Image> { RenderInTargetSpace = true },
                new LogoController()
            )
        );
    }

    private sealed class LogoController : Component
    {
        private const float Speed = 0.25f;

        private readonly Color[] colors =
        [
            (1, 0, 0),
            (1, 1, 0),
            (1, 0, 1),
            (0, 1, 1),
            (1, 0, 0),
            (0, 1, 0),
            (1, 0.65f, 1)
        ];

        private readonly Vector unsignedDir = new Vector(1.5f, 1).Normalized;

        private Color currentColor;
        private Vector dir;

        private Image logo;
        private Vector pos;

        protected override void Activate()
        {
            logo = GetRequiredSystem<ResourceLoader>().Load<Image>("screensaver/logo.tmc");
            GetRequiredComponent<ContentRenderer<Image>>().Content = logo;

            dir = unsignedDir;
            currentColor = BasicColor.White;
            RandomizeColor();
        }

        protected override void Tick()
        {
            Transform transform = GetRequiredComponent<Transform>();
            VectorInt displaySize = GetRequiredSystem<DisplaySystem>().Size;

            if (transform.Pos.Y < 0 && Math.Abs(dir.Y - unsignedDir.Y) > 0.01f)
            {
                dir = new Vector(dir.X, unsignedDir.Y);
                RandomizeColor();
            }
            else if (
                transform.Pos.Y + logo.Size.Y > displaySize.Y + 0.5f
                && Math.Abs(dir.Y - -unsignedDir.Y) > 0.01f)
            {
                dir = new Vector(dir.X, -unsignedDir.Y);
                RandomizeColor();
            }

            if (transform.Pos.X < 0 && Math.Abs(dir.X - unsignedDir.X) > 0.01f)
            {
                dir = new Vector(unsignedDir.X, dir.Y);
                RandomizeColor();
            }
            else if (
                transform.Pos.X + logo.Size.X > displaySize.X + 0.5f
                && Math.Abs(dir.X - -unsignedDir.X) > 0.01f)
            {
                dir = new Vector(-unsignedDir.X, dir.Y);
                RandomizeColor();
            }

            pos += dir * Speed * Game.DeltaTime;
            transform.Pos = (displaySize.X * pos.X, displaySize.Y * pos.Y);
        }

        private void RandomizeColor()
        {
            IEnumerable<Color> otherColors = colors
                .Where(c => c != currentColor);
            Color newColor = otherColors.ElementAt(Random.Shared.Next(otherColors.Count()));

            ContentRenderer<Image> imageRenderer = GetRequiredComponent<ContentRenderer<Image>>();
            imageRenderer.Content = imageRenderer.Content.WithColorSwapped(currentColor, newColor);
            currentColor = newColor;
        }
    }
}

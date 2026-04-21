using Termule.Demos.Core;
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Resources;
using Termule.Engine.Types;

namespace Termule.Demos.Demos;

internal class Screensaver : Demo
{
    protected override void Start()
    {
        Root.Add(
            new Transform(),
            new Camera(),
            new Logo());
    }

    private class Logo : GameObject
    {
        private const float Speed = 0.25f;

        private readonly Color[] colors =
        [
            (255, 0, 0),
            (255, 127, 0),
            (255, 255, 0),
            (0, 255, 0),
            (0, 0, 255),
            (75, 0, 130),
            (143, 0, 255)
        ];

        private readonly Random random = new();
        private readonly Vector unsignedDir = new Vector(1.5f, 1).Normalized;

        private Color currentColor;
        private Vector dir;

        private Image logo;
        private Vector pos;

        internal Logo()
        {
            Add(
                new Transform(),
                new ContentRenderer<Image> { TargetSpace = true });

            Registered += OnRegistered;
            Ticked += OnTicked;
        }

        private void OnRegistered()
        {
            logo = Systems.Get<ResourceLoader>().Load<Image>("screensaver/logo");
            Get<ContentRenderer<Image>>().Content = logo;

            dir = unsignedDir;
            currentColor = BasicColor.White;
            RandomizeColor();
        }

        private void OnTicked()
        {
            Transform transform = Get<Transform>();
            VectorInt displaySize = Systems.Get<DisplaySystem>().Size;

            if (transform.Pos.Y < 0 && Math.Abs(dir.Y - unsignedDir.Y) > 0.01f)
            {
                dir = dir with { Y = unsignedDir.Y };
                RandomizeColor();
            }
            else if (transform.Pos.Y + logo.Size.Y > displaySize.Y && Math.Abs(dir.Y - -unsignedDir.Y) > 0.01f)
            {
                dir = dir with { Y = -unsignedDir.Y };
                RandomizeColor();
            }

            if (transform.Pos.X < 0 && Math.Abs(dir.X - unsignedDir.X) > 0.01f)
            {
                dir = dir with { X = unsignedDir.X };
                RandomizeColor();
            }
            else if (transform.Pos.X + logo.Size.X > displaySize.X && Math.Abs(dir.X - -unsignedDir.X) > 0.01f)
            {
                dir = dir with { X = -unsignedDir.X };
                RandomizeColor();
            }

            pos += dir * Speed * Game.DeltaTime;
            transform.Pos = (displaySize.X * pos.X, displaySize.Y * pos.Y);
        }

        private void RandomizeColor()
        {
            ContentRenderer<Image> imageRenderer = Get<ContentRenderer<Image>>();
            Color newColor = colors.Where(c => c != currentColor).ElementAt(random.Next(0, colors.Length - 1));
            imageRenderer.Content = imageRenderer.Content.WithColorSwapped(currentColor, newColor);
            currentColor = newColor;
        }
    }
}
using Termule.Components;
using Termule.Core;
using Termule.Systems.Display;
using Termule.Systems.ResourceLoader;
using Termule.Types;

internal class Screensaver : Demo
{
    protected override void Start()
    {
        Root.Add(
            new Transform(),
            new Camera(),
            new Logo());
    }

    internal class Logo : GameObject
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

        private readonly Vector unsignedDir = new Vector(1.5f, 1).Normalized;
        private readonly Random random = new();

        private Image logo;
        private Vector pos;
        private Vector dir;
        private Color currentColor;

        internal Logo()
        {
            Add(
                new Transform(),
                new ContentRenderer<Image>() { DisplaySpace = true });

            Registered += OnRegistered;
            Ticked += OnTicked;
        }

        private void OnRegistered()
        {
            logo = new(Systems.Get<ResourceLoader>().Load<Content>("Screensaver/Logo"));
            Get<ContentRenderer<Image>>().Content = logo;

            dir = unsignedDir;
            currentColor = BasicColor.White;
            RandomizeColor();
        }

        private void OnTicked()
        {
            Transform transform = Get<Transform>();
            VectorInt displaySize = Systems.Get<Display>().Size;

            if (transform.Pos.Y < 0 && dir.Y != unsignedDir.Y)
            {
                dir = dir with { Y = unsignedDir.Y };
                RandomizeColor();
            }
            else if (transform.Pos.Y + logo.Size.Y > displaySize.Y && dir.Y != -unsignedDir.Y)
            {
                dir = dir with { Y = -unsignedDir.Y };
                RandomizeColor();
            }

            if (transform.Pos.X < 0 && dir.X != unsignedDir.X)
            {
                dir = dir with { X = unsignedDir.X };
                RandomizeColor();
            }
            else if (transform.Pos.X + logo.Size.X > displaySize.X && dir.X != -unsignedDir.X)
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
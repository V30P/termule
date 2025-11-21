namespace Termule.Rendering;

public class SpriteRenderer : Renderer
{
    private Transform _transform;

    public Image Sprite
    {
        get => _sprite;

        set
        {
            _sprite = value;
            _originOffset = new Vector(-Sprite.Size.X, Sprite.Size.Y) / 2f;
        }
    }
    private Image _sprite;
    private Vector _originOffset;

    public SpriteRenderer()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();
    }

    internal override void Render(Frame frame, Vector viewOrigin)
    {
        // Determine the location of the sprite's origin in frame space
        Vector originWorldPos = _transform.Pos + _originOffset;
        Vector originViewPos = originWorldPos - viewOrigin;
        VectorInt originFramePos = new Vector(originViewPos.X, -originViewPos.Y).RoundToInt();

        // Draw the sprite from its calculated frame space origin
        for (int x = 0; x < Sprite.Size.X; x++)
        {
            for (int y = 0; y < Sprite.Size.Y; y++)
            {
                VectorInt framePos = originFramePos + (x, y);
                if ((uint)framePos.X < frame.Size.X && (uint)framePos.Y < frame.Size.Y)
                {
                    frame.Contribute(framePos, this, Sprite.Colors[x, y], Sprite.Text[x, y]);
                }
            }
        }
    }
}
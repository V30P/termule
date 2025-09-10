namespace Termule.Rendering;

public class SpriteRenderer : Renderer
{
    Transform transform;

    public Image sprite
    {
        get => _sprite;

        set
        {
            _sprite = value;
            originOffset = new Vector(-sprite.size.x, sprite.size.y) / 2f;
        }
    }
    Image _sprite;
    Vector originOffset;

    public SpriteRenderer()
    {
        Rooted += () => transform = gameObject.Get<Transform>();
    }

    internal override void Render(Frame frame, Vector viewOrigin, Vector viewSize)
    {
        // Determine the location of the sprite's origin in frame space
        Vector originWorldPos = transform.pos + originOffset;
        Vector originViewPos = originWorldPos - viewOrigin;
        VectorInt originFramePos = new Vector(originViewPos.x, -originViewPos.y).RoundToInt();

        for (int x = 0; x < sprite.size.x; x++)
        {
            for (int y = 0; y < sprite.size.y; y++)
            {
                VectorInt framePos = originFramePos + (x, y);
                if ((uint) framePos.x < viewSize.x && (uint) framePos.y < viewSize.y)
                {
                    frame.Contribute(sprite.color[x, y], framePos.x, framePos.y, this);
                    frame.Contribute(sprite.text[x, y], framePos.x, framePos.y);
                }
            }
        }
    }
}
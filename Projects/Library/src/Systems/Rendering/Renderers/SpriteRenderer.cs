namespace Termule.Rendering;

public class SpriteRenderer : Renderer
{
    Transform transform;
    public Color[,] sprite;

    public SpriteRenderer()
    {
        Rooted += () => transform = gameObject.Get<Transform>();
    }
    
    internal override void Render(Frame frame, Vector viewOrigin, Vector viewSize)
    {
        // The position of the sprite's upper left corner, relative to the view's origin
        Vector cornerPos = transform.pos + new Vector(-sprite.GetLength(0), sprite.GetLength(1)) / 2f - viewOrigin;

        // The relative pixel coordinates of the sprite's upper left corner
        (int x, int y) cornerPixelPos = ((int) MathF.Round(cornerPos.x), (int) MathF.Round(-cornerPos.y));

        for (int x = 0; x < sprite.GetLength(0); x++)
        {
            for (int y = 0; y < sprite.GetLength(1); y++)
            {
                (int x, int y) pixelPos = (cornerPixelPos.x + x, cornerPixelPos.y + y);
                if ((uint) pixelPos.x < viewSize.x && (uint) pixelPos.y < viewSize.y)
                {
                    frame.Contribute(sprite[x, y], pixelPos.x, pixelPos.y, this);
                }
            }
        }
    }
}
namespace Termule.Rendering;

public class SpriteRenderer : Renderer
{
    Transform transform;

    public Color[,] sprite;

    public SpriteRenderer()
    {
        Rooted += () => transform = gameObject.Get<Transform>();
    }
    
    internal override void RenderTo(Frame frame)
    {
        // The position of the sprite's upper left corner, relative to the frame's upper left bound
        Vector cornerPos = transform.pos + new Vector(-sprite.GetLength(0), sprite.GetLength(1)) / 2f - frame.upperLeftBound;

        // The pixel coordinates of the sprite's upper left corner
        (int x, int y) cornerPixel = ((int) MathF.Round(cornerPos.x), (int) MathF.Round(-cornerPos.y));

        for (int x = 0; x < sprite.GetLength(0); x++)
        {
            for (int y = 0; y < sprite.GetLength(1); y++)
            {
                (int x, int y) pixel = (cornerPixel.x + x, cornerPixel.y + y);
                if ((uint) pixel.x < frame.sizeX && (uint) pixel.y < frame.sizeY)
                {
                    frame[pixel.x, pixel.y] = sprite[x, y];
                }
            }
        }
    }
}
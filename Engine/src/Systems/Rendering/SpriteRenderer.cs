namespace Termule.Rendering;

public class SpriteRenderer : Renderer
{
    [RelativeComponent(Relation.Sibling)]
    readonly Transform transform = null;

    public Color[,] sprite;
    internal override void RenderTo(Frame frame)
    {
        if (sprite != null && transform != null)
        {
            //The position of the sprite's upper left corner, relative to the viewport's upper left bound
            Vector cornerPos = transform.pos + new Vector(-sprite.GetLength(0), sprite.GetLength(1)) / 2f - frame.upperLeftBound;

            //The pixel coordinates of the sprite's upper left corner
            int cornerPixelX = (int) MathF.Round(cornerPos.x);
            int cornerPixelY = (int) MathF.Round(-cornerPos.y);

            for (int x = 0; x < sprite.GetLength(0); x++)
            {
                for (int y = 0; y < sprite.GetLength(1); y++)
                {
                    int pixelX = cornerPixelX + x, pixelY = cornerPixelY + y;
                    if ((uint) pixelX < frame.sizeX && (uint) pixelY < frame.sizeY)
                    {
                        frame.backgroundColor[pixelX, pixelY] = sprite[x, y];
                    }
                }
            }
        }
    }
}
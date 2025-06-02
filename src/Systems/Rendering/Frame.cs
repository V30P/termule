namespace Termule.Rendering;

public class Frame : Image
{
    public readonly Vector upperLeftBound; 

    public Frame() : base(RenderSystem.sizeX, RenderSystem.sizeY)
    {
        upperLeftBound = RenderSystem.viewportCenter + new Vector(-RenderSystem.sizeX, RenderSystem.sizeY) / 2;
    }
}
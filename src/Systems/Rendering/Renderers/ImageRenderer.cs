namespace Termule.Rendering;

public sealed class ImageRenderer : Renderer
{
    private Transform _transform;

    public Image Image;
    public bool RenderInScreenSpace = false;

    public ImageRenderer()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();
    }

    internal override void Render(Frame frame, Vector viewOrigin)
    {
        VectorInt startingPos;
        if (!RenderInScreenSpace)
        {
            startingPos = (_transform.Pos - viewOrigin).RoundToInt(); // Get integer position relative to viewOrigin
            startingPos = new VectorInt(startingPos.X, -startingPos.Y); // Flip y to account for changing from world to screen space
        }
        else
        {
            startingPos = _transform.Pos.RoundToInt();
        }

        for (int x = 0; x < Image?.Size.X; x++)
        {
            for (int y = 0; y < Image.Size.Y; y++)
            {
                VectorInt pos = startingPos + (x, y);
                if ((uint)pos.X < frame.Size.X && (uint)pos.Y < frame.Size.Y)
                {
                    frame.Contribute(pos, this, Image.Color[x, y], Image.Text[x, y], Image.TextColor[x, y]);
                }
            }
        }
    }
}
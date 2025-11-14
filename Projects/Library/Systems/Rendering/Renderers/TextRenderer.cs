namespace Termule.Rendering;

public class TextRenderer : Renderer
{
    private Transform _transform;
    public string Text;

    public TextRenderer()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();
    }

    internal override void Render(Frame frame, Vector _, Vector viewSize)
    {
        VectorInt pixelPos = ((int)_transform.Pos.X, (int)_transform.Pos.Y);
        for (int i = 0; i < Text.Length; i++)
        {
            if (Text[i] == '\n')
            {
                pixelPos = (0, pixelPos.Y + 1);
                continue;
            }
            if (Text[i] == '\r')
            {
                continue;
            }

            if ((uint)pixelPos.X < viewSize.X && (uint)pixelPos.Y < viewSize.Y)
            {
                frame.Contribute(Text[i], pixelPos.X, pixelPos.Y, this);
            }

            pixelPos.X++;
        }
    }
}
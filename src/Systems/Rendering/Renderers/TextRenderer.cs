namespace Termule.Rendering;

public sealed class TextRenderer : Renderer
{
    private Transform _transform;
    public string Text = "";

    public TextRenderer()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();
    }

    internal override void Render(Frame frame, Vector _)
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

            if ((uint)pixelPos.X < frame.Size.X && (uint)pixelPos.Y < frame.Size.Y)
            {
                frame.Contribute(pixelPos, this, Text[i]);
            }

            pixelPos.X++;
        }
    }
}
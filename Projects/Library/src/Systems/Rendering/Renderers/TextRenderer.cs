using System.Diagnostics.Contracts;

namespace Termule.Rendering;

public class TextRenderer : Renderer
{
    Transform transform;
    public string text;

    public TextRenderer()
    {
        Rooted += () => transform = gameObject.Get<Transform>();
    }

    internal override void Render(Frame frame, Vector _, Vector viewSize)
    {
        VectorInt pixelPos = ((int) transform.pos.x, (int) transform.pos.y);
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n' || text[i] == '\r')
            {
                if (text[i] == '\n')
                {
                    pixelPos = (0, pixelPos.y + 1);
                }

                continue;
            }

            if ((uint) pixelPos.x < viewSize.x && (uint) pixelPos.y < viewSize.y)
            {
                frame.Contribute(text[i], pixelPos.x, pixelPos.y, this);
            }

            pixelPos.x++;
        }
    }
}
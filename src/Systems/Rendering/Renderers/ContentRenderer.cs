using System.Reflection;

namespace Termule.Rendering;

public sealed class ContentRenderer<T> : Renderer where T : Content
{
    private Transform _transform;

    public T Content;

    public bool ScreenSpace = false;
    public bool Centered = false;

    public ContentRenderer()
    {
        Rooted += () => _transform = GameObject.Get<Transform>();

        if (typeof(T).GetConstructor(Type.EmptyTypes) is ConstructorInfo paramaterlessConstructor)
        {
            Content = (T)paramaterlessConstructor.Invoke([]);
        }
    }

    internal override void Render(Frame frame, Vector viewOrigin)
    {
        VectorInt pos;
        if (!ScreenSpace)
        {
            pos = (_transform.Pos - viewOrigin).RoundToInt(); // Get integer position relative to viewOrigin
            pos = new VectorInt(pos.X, -pos.Y); // Flip y to account for the change from world to screen space
        }
        else
        {
            pos = _transform.Pos.RoundToInt();
        }

        if (Centered)
        {
            pos -= (((Vector)Content.Size) / 2).RoundToInt();
        }

        for (int x = 0; x < Content?.Size.X; x++)
        {
            for (int y = 0; y < Content.Size.Y; y++)
            {
                Cell cell = Content.At(x, y);
                VectorInt cellPos = pos + (x, y);
                if ((uint)cellPos.X < frame.Size.X && (uint)cellPos.Y < frame.Size.Y)
                {
                    frame.Contribute
                    (
                        this,
                        cellPos,
                        cell.Color != Color.Default ? cell.Color : null,
                        cell.Char != default(char) ? cell.Char : null,
                        cell.CharColor != Color.Default ? cell.CharColor : null
                    );
                }
            }
        }
    }
}
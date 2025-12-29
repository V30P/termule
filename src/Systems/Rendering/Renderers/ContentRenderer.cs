using System.Reflection;

namespace Termule.Rendering;

public sealed class ContentRenderer<T> : TransformRenderer where T : Content
{
    public T Content;
    public bool Centered;

    public ContentRenderer()
    {
        if (typeof(T).GetConstructor(Type.EmptyTypes) is ConstructorInfo paramaterlessConstructor)
        {
            Content = (T)paramaterlessConstructor.Invoke([]);
        }
    }

    private protected override void Render(Frame frame, VectorInt pos)
    {
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
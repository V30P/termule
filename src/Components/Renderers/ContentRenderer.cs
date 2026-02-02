using System.Reflection;
using Termule.Systems.RenderSystem;
using Termule.Types;

namespace Termule.Components;

public sealed class ContentRenderer<TContent> : TransformRenderer where TContent : Content
{
    public TContent Content;
    public bool Centered;

    protected override Vector Offset => Centered && Content != null ? -(Vector)Content.Size / 2 : (0, 0);

    public ContentRenderer()
    {
        if (typeof(TContent).GetConstructor(Type.EmptyTypes) is ConstructorInfo parameterlessConstructor)
        {
            Content = (TContent)parameterlessConstructor.Invoke([]);
        }
    }

    private protected override void Render(Frame frame, VectorInt framespacePos)
    {
        for (int x = 0; x < Content?.Size.X; x++)
        {
            for (int y = 0; y < Content.Size.Y; y++)
            {
                Cell cell = Content.At(x, y);
                VectorInt cellPos = framespacePos + (x, y);
                if ((uint)cellPos.X < frame.Size.X && (uint)cellPos.Y < frame.Size.Y)
                {
                    frame.Contribute
                    (
                        this,
                        cellPos,
                        cell.Color != BasicColor.Default ? cell.Color : null,
                        cell.Char != default(char) ? cell.Char : null,
                        cell.CharColor != BasicColor.Default ? cell.CharColor : null
                    );
                }
            }
        }
    }
}
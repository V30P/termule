namespace Termule.Components;

using System.Reflection;
using Systems.RenderSystem;
using Types;

public sealed class ContentRenderer<TContent> : TransformRenderer
    where TContent : Content
{
    public ContentRenderer()
    {
        if (typeof(TContent).GetConstructor(Type.EmptyTypes) is ConstructorInfo parameterlessConstructor)
        {
            this.Content = (TContent)parameterlessConstructor.Invoke([]);
        }
    }

    public TContent Content { get; set; }

    public bool Centered { get; set; }

    protected override Vector Offset => this.Centered && this.Content != null ? -(Vector)this.Content.Size / 2 : (0, 0);

    private protected override void Render(Frame frame, VectorInt framespacePos)
    {
        for (int x = 0; x < this.Content?.Size.X; x++)
        {
            for (int y = 0; y < this.Content.Size.Y; y++)
            {
                Cell cell = this.Content.At(x, y);
                VectorInt cellPos = framespacePos + (x, y);
                if ((uint)cellPos.X < frame.Size.X && (uint)cellPos.Y < frame.Size.Y)
                {
                    frame.Contribute(
                        this,
                        cellPos,
                        cell.Color != BasicColor.Default ? cell.Color : null,
                        cell.Char != default(char) ? cell.Char : null,
                        cell.CharColor != BasicColor.Default ? cell.CharColor : null);
                }
            }
        }
    }
}
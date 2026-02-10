namespace Termule.Components;

using System.Reflection;
using Systems.RenderSystem;
using Types;

/// <summary>
/// Renders a <see cref="Content"/> instance at the local <see cref="Transform"/>'s position.
/// </summary>
/// <typeparam name="TContent">
/// The type of content to render.
/// An instance will be created automatically if a parameterless constructor exists.
/// </typeparam>
public sealed class ContentRenderer<TContent> : TransformRenderer
    where TContent : Content
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentRenderer{TContent}"/> class.
    /// </summary>
    public ContentRenderer()
    {
        if (typeof(TContent).GetConstructor(Type.EmptyTypes) is ConstructorInfo parameterlessConstructor)
        {
            this.Content = (TContent)parameterlessConstructor.Invoke([]);
        }
    }

    /// <summary>
    /// Gets or sets the <typeparamref name="TContent"/> to render.
    /// </summary>
    public TContent Content { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the content should be rendered centered on its transform.
    /// </summary>
    public bool Centered { get; set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected override Vector Offset => this.Centered && this.Content != null ? -(Vector)this.Content.Size / 2 : (0, 0);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

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
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a <see cref="IContent" /> instance at the local <see cref="Transform" />'s position.
/// </summary>
/// <typeparam name="TContent">
///     The type of content to render.
///     An instance will be created automatically if a parameterless constructor exists.
/// </typeparam>
public sealed class ContentRenderer<TContent> : PositionalRenderer
    where TContent : IContent
{
    /// <summary>
    ///     Gets or sets the <typeparamref name="TContent" /> to render.
    /// </summary>
    public TContent Content { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the content should be rendered centered on its transform.
    /// </summary>
    public bool Centered { get; set; }


    /// <inheritdoc />
    protected override Vector Offset =>
        Centered && Content != null ? -Content.Size / 2 : (0, 0);

    /// <summary>
    ///     Initializes a new instance of the <see cref="ContentRenderer{TContent}" /> class.
    /// </summary>
    public ContentRenderer()
    {
        if (typeof(TContent).GetConstructor(Type.EmptyTypes) is { } parameterlessConstructor)
        {
            Content = (TContent)parameterlessConstructor.Invoke([]);
        }
    }

    private protected override void RenderAtPosition(PositionalRenderContext context)
    {
        for (int x = 0; x < Content?.Size.X; x++)
        {
            for (int y = 0; y < Content.Size.Y; y++)
            {
                Cell cell = Content[x, y];
                VectorInt cellPos = context.Origin + (x, y);
                context.Frame.Draw(
                    cellPos,
                    cell.Color != BasicColor.Default ? cell.Color : null,
                    cell.Char != '\0' ? cell.Char : null,
                    cell.CharColor != BasicColor.Default ? cell.CharColor : null);
            }
        }
    }
}
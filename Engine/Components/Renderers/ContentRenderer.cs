using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Renders a <see cref="IContent" /> instance at the local <see cref="IPositionProvider" />'s
///     position.
/// </summary>
/// <typeparam name="TContent">
///     The type of content to render.
///     An instance will be created automatically if a parameterless constructor exists.
/// </typeparam>
public sealed class ContentRenderer<TContent> : PositionalRenderer where TContent : IContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ContentRenderer{TContent}" /> class.
    /// </summary>
    public ContentRenderer()
    {
        if (typeof(TContent).GetConstructor(Type.EmptyTypes) is { } parameterlessConstructor)
        {
            Content = (TContent) parameterlessConstructor.Invoke([]);
        }
    }

    /// <summary>
    ///     Gets or sets the content to render.
    /// </summary>
    public TContent Content { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the content should be drawn centered.
    /// </summary>
    public bool Centered { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the content should be drawn flipped on the
    ///     x-axis.
    /// </summary>
    public bool FlipX { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the content should be drawn flipped on the
    ///     y-axis.
    /// </summary>
    public bool FlipY { get; set; }

    /// <inheritdoc />
    protected override Vector Offset =>
        Centered && Content != null ? -Content.Size / 2 : (0, 0);

    private protected override void RenderPositionally(IRenderTarget target, Vector _)
    {
        if (Content != null)
        {
            target.DrawContent(
                (0, 0),
                Content,
                flipX: FlipX,
                flipY: (!RenderInTargetSpace) ^ FlipY
            );
        }
    }
}

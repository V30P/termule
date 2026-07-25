using Termule.Engine.Components;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Base system for outputting rendered <see cref="FrameBuffer"/>s to the screen or another
///     display.
/// </summary>
public abstract class DisplaySystem : Core.System, ICameraTarget
{
    internal DisplaySystem()
    {
    }

    /// <summary>
    ///     Gets or sets the size of the display (in cells).
    /// </summary>
    public VectorInt Size
    {
        get;

        protected set
        {
            if (field == value)
            {
                return;
            }

            Buffer = new FrameBuffer(value.X, value.Y);
            Screen = new FrameBuffer(value.X, value.Y);

            field = value;
            Game?.Bus.Broadcast(new ResizedMessage(field));
        }
    }

    /// <summary>
    ///     Gets or sets a cell that will fill the background of the display.
    /// </summary>
    public Cell BackgroundCell { get; set; }

    private protected FrameBuffer Buffer { get; set; } = new FrameBuffer(0, 0);

    private protected FrameBuffer Screen { get; private set; } = new FrameBuffer(0, 0);

    IRenderTarget ICameraTarget.GetRenderTarget()
    {
        Buffer.Fill(BackgroundCell);
        return Buffer;
    }

    void ICameraTarget.Update()
    {
        PrintBuffer();
        (Buffer, Screen) = (Screen, Buffer);
    }

    private protected abstract void PrintBuffer();

    /// <summary>
    ///     Broadcast when the display's size changes.
    /// </summary>
    /// <param name="newSize">The display's size after resizing.</param>
    public readonly struct ResizedMessage(VectorInt newSize)
    {
        /// <summary>
        ///     Gets the updated size of the <see cref="Display"/>.
        /// </summary>
        public VectorInt NewSize { get; } = newSize;
    }

    /// <summary>
    ///     Broadcast when the mouse moves.
    /// </summary>
    /// <param name="newPosition">The position of the mouse after moving.</param>
    public readonly struct MouseMovedMessage(VectorInt newPosition)
    {
        /// <summary>
        ///     Gets the updated position of the mouse.
        /// </summary>
        public VectorInt NewPosition { get; } = newPosition;
    }
}

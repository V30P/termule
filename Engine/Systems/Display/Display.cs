using Termule.Types.Vectors;

namespace Termule.Systems.Display;

/// <summary>
///     Base system for drawing content to the screen or other interface.
/// </summary>
public abstract class Display : Core.System
{
    /// <summary>
    ///     Gets or sets the display-space position of the mouse (in cells).
    /// </summary>
    public VectorInt MousePos { get; protected set; }

    /// <summary>
    ///     Gets the size of the display (in cells).
    /// </summary>
    public VectorInt Size
    {
        get;

        private protected set
        {
            Buffer = new FrameBuffer(value.X, value.Y);
            Screen = new FrameBuffer(value.X, value.Y);

            field = value;
        }
    }

    internal FrameBuffer Buffer { get; private set; } = new(0, 0);

    private protected FrameBuffer Screen { get; private set; } = new(0, 0);

    internal Display()
    {
    }

    internal void Draw()
    {
        DrawBuffer();
        (Buffer, Screen) = (Screen, Buffer);
    }

    private protected abstract void DrawBuffer();
}
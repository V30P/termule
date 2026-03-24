using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Base system for displaying frames on the screen or another output target.
/// </summary>
public abstract class Display : Core.System
{
    /// <summary>
    ///     Gets the display-space position of the mouse (in cells).
    /// </summary>
    public VectorInt MousePos { get; private protected set; }

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
        PrintBuffer();
        (Buffer, Screen) = (Screen, Buffer);
    }

    private protected abstract void PrintBuffer();
}
using Termule.Engine.Components;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Base system for displaying frames on the screen or another output target.
/// </summary>
public abstract class Display : Core.System, ICameraTarget
{
    private protected FrameBuffer Buffer = new(0, 0);

    /// <summary>
    ///     Gets the display-space position of the mouse (in cells).
    /// </summary>
    public VectorInt MousePos { get; private protected set; }

    private protected FrameBuffer Screen { get; private set; } = new(0, 0);

    internal Display()
    {
    }

    /// <summary>
    ///     Gets the size of the display (in cells).
    /// </summary>
    public VectorInt Size
    {
        get;

        protected set
        {
            Buffer = new FrameBuffer(value.X, value.Y);
            Screen = new FrameBuffer(value.X, value.Y);

            field = value;
        }
    }


    FrameBuffer ICameraTarget.Buffer
    {
        get => Buffer;
        set => Buffer = value;
    }

    void ICameraTarget.Update()
    {
        PrintBuffer();
        (Buffer, Screen) = (Screen, Buffer);
    }

    private protected abstract void PrintBuffer();
}
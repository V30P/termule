using Termule.Engine.Components;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Base system for displaying frames on the screen or another output target.
/// </summary>
public abstract class DisplaySystem : Core.System, ICameraTarget
{
    private protected FrameBuffer buffer = new(0, 0);

    /// <summary>
    ///     Gets the display-space position of the mouse (in cells).
    /// </summary>
    public VectorInt MousePos { get; private protected set; }

    private protected FrameBuffer Screen { get; private set; } = new(0, 0);

    internal DisplaySystem()
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
            buffer = new FrameBuffer(value.X, value.Y);
            Screen = new FrameBuffer(value.X, value.Y);

            field = value;
        }
    }


    FrameBuffer ICameraTarget.Buffer
    {
        get => buffer;
        set => buffer = value;
    }

    void ICameraTarget.Update()
    {
        PrintBuffer();
        (buffer, Screen) = (Screen, buffer);
    }

    private protected abstract void PrintBuffer();
}
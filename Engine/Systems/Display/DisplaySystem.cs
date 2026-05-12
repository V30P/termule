using Termule.Engine.Components;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Engine.Systems.Display;

/// <summary>
///     Base system for displaying frames on the screen or another output target.
/// </summary>
public abstract class DisplaySystem : Core.System, ICameraTarget
{
    private protected FrameBuffer buffer = new(0, 0);

    internal DisplaySystem()
    {
    }

    /// <summary>
    ///     Gets the display-space position of the mouse (in cells).
    /// </summary>
    public VectorInt MousePos
    {
        get;

        private protected set
        {
            if (field == value)
            {
                return;
            }

            field = value;
            Game?.Bus.Broadcast(new MouseMovedMessage(field));
        }
    }

    private protected FrameBuffer Screen { get; private set; } = new(0, 0);

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

            buffer = new FrameBuffer(value.X, value.Y);
            Screen = new FrameBuffer(value.X, value.Y);

            field = value;
            Game?.Bus.Broadcast(new ResizedMessage(field));
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

    /// <summary>
    ///     Broadcast when the display's size changes.
    /// </summary>
    /// <param name="NewSize">The display's size after resizing.</param>
    public record struct ResizedMessage(VectorInt NewSize);

    /// <summary>
    ///     Broadcast when the mouse is moved.
    /// </summary>
    /// <param name="NewPosition">The position of the mouse after the movement.</param>
    public record struct MouseMovedMessage(VectorInt NewPosition);
}
namespace Termule.Systems.Display;

using Termule.Components;
using Types;

/// <summary>
/// Base system for drawing content to the screen or other interface.
/// </summary>
public abstract class Display : Core.System
{
    internal Display()
    {
    }

    /// <summary>
    /// Gets or sets the display-space position of the mouse (in cells).
    /// </summary>
    public VectorInt MousePos { get; protected set; }

    /// <summary>
    /// Gets the size of the display (in cells).
    /// </summary>
    public VectorInt Size
    {
        get => field;

        private protected set
        {
            this.Buffer = new FrameBuffer(value.X, value.Y);
            this.Screen = new FrameBuffer(value.X, value.Y);

            field = value;
        }
    }

    internal FrameBuffer Buffer { get; set; } = new FrameBuffer(0, 0);

    private protected FrameBuffer Screen { get; private set; } = new FrameBuffer(0, 0);

    internal void Draw()
    {
        this.DrawBuffer();
        (this.Buffer, this.Screen) = (this.Screen, this.Buffer);
    }

    internal abstract void DrawBuffer();
}
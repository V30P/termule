namespace Termule.Systems.Display;

using Types;

/// <summary>
/// The base class of <see cref="System"/> responsible for drawing content to the screen or other interface.
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
    /// Gets or sets the size of the <see cref="Display"/> (in cells).
    /// </summary>
    public VectorInt Size { get; protected set; }

    internal abstract void Draw(Content content);
}
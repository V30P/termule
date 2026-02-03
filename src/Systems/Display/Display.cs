namespace Termule.Systems.Display;

using Types;

public abstract class Display : Core.System
{
    internal Display()
    {
    }

    public VectorInt MousePos { get; protected set; }

    public VectorInt Size { get; protected set; }

    internal abstract void Draw(Content content);
}
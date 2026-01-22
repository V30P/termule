using Termule.Types;

namespace Termule.Systems.Display;

public abstract class Display : Core.System
{
    public VectorInt MousePos { get; protected set; }
    public VectorInt Size { get; protected set; }

    internal Display() { }

    internal abstract void Draw(Content content);
}
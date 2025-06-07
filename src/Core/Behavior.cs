namespace Termule;

using Termule.Internals;

public abstract partial class Behavior : EngineObject
{
    public Behavior()
    {
        SetUpRelativeComponents();
    }
}
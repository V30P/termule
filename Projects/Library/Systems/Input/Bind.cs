namespace Termule.Input;

public abstract class Bind
{
    internal bool Active
    {
        set
        {
            if (value)
            {
                Hook();
            }
            else
            {
                Unhook();
            }
        }
    }

    protected abstract void Hook();
    internal abstract object GetValue();
    protected abstract void Unhook();
}

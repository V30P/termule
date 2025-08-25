namespace Termule.Input;

public abstract class Control()
{
    internal bool active
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

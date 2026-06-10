using Termule.Engine.Systems.Input;

namespace Termule.Tests.Common;

internal class FakeBind : Bind
{
    public bool GetValueInvoked { get; private set; }

    internal override object GetValue()
    {
        GetValueInvoked = true;
        return true;
    }
}

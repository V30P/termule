using Termule.Engine.Systems.Display;

namespace Termule.Tests.Common;

internal sealed class FakeTerminal : Terminal
{
    private string input;

    internal void SetInput(string value)
    {
        input = value;
    }

    private protected override string CollectInput()
    {
        string temp = input;
        input = string.Empty;

        return temp;
    }
}

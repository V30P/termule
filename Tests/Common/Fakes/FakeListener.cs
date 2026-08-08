using Termule.Engine.Core;

namespace Termule.Tests.Common;

internal sealed class FakeListener<TMessage> : IMessageListener<TMessage>
{
    public TMessage Message { get; private set; }

    public int MessageCount { get; private set; }

    void IMessageListener<TMessage>.OnMessage(TMessage message)
    {
        Message = message;
        MessageCount++;
    }
}

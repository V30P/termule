using Termule.Engine.Core.Messaging;

namespace Termule.Tests.Common;

internal sealed class FakeListener<TMessage> : IMessageListener<TMessage>
{
    public TMessage ReceivedMessage { get; private set; }

    public int MessageCount { get; private set; }

    void IMessageListener<TMessage>.OnMessage(TMessage message)
    {
        ReceivedMessage = message;
        MessageCount++;
    }
}

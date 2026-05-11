using Termule.Engine.Core.Messaging;

namespace Termule.Tests.Core.Messaging;

public class FakeListener<TMessage> : IMessageListener<TMessage>
{
    public TMessage ReceivedMessage { get; private set; }
    public int MessageCount { get; private set; }

    void IMessageListener<TMessage>.OnMessage(TMessage message)
    {
        ReceivedMessage = message;
        MessageCount++;
    }
}
using System.Reflection.Metadata;
using Termule.Engine.Core.Messaging;
using Xunit.Sdk;

namespace Termule.Tests.Core.Messaging;

public class FakeListener<TMessage> : IMessageListener<TMessage>
{
    public TMessage ReceivedMessage { get; private set; }

    void IMessageListener<TMessage>.Receive(TMessage message)
    {
        ReceivedMessage = message;
    }
}
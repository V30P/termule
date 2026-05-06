namespace Termule.Engine.Core.Messaging;

public class MessageBus
{
    public void Subscribe<TMessage>(IMessageListener<TMessage> listener)
    {
        
    }

    public void Unsubscribe<TMessage>(IMessageListener<TMessage> listener)
    {

    }
    
    public void Broadcast(object message)
    {
        
    }
}
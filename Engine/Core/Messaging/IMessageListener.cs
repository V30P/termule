namespace Termule.Engine.Core.Messaging;

public interface IMessageListener<TMessage>
{
    public void Receive(TMessage message);
}
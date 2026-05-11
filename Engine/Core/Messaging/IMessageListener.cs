namespace Termule.Engine.Core.Messaging;

/// <summary>
///     Base interface for types that can subscribe to <see cref="MessageBus" />ses.
/// </summary>
/// <remarks>
///     Do not implement this directly, instead implement <see cref="IMessageListener{TMessage}" />.
/// </remarks>
public interface IMessageListenerBase
{
}

/// <summary>
///     Marks a class as able to subscribe to a type of message from <see cref="MessageBus" />ses.
/// </summary>
/// <typeparam name="TMessage"> The type of message to subscribe to. </typeparam>
public interface IMessageListener<TMessage> : IMessageListenerBase
{
    /// <summary>
    ///     The behavior executed when the message is received.
    /// </summary>
    /// <param name="message"> The received message. </param>
    public void OnMessage(TMessage message);
}
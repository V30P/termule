namespace Termule.Engine.Core.Messaging;

/// <summary>
///     A hub for registering to and sending messages between objects.
/// </summary>
public class MessageBus : GameElement
{
    private readonly Dictionary<Type, HashSet<IMessageListenerBase>> subscribers = [];

    /// <summary>
    ///     REgister the listener to receive messages.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to subscribe to.</typeparam>
    /// <param name="listener">The listener to subscribe.</param>
    public void Subscribe<TMessage>(IMessageListener<TMessage> listener)
    {
        if (!subscribers.ContainsKey(typeof(TMessage)))
        {
            subscribers.Add(typeof(TMessage), []);
        }

        _ = subscribers[typeof(TMessage)].Add(listener);
    }

    /// <summary>
    ///     Unregister the listener from receiving messages.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to unsubscribe from.</typeparam>
    /// <param name="listener">The listener to unsubscribe.</param>
    public void Unsubscribe<TMessage>(IMessageListener<TMessage> listener)
    {
        _ = subscribers[typeof(TMessage)]?.Remove(listener);
    }

    /// <summary>
    ///     Send a message to its subscribed listeners on this bus.
    /// </summary>
    /// <typeparam name="TMessage">The type of message being sent.</typeparam>
    /// <param name="message">The message to send.</param>
    public void Broadcast<TMessage>(TMessage message)
    {
        bool hasSubscribers = subscribers.TryGetValue(
            typeof(TMessage), out HashSet<IMessageListenerBase> typedSubscribers
        );
        if (!hasSubscribers)
        {
            return;
        }

        foreach (IMessageListenerBase listener in typedSubscribers)
        {
            ((IMessageListener<TMessage>) listener).OnMessage(message);
        }
    }
}

using System.Reflection;
using static Termule.Engine.Core.Utilities;

namespace Termule.Engine.Core;

/// <summary>
///     A hub for subscribing to and sending messages between objects.
/// </summary>
public class MessageBus : GameElement
{
    private readonly Dictionary<Type, HashSet<IMessageListenerBase>> subscribers = [];

    internal MessageBus()
    {
    }

    /// <summary>
    ///     Register the listener to receive the specified type of message.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to subscribe to.</typeparam>
    /// <param name="listener">The listener to subscribe.</param>
    public void Subscribe<TMessage>(IMessageListener<TMessage> listener)
    {
        Subscribe(listener, typeof(TMessage));
    }

    /// <summary>
    ///     Register the listener to receive all types of messages that it implements
    ///     <see cref="IMessageListener{TMessage}"/> for.
    /// </summary>
    /// <param name="listener">The listener to subscribe.</param>
    public void SubscribeAll(IMessageListenerBase listener)
    {
        foreach (Type messageType in GetListenerMessageTypes(listener))
        {
            Subscribe(listener, messageType);
        }
    }

    /// <summary>
    ///     Unregister the listener from receiving the specified type of message.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to unsubscribe from.</typeparam>
    /// <param name="listener">The listener to unsubscribe.</param>
    public void Unsubscribe<TMessage>(IMessageListener<TMessage> listener)
    {
        _ = subscribers[typeof(TMessage)]?.Remove(listener);
    }

    /// <summary>
    ///     Unregisters the listener from receiving all types of messages.
    /// </summary>
    /// <param name="listener">The listener to subscribe.</param>
    public void UnsusbcribeAll(IMessageListenerBase listener)
    {
        foreach (Type messageType in GetListenerMessageTypes(listener))
        {
            Unsubscribe(listener, messageType);
        }
    }

    /// <summary>
    ///     Send a message to its subscribed listeners on this bus.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void Broadcast(object message)
    {
        foreach (Type type in GetTypeHierarchy(message))
        {
            bool hasSubscribers = subscribers.TryGetValue(
                type, out HashSet<IMessageListenerBase> typedSubscribers
            );
            if (!hasSubscribers)
            {
                continue;
            }

            Type listenerType = typeof(IMessageListener<>).MakeGenericType(type);
            MethodInfo onMessage = listenerType.GetMethod(nameof(IMessageListener<>.OnMessage));
            foreach (IMessageListenerBase listener in typedSubscribers)
            {
                _ = onMessage.Invoke(listener, [message]);
            }
        }
    }

    private static IEnumerable<Type> GetListenerMessageTypes(IMessageListenerBase listener)
    {
        return listener.GetType().GetInterfaces()
            .Where(
                i => i != typeof(IMessageListenerBase)
                    && i.IsAssignableTo(typeof(IMessageListenerBase))
            )
            .Select(i => i.GetGenericArguments().First());
    }

    private void Subscribe(IMessageListenerBase listener, Type messageType)
    {
        if (!subscribers.TryGetValue(
                messageType, out HashSet<IMessageListenerBase> typedSubscribers
            ))
        {
            typedSubscribers = [];
            subscribers.Add(messageType, typedSubscribers);
        }

        _ = typedSubscribers.Add(listener);
    }

    private void Unsubscribe(IMessageListenerBase listener, Type messageType)
    {
        _ = subscribers[messageType]?.Remove(listener);
    }
}

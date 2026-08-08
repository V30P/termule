namespace Termule.Engine.Core;

/// <summary>
///     A message bus for routing messages through the world.
/// </summary>
public sealed class LocalMessageBus : MessageBus
{
    private readonly GameObject gameObject;

    internal LocalMessageBus(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    /// <summary>
    ///     Send a message to its subscribed listeners along the provided <paramref name="route" />.
    /// </summary>
    /// <typeparam name="TMessage">The type of message being sent.</typeparam>
    /// <param name="message">The message to send.</param>
    /// <param name="route">The path along which the message should be sent.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when trying to send a nonlocal message before the GameObject is activated.
    /// </exception>
    public void Broadcast<TMessage>(TMessage message, Route route)
    {
        if ((int) route > 1 && !Activated)
        {
            throw new InvalidOperationException(
                "Cannot send nonlocal messages before the GameObject is activated."
            );
        }

        if (route.HasFlag(Route.Local))
        {
            Broadcast(message);
        }

        if (route.HasFlag(Route.Upward))
        {
            BroadcastInAncestors(message);
        }

        if (route.HasFlag(Route.Downward))
        {
            BroadcastInDescendants(message);
        }
    }

    private void BroadcastInAncestors<TMessage>(TMessage message)
    {
        for (
            GameObject ancestor = gameObject.GameObject;
            ancestor != null;
            ancestor = ancestor.GameObject)
        {
            ancestor.Bus.Broadcast(message);
        }
    }

    private void BroadcastInDescendants<TMessage>(TMessage message, GameObject target = null)
    {
        target ??= gameObject;

        foreach (GameObject descendant in target.GetAll<GameObject>())
        {
            descendant.Bus.Broadcast(message);
            BroadcastInDescendants(message, descendant);
        }
    }
}

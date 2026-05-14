using Termule.Engine.Core;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Component that stores a position and handles local positioning relative to its parent.
/// </summary>
public sealed class Transform : Component
{
    private readonly List<Transform> children = [];
    private Vector cachedPosition;
    private bool cachedPositionIsLocal = true;

    private Transform parent;

    /// <summary>
    ///     Gets or sets the position of this transform.
    /// </summary>
    public Vector Pos
    {
        get => Activated ? field
            : cachedPositionIsLocal ? (0, 0) : cachedPosition;

        set
        {
            if (value == field)
            {
                return;
            }

            if (Activated)
            {
                Vector difference = value - Pos;
                foreach (Transform child in children)
                {
                    child.Pos += difference;
                }

                field = value;
                GameObject.Bus.Broadcast(new MovedMessage(value));
            }
            else
            {
                cachedPosition = value;
                cachedPositionIsLocal = false;
            }
        }
    }

    /// <summary>
    ///     Gets or sets the position of this transform relative to its parent.
    /// </summary>
    public Vector LocalPos
    {
        get => Activated ? Pos - (parent?.Pos ?? (0, 0))
            : cachedPositionIsLocal ? cachedPosition : (0, 0);

        set
        {
            if (Activated)
            {
                Pos = (parent?.Pos ?? (0, 0)) + value;
            }
            else
            {
                cachedPosition = value;
                cachedPositionIsLocal = true;
            }
        }
    }

    /// <inheritdoc />
    protected override void Activate()
    {
        parent = GameObject.GameObject?.Get<Transform>();
        parent?.children.Add(this);

        Pos = cachedPositionIsLocal ? (parent?.Pos ?? (0, 0)) + cachedPosition : cachedPosition;
    }

    /// <inheritdoc />
    protected override void Deactivate()
    {
        // Cache the current position so it can be restored when re-activated.
        cachedPosition = Pos;
        cachedPositionIsLocal = false;

        _ = parent?.children.Remove(this);
        parent = null;
    }

    /// <summary>
    ///     Broadcast when the local <see cref="Transform"/> is moved.
    /// </summary>
    /// <param name="newPosition">The transform's position after the move is applied.</param>
    public readonly struct MovedMessage(Vector newPosition)
    {
        internal readonly Vector NewPosition = newPosition;
    }
}

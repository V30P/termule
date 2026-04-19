using Termule.Engine.Core;
using Termule.Engine.Types.Vectors;

namespace Termule.Engine.Components;

/// <summary>
///     Component that stores a position and handles local positioning relative to parent.
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
        get => IsRegistered ? field
            : cachedPositionIsLocal ? (0, 0) : cachedPosition;

        set
        {
            if (IsRegistered)
            {
                Vector difference = value - Pos;
                foreach (Transform child in children)
                {
                    child.Pos += difference;
                }

                field = value;
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
        get => IsRegistered ? Pos - (parent?.Pos ?? (0, 0))
            : cachedPositionIsLocal ? cachedPosition : (0, 0);

        set
        {
            if (IsRegistered)
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

    /// <summary>
    ///     Initializes a new instance of the <see cref="Transform" /> class.
    /// </summary>
    public Transform()
    {
        Registered += OnRegistered;
        Unregistered += OnUnregistered;
    }

    private void OnRegistered()
    {
        parent = GameObject.GameObject?.Get<Transform>();
        parent?.children.Add(this);

        Pos = cachedPositionIsLocal ? (parent?.Pos ?? (0, 0)) + cachedPosition : cachedPosition;
    }

    private void OnUnregistered()
    {
        // Cache the current position so it can be restored when re-registered.
        cachedPosition = Pos;
        cachedPositionIsLocal = false;

        parent?.children.Remove(this);
        parent = null;
    }
}
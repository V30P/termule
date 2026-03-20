using Termule.Core;
using Termule.Types.Vectors;

namespace Termule.Components;

/// <summary>
///     Component that stores a position and handles local positioning relative to parent.
/// </summary>
public sealed class Transform : Component
{
    private Vector cachedPosition;
    private bool cachedPositionIsLocal = true;
    
    private List<Transform> children = [];
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
                var difference = value - Pos;
                foreach (var child in children)
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
        Registered += ApplyPositioning;
    }

    private void ApplyPositioning()
    {
        parent = GameObject.GameObject?.Get<Transform>();
        Pos = cachedPositionIsLocal ? (parent?.Pos ?? (0, 0)) + cachedPosition : cachedPosition;

        children = [];
        foreach (var component in GameObject)
        {
            if (component is GameObject componentGameObject &&
                componentGameObject.Get<Transform>() is { } childTransform)
            {
                children.Add(childTransform);
            }
        }
    }
}
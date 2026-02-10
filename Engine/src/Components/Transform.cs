namespace Termule.Components;

using Core;
using Types;

/// <summary>
/// Component that stores a position and handles local positioning relative to parent.
/// </summary>
public sealed class Transform : Component
{
    private Transform parent;
    private List<Transform> children = [];

    private Vector cachedPosition;
    private bool cachedPositionIsLocal = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="Transform"/> class.
    /// </summary>
    public Transform()
    {
        this.Registered += this.ApplyPositioning;
    }

    /// <summary>
    /// Gets or sets the position of this transform.
    /// </summary>
    public Vector Pos
    {
        get => this.IsRegistered ? field
            : (this.cachedPositionIsLocal ? (0, 0) : this.cachedPosition);

        set
        {
            if (this.IsRegistered)
            {
                Vector difference = value - this.Pos;
                foreach (Transform child in this.children)
                {
                    child.Pos += difference;
                }

                field = value;
            }
            else
            {
                this.cachedPosition = value;
                this.cachedPositionIsLocal = false;
            }
        }
    }

    /// <summary>
    /// Gets or sets the position of this transform relative to its parent.
    /// </summary>
    public Vector LocalPos
    {
        get => this.IsRegistered ? this.Pos - (this.parent?.Pos ?? (0, 0))
            : (this.cachedPositionIsLocal ? this.cachedPosition : (0, 0));

        set
        {
            if (this.IsRegistered)
            {
                this.Pos = (this.parent?.Pos ?? (0, 0)) + value;
            }
            else
            {
                this.cachedPosition = value;
                this.cachedPositionIsLocal = true;
            }
        }
    }

    private void ApplyPositioning()
    {
        this.parent = this.GameObject.GameObject?.Get<Transform>();
        this.Pos = this.cachedPositionIsLocal ? (this.parent?.Pos ?? (0, 0)) + this.cachedPosition : this.cachedPosition;

        this.children = [];
        foreach (Component component in this.GameObject)
        {
            if (component is GameObject componentGameObject && componentGameObject.Get<Transform>() is Transform childTransform)
            {
                this.children.Add(childTransform);
            }
        }
    }
}
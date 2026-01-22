using Termule.Core;
using Termule.Types;

namespace Termule.Components;

public sealed class Transform : Component
{
    private bool _hasBeenRooted = false;
    private Transform _parent;
    private List<Transform> _children = [];

    public Vector Pos
    {
        get => _hasBeenRooted ? field
            : (_cachedPositionIsLocal ? (0, 0) : _cachedPosition);

        set
        {
            if (_hasBeenRooted)
            {
                Vector difference = value - Pos;
                foreach (Transform child in _children)
                {
                    child.Pos += difference;
                }

                field = value;
            }
            else
            {
                _cachedPosition = value;
                _cachedPositionIsLocal = false;
            }
        }
    }

    public Vector LocalPos
    {
        get => _hasBeenRooted ? Pos - (_parent?.Pos ?? (0, 0))
            : (_cachedPositionIsLocal ? _cachedPosition : (0, 0));

        set
        {
            if (_hasBeenRooted)
            {
                Pos = (_parent?.Pos ?? (0, 0)) + value;
            }
            else
            {
                _cachedPosition = value;
                _cachedPositionIsLocal = true;
            }
        }
    }

    private Vector _cachedPosition;
    private bool _cachedPositionIsLocal = true;

    public Transform()
    {
        Registered += ApplyPositioning;
    }

    private void ApplyPositioning()
    {
        _hasBeenRooted = true;

        _parent = GameObject.GameObject?.Get<Transform>();
        Pos = _cachedPositionIsLocal ? (_parent?.Pos ?? (0, 0)) + _cachedPosition : _cachedPosition;

        _children = [];
        foreach (Component component in GameObject)
        {
            if (component is GameObject componentGameObject && componentGameObject.Get<Transform>() is Transform childTransform)
            {
                _children.Add(childTransform);
            }
        }
    }
}
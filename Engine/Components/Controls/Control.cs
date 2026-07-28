using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;
using Termule.Engine.Systems.Input;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Component that provides a value based on messages from a <see cref="TerminalController"/>.
/// </summary>
/// <typeparam name="TValue">The type of value this control produces.</typeparam>
public class Control<TValue> : Component, IMessageListener<InputMessage>
{
    internal Control()
    {
    }

    /// <summary>
    ///     Gets this control's value.
    /// </summary>
    public TValue Value { get; private protected set; }

    void IMessageListener<InputMessage>.OnMessage(InputMessage message)
    {
        switch (message)
        {
            case ButtonPressed pressed:
                OnButtonPressed(pressed.Button);
                break;
            case HoldStarted holdStarted:
                OnHoldStarted(holdStarted.Button);
                break;
            case HoldStopped holdStopped:
                OnHoldStopped(holdStopped.Button);
                break;
            case CharTyped charTyped:
                OnCharTyped(charTyped.Char);
                break;
            case MouseMoved mouseMoved:
                OnMouseMoved(mouseMoved.Pos);
                break;
            default:
                break;
        }
    }

    /// <inheritdoc/>
    protected override void Activate()
    {
        Game.Bus.Subscribe(this);
    }

    private protected virtual void OnButtonPressed(Button button)
    {
    }

    private protected virtual void OnHoldStarted(Button button)
    {
    }

    private protected virtual void OnHoldStopped(Button button)
    {
    }

    private protected virtual void OnCharTyped(char character)
    {
    }

    private protected virtual void OnMouseMoved(VectorInt pos)
    {
    }
}

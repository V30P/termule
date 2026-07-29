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
            case ButtonPressed buttonPressed:
                OnButtonPressed(buttonPressed.Button);
                break;
            case ButtonDown buttonDown:
                OnButtonDown(buttonDown.Button);
                break;
            case ButtonUp buttonUp:
                OnButtonUp(buttonUp.Button);
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

    private protected virtual void OnButtonDown(Button button)
    {
    }

    private protected virtual void OnButtonUp(Button button)
    {
    }

    private protected virtual void OnCharTyped(char character)
    {
    }

    private protected virtual void OnMouseMoved(VectorInt pos)
    {
    }
}

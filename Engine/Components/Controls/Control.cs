using Termule.Engine.Core;
using Termule.Engine.Systems.Input;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Component that provides a value based on messages from a <see cref="TerminalController"/>.
/// </summary>
/// <typeparam name="TValue">The type of value this control produces.</typeparam>
public class Control<TValue> : Component,
    IMessageListener<ButtonDown>,
    IMessageListener<ButtonUp>,
    IMessageListener<CharTyped>,
    IMessageListener<MouseMoved>
{
    internal Control()
    {
    }

    /// <summary>
    ///     Gets this control's value.
    /// </summary>
    public TValue Value { get; private protected set; }

    void IMessageListener<ButtonDown>.OnMessage(ButtonDown message)
    {
        OnButtonDown(message.Button);
    }

    void IMessageListener<ButtonUp>.OnMessage(ButtonUp message)
    {
        OnButtonUp(message.Button);
    }

    void IMessageListener<CharTyped>.OnMessage(CharTyped message)
    {
        OnCharTyped(message.Char);
    }

    void IMessageListener<MouseMoved>.OnMessage(MouseMoved message)
    {
        OnMouseMoved(message.Pos);
    }

    /// <inheritdoc/>
    protected override void Activate()
    {
        Game.Bus.SubscribeAll(this);
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

using Termule.Engine.Core;
using Termule.Engine.Core.Messaging;
using Termule.Engine.Systems.Input;
using Termule.Engine.Types;

namespace Termule.Engine.Components;

/// <summary>
///     Component that provides a value based on messages from a <see cref="TerminalController"/>.
/// </summary>
/// <typeparam name="TValue">The type of value this control produces.</typeparam>
public class Control<TValue> : Component,
    IMessageListener<ButtonPressed>,
    IMessageListener<HoldStarted>,
    IMessageListener<HoldStopped>,
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

    void IMessageListener<ButtonPressed>.OnMessage(ButtonPressed message)
    {
        OnButtonPressed(message.Button);
    }

    void IMessageListener<HoldStarted>.OnMessage(HoldStarted message)
    {
        OnHoldStarted(message.Button);
    }

    void IMessageListener<HoldStopped>.OnMessage(HoldStopped message)
    {
        OnHoldStopped(message.Button);
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
        Game.Bus.Subscribe<ButtonPressed>(this);
        Game.Bus.Subscribe<HoldStarted>(this);
        Game.Bus.Subscribe<HoldStopped>(this);
        Game.Bus.Subscribe<CharTyped>(this);
        Game.Bus.Subscribe<MouseMoved>(this);
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

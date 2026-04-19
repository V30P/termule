namespace Termule.Engine.Systems.Input;

/// <summary>
///     Base class for a <see cref="Keyboard"/> bind.
/// </summary>
public abstract class Bind
{
    internal Keyboard Keyboard
    {

        set
        {
            if (field != null)
            {
                field.ButtonDown -= OnButtonDown;
                field.ButtonUp -= OnButtonUp;
                field.CharacterTyped -= OnCharacterTyped;
            }

            field = value;

            if (field != null)
            {
                field.ButtonDown += OnButtonDown;
                field.ButtonUp += OnButtonUp;
                field.CharacterTyped += OnCharacterTyped;
            }
        }
    }

    internal Bind()
    {
    }

    internal abstract object GetValue();

    /// <summary>
    ///     Invoked when a button is first pressed.
    /// </summary>
    /// <param name="button">The button that was pressed.</param>
    protected virtual void OnButtonDown(Button button)
    {
    }

    /// <summary>
    ///     Invoked when a button is first released.
    /// </summary>
    /// <param name="button">The button that was released.</param>
    protected virtual void OnButtonUp(Button button)
    {
    }

    /// <summary>
    ///     Invoked when a valid character is produced from keyboard input.
    /// </summary>
    /// <param name="character">The produced character.</param>
    protected virtual void OnCharacterTyped(char character)
    {
    }
}
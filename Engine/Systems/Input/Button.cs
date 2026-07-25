namespace Termule.Engine.Systems.Input;

/// <summary>
///     A physical button on a keyboard or mouse.
/// </summary>
public enum Button
{
    // Control characters
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Backspace,   // \b (8) / sometimes DEL (127)
    Tab,         // \t
    Enter,       // \r or \n
    Escape,      // 27
    Space,

    // Digits
    D0,
    D1,
    D2,
    D3,
    D4,
    D5,
    D6,
    D7,
    D8,
    D9,

    // Letters
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z,

    // Symbols
    Exclamation,     // !
    DoubleQuote,     // "
    Hash,            // #
    Dollar,          // $
    Percent,         // %
    Ampersand,       // &
    Apostrophe,      // '
    LeftParen,       // (
    RightParen,      // )
    Asterisk,        // *
    Plus,            // +
    Comma,           // ,
    Minus,           // -
    Period,          // .
    Slash,           // /

    Colon,           // :
    Semicolon,       // ;
    LessThan,        // <
    Equals,          // =
    GreaterThan,     // >
    Question,        // ?
    At,              // @

    LeftBracket,     // [
    Backslash,       // \
    RightBracket,    // ]
    Caret,           // ^
    Underscore,      // _
    Grave,           // `

    LeftBrace,       // {
    Pipe,            // |
    RightBrace,      // }
    Tilde,           // ~

    LeftMouse,
    MiddleMouse,
    RightMouse,
    MouseWheelUp,
    MouseWheelDown,
    MouseWheelLeft,
    MouseWheelRight
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

internal static class ButtonConversions
{
    private static readonly Dictionary<char, Button> CharToButton = new()
    {
        ['\b'] = Button.Backspace,
        ['\t'] = Button.Tab,
        ['\r'] = Button.Enter,
        ['\n'] = Button.Enter,
        [(char) 27] = Button.Escape,
        [' '] = Button.Space,

        ['0'] = Button.D0,
        ['1'] = Button.D1,
        ['2'] = Button.D2,
        ['3'] = Button.D3,
        ['4'] = Button.D4,
        ['5'] = Button.D5,
        ['6'] = Button.D6,
        ['7'] = Button.D7,
        ['8'] = Button.D8,
        ['9'] = Button.D9,

        ['a'] = Button.A,
        ['b'] = Button.B,
        ['c'] = Button.C,
        ['d'] = Button.D,
        ['e'] = Button.E,
        ['f'] = Button.F,
        ['g'] = Button.G,
        ['h'] = Button.H,
        ['i'] = Button.I,
        ['j'] = Button.J,
        ['k'] = Button.K,
        ['l'] = Button.L,
        ['m'] = Button.M,
        ['n'] = Button.N,
        ['o'] = Button.O,
        ['p'] = Button.P,
        ['q'] = Button.Q,
        ['r'] = Button.R,
        ['s'] = Button.S,
        ['t'] = Button.T,
        ['u'] = Button.U,
        ['v'] = Button.V,
        ['w'] = Button.W,
        ['x'] = Button.X,
        ['y'] = Button.Y,
        ['z'] = Button.Z,

        ['A'] = Button.A,
        ['B'] = Button.B,
        ['C'] = Button.C,
        ['D'] = Button.D,
        ['E'] = Button.E,
        ['F'] = Button.F,
        ['G'] = Button.G,
        ['H'] = Button.H,
        ['I'] = Button.I,
        ['J'] = Button.J,
        ['K'] = Button.K,
        ['L'] = Button.L,
        ['M'] = Button.M,
        ['N'] = Button.N,
        ['O'] = Button.O,
        ['P'] = Button.P,
        ['Q'] = Button.Q,
        ['R'] = Button.R,
        ['S'] = Button.S,
        ['T'] = Button.T,
        ['U'] = Button.U,
        ['V'] = Button.V,
        ['W'] = Button.W,
        ['X'] = Button.X,
        ['Y'] = Button.Y,
        ['Z'] = Button.Z,

        ['!'] = Button.Exclamation,
        ['"'] = Button.DoubleQuote,
        ['#'] = Button.Hash,
        ['$'] = Button.Dollar,
        ['%'] = Button.Percent,
        ['&'] = Button.Ampersand,
        ['\''] = Button.Apostrophe,
        ['('] = Button.LeftParen,
        [')'] = Button.RightParen,
        ['*'] = Button.Asterisk,
        ['+'] = Button.Plus,
        [','] = Button.Comma,
        ['-'] = Button.Minus,
        ['.'] = Button.Period,
        ['/'] = Button.Slash,
        [':'] = Button.Colon,
        [';'] = Button.Semicolon,
        ['<'] = Button.LessThan,
        ['='] = Button.Equals,
        ['>'] = Button.GreaterThan,
        ['?'] = Button.Question,
        ['@'] = Button.At,
        ['['] = Button.LeftBracket,
        ['\\'] = Button.Backslash,
        [']'] = Button.RightBracket,
        ['^'] = Button.Caret,
        ['_'] = Button.Underscore,
        ['`'] = Button.Grave,
        ['{'] = Button.LeftBrace,
        ['|'] = Button.Pipe,
        ['}'] = Button.RightBrace,
        ['~'] = Button.Tilde,
    };

    internal static bool TryConvertCharToButton(char character, out Button button)
    {
        return CharToButton.TryGetValue(character, out button);
    }

    internal static Button MouseButtonIndexToButton(int index)
    {
        return index switch
        {
            0 => Button.LeftMouse,
            1 => Button.MiddleMouse,
            2 => Button.RightMouse,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
    }

    internal static Button MouseWheelIndexToButton(int index)
    {
        return index switch
        {
            0 => Button.MouseWheelUp,
            1 => Button.MouseWheelDown,
            2 => Button.MouseWheelLeft,
            3 => Button.MouseWheelRight,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };
    }
}

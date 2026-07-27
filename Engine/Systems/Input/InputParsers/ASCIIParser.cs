namespace Termule.Engine.Systems.Input;

internal abstract class ASCIIParser : InputParser
{
    private static readonly Dictionary<int, Button> BaseCharToButton = new()
    {
        ['\b'] = Button.Backspace,
        ['\x7F'] = Button.Backspace,
        ['\t'] = Button.Tab,
        ['\r'] = Button.Enter,
        ['\n'] = Button.Enter,
        ['\e'] = Button.Escape,
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

        ['`'] = Button.Grave,
        ['-'] = Button.Minus,
        ['='] = Button.Equals,
        ['['] = Button.LeftBracket,
        [']'] = Button.RightBracket,
        ['\\'] = Button.Backslash,
        [';'] = Button.Semicolon,
        ['\''] = Button.Apostrophe,
        [','] = Button.Comma,
        ['.'] = Button.Period,
        ['/'] = Button.Slash,
    };

    private static readonly Dictionary<int, Button> ShiftedCharToButton = new()
    {
        ['!'] = Button.D1,
        ['@'] = Button.D2,
        ['#'] = Button.D3,
        ['$'] = Button.D4,
        ['%'] = Button.D5,
        ['^'] = Button.D6,
        ['&'] = Button.D7,
        ['*'] = Button.D8,
        ['('] = Button.D9,
        [')'] = Button.D0,

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

        ['~'] = Button.Grave,
        ['_'] = Button.Minus,
        ['+'] = Button.Equals,
        ['{'] = Button.LeftBracket,
        ['}'] = Button.RightBracket,
        ['|'] = Button.Backslash,
        [':'] = Button.Semicolon,
        ['"'] = Button.Apostrophe,
        ['<'] = Button.Comma,
        ['>'] = Button.Period,
        ['?'] = Button.Slash
    };

    protected static bool TryConvertASCIIToButton(int codepoint, out Button button)
    {
        return BaseCharToButton.TryGetValue(codepoint, out button)
            || ShiftedCharToButton.TryGetValue(codepoint, out button);
    }
}

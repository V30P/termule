namespace Termule.Engine.Systems.Input;

internal sealed partial class CharacterParser : InputParser
{
    private static readonly Dictionary<char, Button> CharToButton = new()
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

    internal override IEnumerable<InputMessage> Parse(string input)
    {
        foreach (char character in input)
        {
            yield return new CharTyped(character);

            if (CharToButton.TryGetValue(character, out Button key))
            {
                yield return new ButtonPressed(key);
            }
        }

        Remainder = string.Empty;
    }
}

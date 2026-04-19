using SharpHook.Data;

namespace Termule.Engine.Systems.Input;

internal static class ButtonConversions
{
    private static readonly Dictionary<MouseButton, Button> MouseButtonToButton = new()
    {
        { MouseButton.Button1, Button.Mouse1 },
        { MouseButton.Button2, Button.Mouse2 },
        { MouseButton.Button3, Button.Mouse3 },
        { MouseButton.Button4, Button.Mouse4 },
        { MouseButton.Button5, Button.Mouse5 }
    };

#pragma warning disable SA1509 // Opening braces should not be preceded by blank line
    private static readonly Dictionary<KeyCode, Button> KeyCodeToButton = new()
    {
        { KeyCode.VcA, Button.A }, { KeyCode.VcB, Button.B }, { KeyCode.VcC, Button.C },
        { KeyCode.VcD, Button.D }, { KeyCode.VcE, Button.E }, { KeyCode.VcF, Button.F },
        { KeyCode.VcG, Button.G }, { KeyCode.VcH, Button.H }, { KeyCode.VcI, Button.I },
        { KeyCode.VcJ, Button.J }, { KeyCode.VcK, Button.K }, { KeyCode.VcL, Button.L },
        { KeyCode.VcM, Button.M }, { KeyCode.VcN, Button.N }, { KeyCode.VcO, Button.O },
        { KeyCode.VcP, Button.P }, { KeyCode.VcQ, Button.Q }, { KeyCode.VcR, Button.R },
        { KeyCode.VcS, Button.S }, { KeyCode.VcT, Button.T }, { KeyCode.VcU, Button.U },
        { KeyCode.VcV, Button.V }, { KeyCode.VcW, Button.W }, { KeyCode.VcX, Button.X },
        { KeyCode.VcY, Button.Y }, { KeyCode.VcZ, Button.Z },

        { KeyCode.Vc0, Button._0 }, { KeyCode.Vc1, Button._1 }, { KeyCode.Vc2, Button._2 },
        { KeyCode.Vc3, Button._3 }, { KeyCode.Vc4, Button._4 }, { KeyCode.Vc5, Button._5 },
        { KeyCode.Vc6, Button._6 }, { KeyCode.Vc7, Button._7 }, { KeyCode.Vc8, Button._8 },
        { KeyCode.Vc9, Button._9 },

        { KeyCode.VcF1, Button.F1 }, { KeyCode.VcF2, Button.F2 }, { KeyCode.VcF3, Button.F3 },
        { KeyCode.VcF4, Button.F4 }, { KeyCode.VcF5, Button.F5 }, { KeyCode.VcF6, Button.F6 },
        { KeyCode.VcF7, Button.F7 }, { KeyCode.VcF8, Button.F8 }, { KeyCode.VcF9, Button.F9 },
        { KeyCode.VcF10, Button.F10 }, { KeyCode.VcF11, Button.F11 }, { KeyCode.VcF12, Button.F12 },
        { KeyCode.VcF13, Button.F13 }, { KeyCode.VcF14, Button.F14 }, { KeyCode.VcF15, Button.F15 },
        { KeyCode.VcF16, Button.F16 }, { KeyCode.VcF17, Button.F17 }, { KeyCode.VcF18, Button.F18 },
        { KeyCode.VcF19, Button.F19 }, { KeyCode.VcF20, Button.F20 }, { KeyCode.VcF21, Button.F21 },
        { KeyCode.VcF22, Button.F22 }, { KeyCode.VcF23, Button.F23 }, { KeyCode.VcF24, Button.F24 },

        { KeyCode.VcLeftShift, Button.LeftShift }, { KeyCode.VcRightShift, Button.RightShift },
        { KeyCode.VcLeftControl, Button.LeftControl }, { KeyCode.VcRightControl, Button.RightControl },
        { KeyCode.VcLeftAlt, Button.LeftAlt }, { KeyCode.VcRightAlt, Button.RightAlt },
        { KeyCode.VcLeftMeta, Button.LeftMeta }, { KeyCode.VcRightMeta, Button.RightMeta },

        { KeyCode.VcCapsLock, Button.CapsLock },
        { KeyCode.VcNumLock, Button.NumLock },
        { KeyCode.VcScrollLock, Button.ScrollLock },

        { KeyCode.VcBackspace, Button.Backspace },
        { KeyCode.VcTab, Button.Tab },
        { KeyCode.VcEnter, Button.Enter },
        { KeyCode.VcEscape, Button.Escape },
        { KeyCode.VcInsert, Button.Insert },
        { KeyCode.VcDelete, Button.Delete },
        { KeyCode.VcHome, Button.Home },
        { KeyCode.VcEnd, Button.End },
        { KeyCode.VcPageUp, Button.PageUp },
        { KeyCode.VcPageDown, Button.PageDown },
        { KeyCode.VcPrintScreen, Button.PrintScreen },
        { KeyCode.VcPause, Button.Pause },
        { KeyCode.VcContextMenu, Button.ContextMenu },
        { KeyCode.VcLeft, Button.LeftArrow },
        { KeyCode.VcRight, Button.RightArrow },
        { KeyCode.VcUp, Button.UpArrow },
        { KeyCode.VcDown, Button.DownArrow },
        { KeyCode.VcSpace, Button.Space },
        { KeyCode.VcMinus, Button.Minus },
        { KeyCode.VcEquals, Button.Equals },
        { KeyCode.VcOpenBracket, Button.OpenBracket },
        { KeyCode.VcCloseBracket, Button.CloseBracket },
        { KeyCode.VcBackslash, Button.Backslash },
        { KeyCode.VcSemicolon, Button.Semicolon },
        { KeyCode.VcQuote, Button.Quote },
        { KeyCode.VcComma, Button.Comma },
        { KeyCode.VcPeriod, Button.Period },
        { KeyCode.VcSlash, Button.Slash },
        { KeyCode.VcBackQuote, Button.BackQuote },
        { KeyCode.VcNumPad0, Button.Np0 }, { KeyCode.VcNumPad1, Button.Np1 },
        { KeyCode.VcNumPad2, Button.Np2 }, { KeyCode.VcNumPad3, Button.Np3 },
        { KeyCode.VcNumPad4, Button.Np4 }, { KeyCode.VcNumPad5, Button.Np5 },
        { KeyCode.VcNumPad6, Button.Np6 }, { KeyCode.VcNumPad7, Button.Np7 },
        { KeyCode.VcNumPad8, Button.Np8 }, { KeyCode.VcNumPad9, Button.Np9 },
        { KeyCode.VcNumPadEnter, Button.NpEnter },
        { KeyCode.VcNumPadAdd, Button.NpAdd },
        { KeyCode.VcNumPadSubtract, Button.NpSubtract },
        { KeyCode.VcNumPadMultiply, Button.NpMultiply },
        { KeyCode.VcNumPadDivide, Button.NpDivide },
        { KeyCode.VcNumPadDecimal, Button.NpDecimal },
        { KeyCode.VcNumPadEquals, Button.NpEquals },

        { KeyCode.VcVolumeMute, Button.VolumeMute },
        { KeyCode.VcVolumeUp, Button.VolumeUp },
        { KeyCode.VcVolumeDown, Button.VolumeDown },
        { KeyCode.VcMediaPlay, Button.MediaPlay },
        { KeyCode.VcMediaStop, Button.MediaStop },
        { KeyCode.VcMediaNext, Button.MediaNext },
        { KeyCode.VcMediaPrevious, Button.MediaPrevious },
        { KeyCode.VcMediaSelect, Button.MediaSelect }
    };
#pragma warning restore SA1509 // Opening braces should not be preceded by blank line

    internal static Button? ToButton(this MouseButton mouseButton)
    {
        return MouseButtonToButton.TryGetValue(mouseButton, out Button button) ? button : null;
    }

    internal static Button? ToButton(this KeyCode keyCode)
    {
        return KeyCodeToButton.TryGetValue(keyCode, out Button button) ? button : null;
    }
}
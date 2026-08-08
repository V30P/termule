using Termule.Engine.Core;
using Termule.Engine.Systems.Input;
using Termule.Tests.Common;

namespace Termule.Tests.Systems.Input;

public class TestTerminalController
{
    private static List<InputMessage> GetInputMessagesForSequence(string sequence)
    {
        Game game = new();

        FakeTerminal terminal = new();
        game.Systems.Install(
            terminal,
            new TerminalController()
        );

        InputMessageSpy spy = new();
        game.Bus.Subscribe(spy);

        game.Start();
        terminal.SetInput(sequence);
        game.RunTick();

        return spy.Messages;
    }

    public class TestSGRParsing
    {
        public static readonly TheoryData<string, int, int> MovementData = new()
        {
            { "\e[<35;2;3M", 1, 2 },
            { "\e[<35;5;6M", 4, 5 },
            { "\e[<35;4;4M", 3, 3 },
        };

        public static readonly TheoryData<string, Button, bool> ButtonData = new()
        {
            { "\e[<0;0;0M", Button.MouseLeft, true },
            { "\e[<1;0;0M", Button.MouseMiddle, true },
            { "\e[<2;0;0M", Button.MouseRight, true },
        };

        public static readonly TheoryData<string, Button> WheelData = new()
        {
            { "\e[<64;0;0M", Button.MouseWheelUp },
            { "\e[<65;0;0m", Button.MouseWheelDown },
            { "\e[<66;0;0M", Button.MouseWheelLeft },
            { "\e[<67;0;0m", Button.MouseWheelRight },
        };

        [Theory]
        [MemberData(nameof(MovementData))]
        public void Tick_GivenMovementSGR_BroadcastsMouseMoved(
            string sequence,
            int expectedX,
            int expectedY)
        {
            List<InputMessage> messages = GetInputMessagesForSequence(sequence);

            InputMessage message = Assert.Single(messages);
            MouseMoved mouseMoved = Assert.IsType<MouseMoved>(message);
            Assert.Equal(mouseMoved.Pos, (expectedX, expectedY));
        }

        [Theory]
        [MemberData(nameof(ButtonData))]
        public void Tick_GivenButtonDownSGR_BroadcastButtonDown(
            string sequence,
            Button expectedButton,
            bool expectedStateIsDown)
        {
            List<InputMessage> messages = GetInputMessagesForSequence(sequence);

            InputMessage message = Assert.Single(messages);
            ButtonMessage buttonMessage = expectedStateIsDown ? Assert.IsType<ButtonDown>(message)
                : Assert.IsType<ButtonUp>(message);

            Assert.Equal(buttonMessage.Button, expectedButton);
        }

        [Fact]
        public void Tick_GivenMovementAndButtonDownSGR_BroadcastsProperMessages()
        {
            List<InputMessage> messages = GetInputMessagesForSequence("\e[<34;2;3M");

            Assert.Equal(2, messages.Count);

            MouseMoved mouseMoved = Assert.IsType<MouseMoved>(messages[0]);
            Assert.Equal(mouseMoved.Pos, (1, 2));
            ButtonDown buttonDown = Assert.IsType<ButtonDown>(messages[1]);
            Assert.Equal(Button.MouseRight, buttonDown.Button);
        }

        [Fact]
        public void Tick_GivenButtonUpSGRWithoutPriorButtonDown_BroadcastsNothing()
        {
            List<InputMessage> messages = GetInputMessagesForSequence("\e[<0;0;0m");

            Assert.Empty(messages);
        }

        [Fact]
        public void Tick_GivenButtonUpSGRWithPriorButtonDown_BroadcastsButtonUp()
        {
            List<InputMessage> messages = GetInputMessagesForSequence("\e[<1;0;0M\e[<1;0;0m");

            Assert.Equal(2, messages.Count);
            ButtonUp buttonUp = Assert.IsType<ButtonUp>(messages[1]);
            Assert.Equal(Button.MouseMiddle, buttonUp.Button);
        }

        [Theory]
        [MemberData(nameof(WheelData))]
        public void Tick_GivenWheelSGR_BroadcastsProperMessages(
            string sequence,
            Button expectedButton)
        {
            List<InputMessage> messages = GetInputMessagesForSequence(sequence);

            Assert.Equal(2, messages.Count);
            ButtonDown buttonDown = Assert.IsType<ButtonDown>(messages[0]);
            Assert.Equal(expectedButton, buttonDown.Button);
            ButtonUp buttonUp = Assert.IsType<ButtonUp>(messages[1]);
            Assert.Equal(expectedButton, buttonUp.Button);
        }
    }

    public class TestSS3Parsing
    {
        public static readonly TheoryData<string, Button> SS3Data = new()
        {
            { "\eOA", Button.Up },
            { "\eOB", Button.Down },
            { "\eOC", Button.Right },
            { "\eOD", Button.Left },
            { "\eOH", Button.Home },
            { "\eOF", Button.End },
            { "\eOP", Button.F1 },
            { "\eOQ", Button.F2 },
            { "\eOR", Button.F3 },
            { "\eOS", Button.F4 }
        };

        [Theory]
        [MemberData(nameof(SS3Data))]
        public void Tick_GivenSS3Sequence_BroadcastProperMessages(
            string sequence,
            Button expectedButton)
        {
            List<InputMessage> messages = GetInputMessagesForSequence(sequence);

            Assert.Equal(2, messages.Count);
            ButtonDown buttonDown = Assert.IsType<ButtonDown>(messages[0]);
            Assert.Equal(expectedButton, buttonDown.Button);
            ButtonUp buttonUp = Assert.IsType<ButtonUp>(messages[1]);
            Assert.Equal(expectedButton, buttonUp.Button);
        }
    }

    public class TestCSIParsing
    {
        public static readonly TheoryData<string, Button> WithoutKittyButtonDownData = new()
        {
            { "\e[A", Button.Up },
            { "\e[1;2A", Button.Up },
            { "\e[1;5B", Button.Down },
            { "\e[1;3C", Button.Right },
            { "\e[1;6D", Button.Left },
            { "\e[1;7H", Button.Home },
            { "\e[1;8F", Button.End },
            { "\e[1~", Button.Home },
            { "\e[1;2~", Button.Home },
            { "\e[2~", Button.Insert },
            { "\e[2;5~", Button.Insert },
            { "\e[3~", Button.Delete },
            { "\e[3;6~", Button.Delete },
            { "\e[4;3~", Button.End },
            { "\e[5~", Button.PageUp },
            { "\e[5;2~", Button.PageUp },
            { "\e[6;5~", Button.PageDown },
            { "\e[11~", Button.F1 },
            { "\e[12;2~", Button.F2 },
            { "\e[13;5~", Button.F3 },
            { "\e[15~", Button.F5 },
            { "\e[17;2~", Button.F6 },
            { "\e[18;5~", Button.F7 },
            { "\e[20;3~", Button.F9 },
            { "\e[24;5~", Button.F12 },
        };

        public static readonly TheoryData<string, Button, char?> WithKittyButtonDownData = new()
        {
            { "\e[97;1;97u", Button.A, 'a' },
            { "\e[98;2;98u", Button.B, 'b' },
            { "\e[49;1;49u", Button.D1, '1' },
            { "\e[32;5;32u", Button.Space, ' ' },
            { "\e[13;1;13u", Button.Enter, '\r' },
            { "\e[9;1;9u", Button.Tab, '\t' },
            { "\e[57344;1u", Button.Escape, null },
            { "\e[57345;1;13u", Button.Enter, '\r' },
            { "\e[57347;1u", Button.Backspace, null },
            { "\e[57350;1u", Button.Left, null },
            { "\e[57353;5u", Button.Down, null },
            { "\e[57356;1u", Button.Home, null },
            { "\e[57364;1u", Button.F1, null },
            { "\e[57375;2u", Button.F12, null },
            { "\e[57399;1;48u", Button.Keypad0, '0' },
            { "\e[57404;1;53u", Button.Keypad5, '5' },
            { "\e[57414;1;13u", Button.KeypadEnter, '\r' },
            { "\e[57428;1u", Button.MediaPlay, null },
            { "\e[57440;1u", Button.MuteVolume, null },
            { "\e[57441;1u", Button.LeftShift, null },
            { "\e[57442;5u", Button.LeftControl, null },
            { "\e[57449;1u", Button.RightAlt, null },
            { "\e[A", Button.Up, null },
            { "\e[1;5B", Button.Down, null },
            { "\e[C", Button.Right, null },
            { "\e[1;2D", Button.Left, null },
            { "\e[P", Button.F1, null },
            { "\e[R", Button.F3, null },
            { "\e[1~", Button.Home, null },
            { "\e[3;5~", Button.Delete, null },
            { "\e[5~", Button.PageUp, null },
            { "\e[6;2~", Button.PageDown, null },
            { "\e[17~", Button.F6, null },
            { "\e[24;3~", Button.F12, null },
        };

        [Theory]
        [MemberData(nameof(WithoutKittyButtonDownData))]
        public void Tick_GivenCSISequenceWithoutKittyAvaliable_BroadcastsProperMessages(
            string sequence,
            Button expectedButton)
        {
            List<InputMessage> messages = GetInputMessagesForSequence(sequence);

            Assert.Equal(2, messages.Count);
            ButtonDown buttonDown = Assert.IsType<ButtonDown>(messages[0]);
            Assert.Equal(expectedButton, buttonDown.Button);
            ButtonUp buttonUp = Assert.IsType<ButtonUp>(messages[1]);
            Assert.Equal(expectedButton, buttonUp.Button);
        }

        [Theory]
        [MemberData(nameof(WithKittyButtonDownData))]
        public void Tick_GivenKittyButtonDownSequence_BroadcastsProperMessages(
            string sequence,
            Button expectedButton,
            char? expectedTypedChar)
        {
            List<InputMessage> messages = GetInputMessagesForKittySequence(sequence);

            ButtonDown buttonDown;
            if (expectedTypedChar is char character)
            {
                Assert.Equal(2, messages.Count);
                Assert.Equal(character, Assert.IsType<CharTyped>(messages[1]).Char);
            }
            else
            {
                _ = Assert.Single(messages);
            }

            buttonDown = Assert.IsType<ButtonDown>(messages[0]);
            Assert.Equal(expectedButton, buttonDown.Button);
        }

        [Fact]
        public void Tick_GivenKittyButtonUpSequenceWithoutPriorButtonDown_BroadcastsNothing()
        {
            List<InputMessage> messages = GetInputMessagesForKittySequence("\e[97;1:3;97u");

            Assert.Empty(messages);
        }

        [Fact]
        public void Tick_GivenKittyButtonUpSequenceWithPriorButtonDown_BroadcastsButtonUp()
        {
            List<InputMessage> messages = GetInputMessagesForKittySequence(
                "\e[49;2:1u\e[49;2:3;33u"
            );

            ButtonUp buttonUp = Assert.IsType<ButtonUp>(messages[1]);
            Assert.Equal(Button.D1, buttonUp.Button);
        }

        private static List<InputMessage> GetInputMessagesForKittySequence(string sequence)
        {
            return GetInputMessagesForSequence(
                "\e[?31u" // Fake Kitty status response
                + sequence
            );
        }
    }

    public class TestASCIIParsing
    {
        public static readonly TheoryData<char, Button> ASCIIData = new()
        {
            { '0', Button.D0 },
            { '1', Button.D1 },
            { '2', Button.D2 },
            { '5', Button.D5 },
            { '9', Button.D9 },
            { '!', Button.D1 },
            { '@', Button.D2 },
            { '#', Button.D3 },
            { '$', Button.D4 },
            { '%', Button.D5 },
            { '^', Button.D6 },
            { '&', Button.D7 },
            { '*', Button.D8 },
            { '(', Button.D9 },
            { ')', Button.D0 },
            { 'a', Button.A },
            { 'b', Button.B },
            { 'm', Button.M },
            { 'x', Button.X },
            { 'z', Button.Z },
            { 'A', Button.A },
            { 'B', Button.B },
            { 'M', Button.M },
            { 'X', Button.X },
            { 'Z', Button.Z },
            { ' ', Button.Space },
            { '\t', Button.Tab },
            { '\r', Button.Enter },
            { '\b', Button.Backspace },
            { '\e', Button.Escape },
        };

        [Theory]
        [MemberData(nameof(ASCIIData))]
        public void Tick_GivenASCIICharacter_BroadcastsProperMessages(
            char character,
            Button expectedButton)
        {
            List<InputMessage> messages = GetInputMessagesForSequence(character.ToString());

            Assert.Equal(3, messages.Count);
            ButtonDown buttonDown = Assert.IsType<ButtonDown>(messages[0]);
            Assert.Equal(expectedButton, buttonDown.Button);
            ButtonUp buttonUp = Assert.IsType<ButtonUp>(messages[1]);
            Assert.Equal(expectedButton, buttonUp.Button);
            CharTyped charTyped = Assert.IsType<CharTyped>(messages[2]);
            Assert.Equal(character, charTyped.Char);
        }
    }

    private sealed class InputMessageSpy : IMessageListener<InputMessage>
    {
        internal List<InputMessage> Messages { get; } = [];

        public void OnMessage(InputMessage message)
        {
            Messages.Add(message);
        }
    }
}

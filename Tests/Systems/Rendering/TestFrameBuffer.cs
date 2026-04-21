using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class TestFrameBuffer
{
    private static readonly Cell TestCell = new(BasicColor.White, 'X', BasicColor.White);

    // Prevents XUnit from failing to convert BasicColor -> Color? on its own
    private static readonly Color DrawColor = BasicColor.White;
    public static IEnumerable<object[]> DrawData =
    [
        [null, null, null, default(Cell)],
        [DrawColor, null, null, new Cell(DrawColor)],
        [null, 'X', null, new Cell(default, 'X')],
        [null, null, DrawColor, new Cell(default, '\0', DrawColor)],
        [DrawColor, 'X', DrawColor, new Cell(DrawColor, 'X', DrawColor)]
    ];

    public static IEnumerable<object[]> DrawCoverData = [];

    private static void AssertAllCellsEqual(FrameBuffer frame, Cell expectedCell)
    {
        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                Assert.Equal(expectedCell, frame[x, y]);
            }
        }
    }

    [Theory]
    [MemberData(nameof(DrawData))]
    public void Draw_AppliesProvidedValues(Color? color, char? character, Color? characterColor, Cell expectedCell)
    {
        FrameBuffer frame = new(1, 1);

        frame.Draw((0, 0), color, character, characterColor);

        Assert.Equal(expectedCell, frame[0, 0]);
    }

    [Fact]
    public void Draw_IgnoresOutOfBoundsPositions()
    {
        FrameBuffer frame = new(10, 5);
        frame.Draw((-1, 0), BasicColor.White);
        frame.Draw((0, -1), BasicColor.White);
        frame.Draw((10, 0), BasicColor.White);
        frame.Draw((0, 5), BasicColor.White);

        AssertAllCellsEqual(frame, default);
    }

    [Fact]
    public void Draw_ProperlyCoversExistingValues()
    {
        FrameBuffer frame = new(1, 1);
        frame.Draw((0, 0), TestCell.Color, TestCell.Char, TestCell.CharColor);

        frame.Draw((0, 0), null, TestCell.Char);
        Assert.Equal(new Cell(TestCell.Color, TestCell.Char), frame[0, 0]);

        frame.Draw((0, 0), TestCell.Color);
        Assert.Equal(new Cell(TestCell.Color), frame[0, 0]);
    }

    [Fact]
    public void Reset_WithNoCellProvided_FillsWithDefaultCell()
    {
        FrameBuffer frame = new(10, 5);
        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                frame.Draw((x, y), TestCell.Color, TestCell.Char, TestCell.CharColor);
            }
        }

        frame.Reset();

        AssertAllCellsEqual(frame, default);
    }

    [Fact]
    public void Reset_WithCellProvided_FillsWithProvidedCell()
    {
        FrameBuffer frame = new(10, 5);

        frame.Reset(TestCell);

        AssertAllCellsEqual(frame, TestCell);
    }
}
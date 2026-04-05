using Termule.Engine.Systems.Display;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Display;

public class TestFrameBuffer
{
    private static readonly Color TestColor = BasicColor.White;
    private static readonly Cell TestCell = new(TestColor, 'X', TestColor);

    public static IEnumerable<object?[]> DrawData =
    [
        [null, null, null, default(Cell)],
        [TestColor, null, null, new Cell(TestColor)],
        [null, 'X', null, new Cell(default, 'X')],
        [null, null, TestColor, new Cell(default, '\0', TestColor)],
        [TestColor, 'X', TestColor, new Cell(TestColor, 'X', TestColor)]
    ];

    public static IEnumerable<object?[]> DrawCoverData = [];

    private static void AssertAllCellsEqual(FrameBuffer frame, Cell expectedCell)
    {
        for (int x = 0; x < frame.Size.X; x++)
        for (int y = 0; y < frame.Size.Y; y++)
        {
            Assert.Equal(expectedCell, frame[x, y]);
        }
    }

    [Theory]
    [MemberData(nameof(DrawData))]
    public void Draw_ShouldApplyValues(Color? color, char? character, Color? characterColor, Cell expectedCell)
    {
        FrameBuffer frame = new(1, 1);

        frame.Draw((0, 0), color, character, characterColor);

        Assert.Equal(expectedCell, frame[0, 0]);
    }

    [Fact]
    public void Draw_ShouldIgnoreOutOfBoundsPositions()
    {
        FrameBuffer frame = new(10, 5);
        frame.Draw((-1, 0), TestColor);
        frame.Draw((0, -1), TestColor);
        frame.Draw((10, 0), TestColor);
        frame.Draw((0, 5), TestColor);

        AssertAllCellsEqual(frame, default);
    }

    [Fact]
    public void Draw_ShouldProperlyCoverExistingValues()
    {
        FrameBuffer frame = new(1, 1);
        frame.Draw((0, 0), TestCell.Color, TestCell.Char, TestCell.CharColor);

        frame.Draw((0, 0), null, TestCell.Char);
        Assert.Equal(new Cell(TestCell.Color, TestCell.Char), frame[0, 0]);

        frame.Draw((0, 0), TestCell.Color);
        Assert.Equal(new Cell(TestCell.Color), frame[0, 0]);
    }

    [Fact]
    public void Reset_ShouldFillWithDefaultCell_WhenNoCellProvided()
    {
        FrameBuffer frame = new(10, 5);
        for (int x = 0; x < frame.Size.X; x++)
        for (int y = 0; y < frame.Size.Y; y++)
        {
            frame.Draw((x, y), TestCell.Color, TestCell.Char, TestCell.CharColor);
        }

        frame.Reset();

        AssertAllCellsEqual(frame, default);
    }

    [Fact]
    public void Reset_ShouldFillWithProvidedCell()
    {
        FrameBuffer frame = new(10, 5);

        frame.Reset(TestCell);

        AssertAllCellsEqual(frame, TestCell);
    }
}
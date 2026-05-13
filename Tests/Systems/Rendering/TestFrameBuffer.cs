using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class TestFrameBuffer
{
    // Prevents XUnit from failing to convert BasicColor to Color? at runtime
    private static readonly Cell TestCell = new(BasicColor.White, 'X', BasicColor.White);

    private static readonly Color DrawColor = BasicColor.White;

    public static IEnumerable<object[]> DrawData =>
    [
        [null, null, null, default(Cell)],
        [DrawColor, null, null, new Cell(DrawColor)],
        [null, 'X', null, new Cell(default, 'X')],
        [null, null, DrawColor, new Cell(default, '\0', DrawColor)],
        [DrawColor, 'X', DrawColor, new Cell(DrawColor, 'X', DrawColor)]
    ];

    [Theory]
    [MemberData(nameof(DrawData))]
    public void Draw_AppliesProvidedValues(
        Color? color,
        char? character,
        Color? characterColor,
        Cell expectedCell)
    {
        FrameBuffer frame = new(1, 1);

        frame.Draw((0, 0), color, character, characterColor);

        Assert.Equal(expectedCell, frame[0, 0]);
    }

    [Fact]
    public void Draw_DoesNotLayerNonMatchingBoxDrawingChars()
    {
        FrameBuffer frame = new(1, 1);

        frame.Draw((0, 0), character: '─', characterColor: BasicColor.White);
        frame.Draw((0, 0), character: '│', characterColor: BasicColor.Red);

        Assert.Equal('│', frame[0, 0].Character);
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
    public void Draw_LayersMatchingBoxDrawingChars()
    {
        FrameBuffer frame = new(1, 1);

        frame.Draw((0, 0), character: '─', characterColor: BasicColor.White);
        frame.Draw((0, 0), character: '│', characterColor: BasicColor.White);

        Assert.Equal('┼', frame[0, 0].Character);
    }

    [Fact]
    public void Draw_ProperlyCoversExistingValues()
    {
        FrameBuffer frame = new(1, 1);
        frame.Draw((0, 0), TestCell.Color, TestCell.Character, TestCell.CharColor);

        frame.Draw((0, 0), null, TestCell.Character);
        Assert.Equal(new Cell(TestCell.Color, TestCell.Character), frame[0, 0]);

        frame.Draw((0, 0), TestCell.Color);
        Assert.Equal(new Cell(TestCell.Color), frame[0, 0]);
    }

    [Fact]
    public void Draw_WithLayerBoxDrawingCharactersFalse_DoesNotLayerBoxDrawingChars()
    {
        FrameBuffer frame = new(1, 1);

        frame.Draw((0, 0), character: '─', characterColor: BasicColor.White);
        frame.Draw(
            (0, 0),
            character: '│',
            characterColor: BasicColor.White,
            layerBoxDrawingChars: false
        );

        Assert.Equal('│', frame[0, 0].Character);
    }

    [Fact]
    public void Reset_WithCellProvided_FillsWithProvidedCell()
    {
        FrameBuffer frame = new(10, 5);

        frame.Reset(TestCell);

        AssertAllCellsEqual(frame, TestCell);
    }

    [Fact]
    public void Reset_WithNoCellProvided_FillsWithDefaultCell()
    {
        FrameBuffer frame = new(10, 5);
        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                frame.Draw((x, y), TestCell.Color, TestCell.Character, TestCell.CharColor);
            }
        }

        frame.Reset();

        AssertAllCellsEqual(frame, default);
    }

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
}

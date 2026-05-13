using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class TestFrameBuffer
{
    public static readonly TheoryData<BasicColor?, char?, BasicColor?> DrawData = new()
    {
        { null, null, null },
        { BasicColor.White, null, null },
        { null, 'X', null },
        { null, null, BasicColor.White },
        { BasicColor.White, 'X', BasicColor.White }
    };

    [Theory]
    [MemberData(nameof(DrawData))]
    public void Draw_AppliesProvidedValues(
        BasicColor? color,
        char? character,
        BasicColor? characterColor)
    {
        FrameBuffer frame = new(1, 1);
        Cell expectedCell = new(
            color ?? default,
            character ?? '\0',
            characterColor ?? default);

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
        frame.Draw((0, 0), BasicColor.White, 'X', BasicColor.White);

        frame.Draw((0, 0), null, 'X');
        Assert.Equal(new Cell(BasicColor.White, 'X'), frame[0, 0]);

        frame.Draw((0, 0), BasicColor.White);
        Assert.Equal(new Cell(BasicColor.White), frame[0, 0]);
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

        frame.Reset(new(BasicColor.White, 'X', BasicColor.White));

        AssertAllCellsEqual(frame, new(BasicColor.White, 'X', BasicColor.White));
    }

    [Fact]
    public void Reset_WithNoCellProvided_FillsWithDefaultCell()
    {
        FrameBuffer frame = new(10, 5);
        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                frame.Draw((x, y), BasicColor.White, 'X', BasicColor.White);
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

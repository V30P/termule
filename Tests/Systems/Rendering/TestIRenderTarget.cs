using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class TestIRenderTarget
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
        char? glyph,
        BasicColor? glyphColor)
    {
        IRenderTarget target = new FakeRenderTarget(1, 1);
        Cell expectedCell = new(
            color ?? default,
            glyph ?? '\0',
            glyphColor ?? default);

        target.Draw((0, 0), color, glyph, glyphColor);

        Assert.Equal(expectedCell, target.GetCellRef(0, 0));
    }

    [Fact]
    public void Draw_DoesNotLayerNonMatchingBoxDrawingChars()
    {
        IRenderTarget target = new FakeRenderTarget(1, 1);

        target.Draw((0, 0), glyph: '─', glyphColor: BasicColor.White);
        target.Draw((0, 0), glyph: '│', glyphColor: BasicColor.Red);

        Assert.Equal('│', target.GetCellRef(0, 0).Glyph);
    }

    [Fact]
    public void Draw_IgnoresOutOfBoundsPositions()
    {
        IRenderTarget target = new FakeRenderTarget(10, 5);
        target.Draw((-1, 0), BasicColor.White);
        target.Draw((0, -1), BasicColor.White);
        target.Draw((10, 0), BasicColor.White);
        target.Draw((0, 5), BasicColor.White);

        for (int x = 0; x < target.Size.X; x++)
        {
            for (int y = 0; y < target.Size.Y; y++)
            {
                Assert.Equal(default, target.GetCellRef(x, y));
            }
        }
    }

    [Fact]
    public void Draw_LayersMatchingBoxDrawingChars()
    {
        IRenderTarget target = new FakeRenderTarget(1, 1);

        target.Draw((0, 0), glyph: '─', glyphColor: BasicColor.White);
        target.Draw((0, 0), glyph: '│', glyphColor: BasicColor.White);

        Assert.Equal('┼', target.GetCellRef(0, 0).Glyph);
    }

    [Fact]
    public void Draw_ProperlyCoversExistingValues()
    {
        IRenderTarget target = new FakeRenderTarget(1, 1);
        target.Draw((0, 0), BasicColor.White, 'X', BasicColor.White);

        target.Draw((0, 0), null, 'X');
        Assert.Equal(new Cell(BasicColor.White, 'X'), target.GetCellRef(0, 0));

        target.Draw((0, 0), BasicColor.White);
        Assert.Equal(new Cell(BasicColor.White), target.GetCellRef(0, 0));
    }

    [Fact]
    public void Draw_WithLayerBoxDrawingGlyphsFalse_DoesNotLayerBoxDrawingGlyphs()
    {
        IRenderTarget target = new FakeRenderTarget(1, 1);

        target.Draw((0, 0), glyph: '─', glyphColor: BasicColor.White);
        target.Draw(
            (0, 0),
            glyph: '│',
            glyphColor: BasicColor.White,
            layerBoxDrawingChars: false
        );

        Assert.Equal('│', target.GetCellRef(0, 0).Glyph);
    }

    private sealed class FakeRenderTarget(int width, int height) : IRenderTarget
    {
        private readonly Cell[,] cells = new Cell[width, height];

        VectorInt IRenderTarget.LowerBound => (0, 0);

        VectorInt IRenderTarget.UpperBound => (width, height);

        ref Cell IRenderTarget.GetCellRef(int x, int y)
        {
            return ref cells[x, y];
        }
    }
}

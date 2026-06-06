using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;
using Termule.Tests.Systems.Rendering;

namespace Termule.Tests.Components;

public class TestContentRenderer
{
    public static readonly TheoryData<float[], float[], int[]> ViewOriginData = new()
    {
        { [1f, 1f], [0f, 0f], [1, 1] },
        { [1f, 1f], [2f, 2f], [1, 1] },
        { [2.25f, 1.75f], [1f, 1f], [2, 2] }
    };

    [Fact]
    public void Content_WhenTypeHasParameterlessConstructor_ShouldBeInitialized()
    {
        ContentRenderer<ParameterlessContent> contentRenderer = new();

        Assert.NotNull(contentRenderer.Content);
    }

    [Fact]
    public void Content_WhenTypeLacksParameterlessConstructor_ShouldNotBeInitialized()
    {
        ContentRenderer<NonParameterlessContent> contentRenderer = new();

        Assert.Null(contentRenderer.Content);
    }

    [Fact]
    public void Render_WithNullContent_DoesNotThrow()
    {
        ContentRenderer<IContent> contentRenderer = new();
        _ = new GameObject(new Transform(), contentRenderer);

        contentRenderer.Render(new FrameBuffer(0, 0), (0, 0));
    }

    [Fact]
    public void Render_WhenCenteredIsTrue_Offsets()
    {
        Cell[,] contentCells = new Cell[2, 3];
        for (int x = 0; x < contentCells.GetLength(0); x++)
        {
            for (int y = 0; y < contentCells.GetLength(1); y++)
            {
                contentCells[x, y] = new(BasicColor.White);
            }
        }

        ContentRenderer<IContent> contentRenderer = new()
        {
            RenderInTargetSpace = true,
            Centered = true,
            Content = new FakeContent(contentCells)
        };
        _ = new GameObject(new Transform() { Pos = (1, 1.5f) }, contentRenderer);

        IRenderTarget target = new FakeRenderTarget(2, 3);
        contentRenderer.Render(target, default);

        for (int x = target.LowerBound.X; x < target.UpperBound.X; x++)
        {
            for (int y = target.LowerBound.Y; y < target.UpperBound.X; y++)
            {
                Assert.Equal(BasicColor.White, target.GetCellRef(x, y).Color);
            }
        }
    }

    private sealed class ParameterlessContent() : IContent
    {
        VectorInt IContent.Size => throw new NotImplementedException();

        Cell IContent.this[int x, int y] => throw new NotImplementedException();
    }

#pragma warning disable CS9113 // Parameter is unread.
    private sealed class NonParameterlessContent(object _) : IContent
#pragma warning restore CS9113 // Parameter is unread.
    {
        VectorInt IContent.Size => throw new NotImplementedException();

        Cell IContent.this[int x, int y] => throw new NotImplementedException();
    }

    private sealed class FakeContent(Cell[,] cells) : IContent
    {
        VectorInt IContent.Size => (cells.GetLength(0), cells.GetLength(1));

        Cell IContent.this[int x, int y] => cells[x, y];
    }
}

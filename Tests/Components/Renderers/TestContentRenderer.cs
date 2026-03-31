using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types;
using static Termule.Tests.Components.Utilities;

namespace Termule.Tests.Components;

public class TestContentRenderer
{
    public static IEnumerable<object[]> ViewOriginData =>
    [
        [(1f, 1f), (0f, 0f), (1, 1)],
        [(1f, 1f), (2f, 2f), (1, 1)],
        [(2.25f, 1.75f), (1f, 1f), (2, 2)]
    ];
    
    // ReSharper disable once ClassNeverInstantiated.Local
    private class ParameterlessContent() : Content(0, 0)
    {
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class NonParameterlessContent(int width, int height) : Content(width, height)
    {
    }

    private class FakeContent : Content
    {
        public FakeContent(Cell[,] cells) : base(cells.GetLength(0), cells.GetLength(1))
        {
            Cells = cells;
        }
    }

    [Fact]
    public void ContentTypeWithParameterlessConstructor_ShouldBeInitialized()
    {
        ContentRenderer<ParameterlessContent> renderer = new();
        
        Assert.NotNull(renderer.Content);
    }

    [Fact]
    public void ContentTypeWithoutParameterlessConstructor_ShouldNotBeInitialized()
    {
        ContentRenderer<NonParameterlessContent> renderer = new();

        Assert.Null(renderer.Content);
    }
    
    [Fact]
    public void Render_ShouldDrawExpectedCells()
    {
        ContentRenderer<Content> renderer = new()
        {
            TargetSpace = true,
            Content = new FakeContent(new Cell[,]
                { { new(BasicColor.White), new(BasicColor.Red) }, { new(BasicColor.Red), new(BasicColor.White) } })
        };
        _ = new GameObject(new Transform { Pos = (1, 1) }, renderer);
        FrameBuffer frame = new(4, 4);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, [(1, 1), (2, 2)]);
        AssertDrawnCells(frame, BasicColor.Red, [(2, 1), (1, 2)]);
    }

    [Fact]
    public void Render_ShouldNotContributeDefaultValues()
    {
        Cell baseCell = new(BasicColor.Red, 'X', BasicColor.Red);
        ContentRenderer<Content> baseRenderer = new()
        {
            TargetSpace = true,
            Content = new FakeContent(new Cell[,] { { baseCell } })
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, baseRenderer);
        FrameBuffer frame = new(1, 1);
        
        baseRenderer.Render(frame, (0, 0));
        
        ContentRenderer<Content> defaultRenderer = new()
        {
            TargetSpace = true,
            Content = new FakeContent(new Cell[,] { { new() } })
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, defaultRenderer);
        
        defaultRenderer.Render(frame, (0, 0));
        
        Assert.Equal(baseCell, frame[0, 0]);
    }

    [Fact]
    public void Render_WithNullContent_ShouldNotMutateFrame()
    {
        Cell baseCell = new(BasicColor.Red, 'X', BasicColor.Red);
        ContentRenderer<Content> baseRenderer = new()
        {
            TargetSpace = true,
            Content = new FakeContent(new Cell[,] { { baseCell } })
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, baseRenderer);
        FrameBuffer frame = new(1, 1);

        baseRenderer.Render(frame, (0, 0));

        ContentRenderer<Content> nullRenderer = new()
        {
            TargetSpace = true,
            Content = null!
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, nullRenderer);

        nullRenderer.Render(frame, (0, 0));

        Assert.Equal(baseCell, frame[0, 0]);
    }

    [Theory]
    [MemberData(nameof(ViewOriginData))]
    public void Render_ShouldRespectViewOrigin(Vector transformPos, Vector viewOrigin, VectorInt expectedCellPos)
    {
        ContentRenderer<Content> renderer = new()
        {
            TargetSpace = true,
            Content = new FakeContent(new Cell[,] { { new (BasicColor.Red) } })
        };
        _ = new GameObject(new Transform { Pos = transformPos }, renderer);
        FrameBuffer frame = new(3, 3);
        
        renderer.Render(frame, viewOrigin);

        AssertDrawnCells(frame, BasicColor.Red, [
            expectedCellPos
        ]);
    }
    
    [Fact]
    public void Centered_ShouldOffsetContent()
    {
        Cell cell = new(BasicColor.Red);
        ContentRenderer<Content> renderer = new()
        {
            TargetSpace = true,
            Content = new FakeContent(new[,] { { default, cell, default },  { cell, cell, cell }, { default, cell, default }}),
            Centered = true
        };
        _ = new GameObject(new Transform { Pos = (1, 1) }, renderer);
        FrameBuffer frame = new(3, 3);
        
        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.Red, [
            (1, 0),
            (0, 1), (1, 1), (2, 1),
            (1, 2)
        ]);
    }

    [Fact]
    public void Render_WhenNotInTargetSpace_ShouldApplyViewOriginAndFlipY()
    {
        ContentRenderer<Content> renderer = new()
        {
            TargetSpace = false,
            Content = new FakeContent(new Cell[,] { { new(BasicColor.Red) } })
        };
        _ = new GameObject(new Transform { Pos = (2, 0) }, renderer);
        FrameBuffer frame = new(3, 3);

        renderer.Render(frame, (1, 1));

        AssertDrawnCells(frame, BasicColor.Red, [(1, 1)]);
    }
}

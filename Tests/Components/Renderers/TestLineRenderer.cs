using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;
using Termule.Tests.Common;

namespace Termule.Tests.Components;

public class TestLineRenderer
{
    [Fact]
    public void Render_DrawsPolylineSegments()
    {
        IRenderTarget target = new FakeRenderTarget(4, 4);
        LineRenderer renderer = new()
        {
            RenderInTargetSpace = true,
            Color = BasicColor.White,
            Points = [(0, 0), (2, 0), (2, 2)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(target, (0, 0));

        target.AssertDrawnColor(BasicColor.White, [(0, 0), (1, 0), (2, 0), (2, 1), (2, 2)]);
    }

    [Fact]
    public void Render_WhenLessThanTwoPoints_DoesNotDraw()
    {
        IRenderTarget target = new FakeRenderTarget(2, 2);
        LineRenderer renderer = new()
        {
            RenderInTargetSpace = true,
            Color = BasicColor.White,
            Points = [(1, 1)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(target, (0, 0));

        target.AssertDrawnColor(BasicColor.White, []);
    }

    [Fact]
    public void Render_InWorldSpace_UsingBoxDrawingGlyphs_FlipsVerticalConnections()
    {
        IRenderTarget target = new FakeRenderTarget(3, 3);
        LineRenderer renderer = new()
        {
            UseBoxDrawingGlyphs = true,
            Color = BasicColor.White,
            Points = [(0, 0), (1, 1)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(target, (-1.5f, 1.5f));

        Assert.Equal(target[1, 1], new Cell(glyph: '╶', charColor: BasicColor.White));
        Assert.Equal(target[2, 1], new Cell(glyph: '┘', charColor: BasicColor.White));
        Assert.Equal(target[2, 0], new Cell(glyph: '╷', charColor: BasicColor.White));
    }
}

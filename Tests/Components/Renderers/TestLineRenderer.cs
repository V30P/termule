using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;
using static Termule.Tests.Components.Utilities;

namespace Termule.Tests.Components;

public class TestLineRenderer
{
    public static IEnumerable<object[]> SingleSegmentData =>
    [
        [new Vector[] { (0, 0), (3, 0) }, new VectorInt[] { (0, 0), (1, 0), (2, 0), (3, 0) }],
        [new Vector[] { (1, 0), (1, 3) }, new VectorInt[] { (1, 0), (1, 1), (1, 2), (1, 3) }],
        [new Vector[] { (0, 0), (3, 3) }, new VectorInt[] { (0, 0), (1, 1), (2, 2), (3, 3) }],
        [new Vector[] { (3, 1), (0, 1) }, new VectorInt[] { (3, 1), (2, 1), (1, 1), (0, 1) }],
        [new Vector[] { (0, 0), (1, 3) }, new VectorInt[] { (0, 0), (0, 1), (1, 2), (1, 3) }]
    ];

    [Theory]
    [MemberData(nameof(SingleSegmentData))]
    public void Render_DrawsSingleSegment(Vector[] points, VectorInt[] expectedCells)
    {
        FrameBuffer frame = new(6, 6);
        LineRenderer renderer = new() { TargetSpace = true, Color = BasicColor.White, Points = [.. points] };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, expectedCells);
    }

    [Fact]
    public void Render_DrawsPolylineSegments()
    {
        FrameBuffer frame = new(4, 4);
        LineRenderer renderer = new() { TargetSpace = true, Color = BasicColor.White, Points = [(0, 0), (2, 0), (2, 2)] };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, [
            (0, 0), (1, 0), (2, 0), (2, 1), (2, 2)
        ]);
    }

    [Fact]
    public void Render_UsesTransformPositionAsLineOrigin()
    {
        FrameBuffer frame = new(6, 4);
        LineRenderer renderer = new() { TargetSpace = true, Color = BasicColor.White, Points = [(0, 0), (2, 0)] };
        _ = new GameObject(new Transform { Pos = (2, 1) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, [
            (2, 1), (3, 1), (4, 1)
        ]);
    }

    [Fact]
    public void Render_WhenLessThanTwoPoints_DoesNotDraw()
    {
        FrameBuffer frame = new(2, 2);
        LineRenderer renderer = new() { TargetSpace = true, Color = BasicColor.White, Points = [(1, 1)] };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, []);
    }
}
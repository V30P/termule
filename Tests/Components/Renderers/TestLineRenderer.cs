using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;
using static Termule.Tests.Components.Utilities;

namespace Termule.Tests.Components;

public class TestLineRenderer
{
    private static readonly Color TestColor = BasicColor.White;

    public static IEnumerable<object[]> SingleSegmentData =>
    [
        [new Vector[] { (0, 0), (3, 0) }, new VectorInt[] { (0, 0), (1, 0), (2, 0), (3, 0) }],
        [new Vector[] { (1, 0), (1, 3) }, new VectorInt[] { (1, 0), (1, 1), (1, 2), (1, 3) }],
        [new Vector[] { (0, 0), (3, 3) }, new VectorInt[] { (0, 0), (1, 1), (2, 2), (3, 3) }],
        [new Vector[] { (3, 1), (0, 1) }, new VectorInt[] { (3, 1), (2, 1), (1, 1), (0, 1) }],
        [new Vector[] { (0, 0), (1, 3) }, new VectorInt[] { (0, 0), (0, 1), (1, 2), (1, 3) }]
    ];

    [Fact]
    public void Render_ShouldDrawPolylineSegments()
    {
        FrameBuffer frame = new(4, 4);
        LineRenderer renderer = new()
        {
            TargetSpace = true,
            Color = TestColor,
            Points = [(0, 0), (2, 0), (2, 2)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, TestColor, [
            (0, 0), (1, 0), (2, 0), (2, 1), (2, 2)
        ]);
    }

    [Theory]
    [MemberData(nameof(SingleSegmentData))]
    public void Render_ShouldDrawSingleSegment(Vector[] points, VectorInt[] expectedCells)
    {
        FrameBuffer frame = new(6, 6);
        LineRenderer renderer = new()
        {
            TargetSpace = true,
            Color = TestColor,
            Points = points.ToList()
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, TestColor, expectedCells);
    }

    [Fact]
    public void Render_ShouldNotDraw_WhenLessThanTwoPoints()
    {
        FrameBuffer frame = new(2, 2);
        LineRenderer renderer = new()
        {
            TargetSpace = true,
            Color = TestColor,
            Points = [(1, 1)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, TestColor, []);
    }

    [Fact]
    public void Render_ShouldUseTransformPositionAsLineOrigin()
    {
        FrameBuffer frame = new(6, 4);
        LineRenderer renderer = new()
        {
            TargetSpace = true,
            Color = TestColor,
            Points = [(0, 0), (2, 0)]
        };
        _ = new GameObject(new Transform { Pos = (2, 1) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, TestColor, [
            (2, 1), (3, 1), (4, 1)
        ]);
    }
}
using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Tests.Components;

public class TestCircleRenderer
{
    private static readonly Color TestColor = BasicColor.White;

    public static IEnumerable<object[]> OutlineCircleData =>
    [
        [1f, new VectorInt[] { (2, 3), (3, 2), (3, 4), (4, 3) }],
        [
            2f, new VectorInt[]
            {
                (3, 5), (3, 1), (5, 3), (1, 3),
                (4, 5), (4, 1), (5, 2), (1, 2),
                (2, 1), (2, 5), (1, 4), (5, 4)
            }
        ],
        [
            3f, new VectorInt[]
            {
                (3, 6), (3, 0), (6, 3), (0, 3),
                (4, 6), (4, 0), (6, 2), (0, 2),
                (2, 0), (2, 6), (0, 4), (6, 4),
                (5, 6), (5, 0), (6, 1), (0, 1),
                (1, 0), (1, 6), (0, 5), (6, 5)
            }
        ]
    ];

    public static IEnumerable<object[]> FilledCircleData =>
    [
        [
            1f, new VectorInt[]
            {
                (1, 2),
                (2, 1), (2, 2), (2, 3),
                (3, 2)
            }
        ],
        [
            2f, new VectorInt[]
            {
                (1, 4), (2, 4), (3, 4),
                (1, 3), (2, 3), (3, 3),
                (0, 2), (1, 2), (2, 2), (3, 2), (4, 2),
                (0, 1), (1, 1), (2, 1), (3, 1), (4, 1),
                (1, 0), (2, 0), (3, 0),
                (0, 3), (4, 3)
            }
        ],
        [
            3f, new VectorInt[]
            {
                (0, 0), (1, 0), (2, 0), (3, 0), (4, 0),
                (0, 1), (1, 1), (2, 1), (3, 1), (4, 1),
                (0, 2), (1, 2), (2, 2), (3, 2), (4, 2),
                (0, 3), (1, 3), (2, 3), (3, 3), (4, 3),
                (0, 4), (1, 4), (2, 4), (3, 4), (4, 4)
            }
        ]
    ];

    public static IEnumerable<object[]> ViewOriginData =>
    [
        [(1f, 1f), (0f, 0f), (1, 1)],
        [(1f, 1f), (2f, 2f), (1, 1)],
        [(2.25f, 1.75f), (1f, 1f), (2, 2)]
    ];

    private static void AssertDrawnPoints(FrameBuffer frame, Color expectedColor,
        IReadOnlyCollection<VectorInt> expectedPoints)
    {
        var actualPoints = new HashSet<VectorInt>();

        for (var x = 0; x < frame.Size.X; x++)
        for (var y = 0; y < frame.Size.Y; y++)
        {
            if (frame[x, y].Color == expectedColor)
            {
                actualPoints.Add((x, y));
            }
        }

        Assert.Equal(expectedPoints.Count, actualPoints.Count);

        var missing = expectedPoints.Where(p => !actualPoints.Contains(p)).ToArray();
        Assert.True(
            missing.Length == 0,
            $"Expected points: {string.Join(", ", expectedPoints)}; actual points: {string.Join(", ", actualPoints)}; missing points: {string.Join(", ", missing)}");
    }

    [Fact]
    public void Radius_ShouldThrow_WhenNegative()
    {
        var renderer = new CircleRenderer();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => renderer.Radius = -1);

        Assert.Equal("Radius", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(OutlineCircleData))]
    public void Render_ShouldDrawExpectedOutlinePoints(float radius, VectorInt[] expectedPoints)
    {
        var frame = new FrameBuffer(7, 7);
        var renderer = new CircleRenderer { Radius = radius, Color = TestColor, TargetSpace = true };
        _ = new GameObject(new Transform { Pos = (3, 3) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnPoints(frame, TestColor, expectedPoints);
    }

    [Fact]
    public void Render_ShouldDrawSingleCenterPoint_WhenRadiusIsZero()
    {
        var frame = new FrameBuffer(1, 1);
        var renderer = new CircleRenderer { Color = TestColor, TargetSpace = true };
        GameObject _ = [new Transform { Pos = (0, 0) }, renderer];

        renderer.Render(frame, (0, 0));

        AssertDrawnPoints(frame, TestColor, [
            (0, 0)
        ]);
    }

    [Fact]
    public void Render_ShouldDuplicateHorizontally_WhenDoubleWideIsTrue()
    {
        var frame = new FrameBuffer(5, 3);
        var renderer = new CircleRenderer { Radius = 1, Color = TestColor, TargetSpace = true, DoubleWide = true };

        _ = new GameObject(new Transform { Pos = (1, 1) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnPoints(frame, TestColor, [
            (0, 0), (1, 0),
            (2, 1), (3, 1),
            (0, 2), (1, 2)
        ]);
    }

    [Theory]
    [MemberData(nameof(FilledCircleData))]
    public void Render_ShouldFillInterior_WhenFilledIsTrue(float radius, VectorInt[] expectedPoints)
    {
        var frame = new FrameBuffer(5, 5);
        var renderer = new CircleRenderer { Radius = radius, Filled = true, Color = TestColor, TargetSpace = true };
        _ = new GameObject(new Transform { Pos = (2, 2) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnPoints(frame, TestColor, expectedPoints);
    }

    [Theory]
    [MemberData(nameof(ViewOriginData))]
    public void Render_ShouldRespectViewOrigin(Vector transformPos, Vector viewOrigin, VectorInt expectedCenter)
    {
        var frame = new FrameBuffer(3, 3);
        var renderer = new CircleRenderer { Color = TestColor, TargetSpace = true };
        _ = new GameObject(new Transform { Pos = transformPos }, renderer);

        renderer.Render(frame, viewOrigin);

        AssertDrawnPoints(frame, TestColor, [
            expectedCenter
        ]);
    }

    [Fact]
    public void Render_ShouldUseRegisteredTransformPosition()
    {
        var frame = new FrameBuffer(1, 1);
        var renderer = new CircleRenderer { Radius = 0, Color = TestColor, TargetSpace = true };
        GameObject _ = [new Transform { Pos = (0, 0) }, renderer];

        renderer.Render(frame, (0, 0));

        AssertDrawnPoints(frame, TestColor, [
            (0, 0)
        ]);
    }
}
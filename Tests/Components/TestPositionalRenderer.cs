using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Types.Vectors;

namespace Termule.Tests.Components;

public class TestPositionalRenderer
{
    private sealed class FakePositionalRenderer(Vector offset = default) : PositionalRenderer
    {
        protected override Vector Offset { get; } = offset;

        public FrameBuffer? CapturedFrame { get; private set; }
        public VectorInt? CapturedOrigin { get; private set; }
        public Vector? CapturedOffset { get; private set; }
        public int RenderCount { get; private set; }

        private protected override void RenderAtPosition(PositionalRenderContext context)
        {
            RenderCount++;
            CapturedFrame = context.Frame;
            CapturedOrigin = context.Origin;
            CapturedOffset = context.Offset;
        }
    }

    private const float PositionEpsilon = 0.0001f;

    public static IEnumerable<object[]> GameSpaceConversionData =
    [
        [(0f, 0f), (0f, 0f), (0, 0), (0f, 0f)],
        [(1f, 1f), (0f, 0f), (1, -1), (0f, 0f)],
        [(-1f, -1f), (0f, 0f), (-1, 1), (0f, 0f)],

        [(0.25f, 0.25f), (0f, 0f), (0, 0), (0.25f, -0.25f)],
        [(0.75f, 0.75f), (0f, 0f), (1, -1), (-0.25f, 0.25f)],

        [(10.25f, 5f), (5f, 5f), (5, 0), (0.25f, 0f)],
        [(3f, 2f), (0.75f, 0.25f), (2, -2), (0.25f, 0.25f)],
        [(-3f, -2f), (-0.75f, -0.25f), (-2, 2), (-0.25f, -0.25f)],
    ];

    public static IEnumerable<object[]> TargetSpaceConversionData =
    [
        [(0f, 0f), (1f, 2f), (0, 0), (0f, 0f)],
        [(1f, 5f), (3f, 4f), (1, 5), (0f, 0f)],
        [(10.25f, 5f), (5f, 5f), (10, 5), (0.25f, 0f)],

        [(-1.25f, 2.5f), (100f, -50f), (-1, 2), (-0.25f, 0.5f)],
        [(4.8f, -3.2f), (-9.1f, 12.3f), (5, -3), (-0.2f, -0.2f)],
    ];

    [Theory]
    [MemberData(nameof(GameSpaceConversionData))]
    public void Position_ShouldBeConvertedCorrectly_WhenInGameSpace(
        Vector transformPos,
        Vector viewOrigin,
        VectorInt expectedOrigin,
        Vector expectedOffset)
    {
        var renderer = new FakePositionalRenderer();
        GameObject _ = [new Transform { Pos = transformPos }, renderer];

        renderer.Render(new FrameBuffer(0, 0), viewOrigin);

        Assert.Equal(expectedOrigin, renderer.CapturedOrigin);
        AssertVectorApproximately(expectedOffset, renderer.CapturedOffset, PositionEpsilon);
    }

    [Theory]
    [MemberData(nameof(TargetSpaceConversionData))]
    public void Position_ShouldBeConvertedCorrectly_WhenInTargetSpace(
        Vector transformPos,
        Vector viewOrigin,
        VectorInt expectedOrigin,
        Vector expectedOffset)
    {
        var renderer = new FakePositionalRenderer { TargetSpace = true };
        GameObject _ = [new Transform { Pos = transformPos }, renderer];

        renderer.Render(new FrameBuffer(0, 0), viewOrigin);

        Assert.Equal(expectedOrigin, renderer.CapturedOrigin);
        AssertVectorApproximately(expectedOffset, renderer.CapturedOffset, PositionEpsilon);
    }

    [Fact]
    public void Render_ShouldInvokeDerivedRendererOnce_AndPassFrameThrough()
    {
        var frame = new FrameBuffer(0, 0);
        var renderer = new FakePositionalRenderer();
        GameObject _ = [new Transform { Pos = (0, 0) }, renderer];

        renderer.Render(frame, (0, 0));

        Assert.Equal(1, renderer.RenderCount);
        Assert.Same(frame, renderer.CapturedFrame);
    }

    [Fact]
    public void Offset_ShouldApplyCorrectly_WhenInGameSpace()
    {
        var renderer = new FakePositionalRenderer((0.75f, 0.75f));
        GameObject _ = [new Transform { Pos = (0, 0) }, renderer];

        renderer.Render(new FrameBuffer(0, 0), (0, 0));

        Assert.Equal((1, -1), renderer.CapturedOrigin);
        AssertVectorApproximately((-0.25f, 0.25f), renderer.CapturedOffset, PositionEpsilon);
    }

    [Fact]
    public void Offset_ShouldApplyCorrectly_WhenInTargetSpace()
    {
        var renderer = new FakePositionalRenderer((0.75f, 0.75f)) { TargetSpace = true };
        GameObject _ = [new Transform { Pos = (0, 0) }, renderer];

        renderer.Render(new FrameBuffer(0, 0), (0, 0));

        Assert.Equal((1, 1), renderer.CapturedOrigin);
        AssertVectorApproximately((-0.25f, -0.25f), renderer.CapturedOffset, PositionEpsilon);
    }

    private static void AssertVectorApproximately(Vector expected, Vector? actual, float epsilon)
    {
        Assert.NotNull(actual);
        Assert.InRange(actual.Value.X, expected.X - epsilon, expected.X + epsilon);
        Assert.InRange(actual.Value.Y, expected.Y - epsilon, expected.Y + epsilon);
    }
}
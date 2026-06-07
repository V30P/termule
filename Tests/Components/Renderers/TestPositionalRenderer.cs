using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;
using Termule.Tests.Common;

namespace Termule.Tests.Components;

public class TestPositionalRenderer
{
    public static readonly TheoryData<float[], int[], float[]>
        GameSpaceConversionData = new()
    {
        { [0, 0], [1, 1], [0.5f, 0.5f] },
        { [0.25f, -0.25f], [1, 1], [0.75f, 0.75f] },
        { [0.75f, -0.75f], [2, 2], [0.25f, 0.25f] },
        { [-1, 0], [0, 1], [0.5f, 0.5f] },
        { [0, -1], [1, 2], [0.5f, 0.5f] }
    };

    public static readonly TheoryData<float[], int[], float[]>
        TargetSpaceConversionData = new()
    {
        { [0, 0], [0, 0], [0, 0] },
        { [0.5f, 0.5f], [0, 0], [0.5f, 0.5f] },
        { [1, 1], [1, 1], [0, 0] },
        { [1.25f, 2.75f], [1, 2], [0.25f, 0.75f] }
    };

    [Fact]
    public void Render_InvokesDerivedRendererAndPassesParams()
    {
        IRenderTarget target = new FakeRenderTarget(0, 0);
        FakePositionalRenderer renderer = new();
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(target, (-1.5f, 1.5f));

        Assert.Equal(1, renderer.RenderCount);
        Assert.NotNull(renderer.CapturedTarget);
    }

    [Theory]
    [MemberData(nameof(GameSpaceConversionData))]
    public void Render_InGameSpace_CorrectlyAppliesPosition(
        float[] transformPos,
        int[] expectedOrigin,
        float[] expectedError)
    {
        FakePositionalRenderer renderer = new();
        _ = new GameObject(
            new Transform { Pos = (transformPos[0], transformPos[1]) },
            renderer
        );
        IRenderTarget target = new FakeRenderTarget(5, 5);

        renderer.Render(target, (-1.5f, 1.5f));

        target.AssertDrawnColor(BasicColor.White, [(expectedOrigin[0], expectedOrigin[1])]);
        AssertVectorApproximately(
            (expectedError[0], expectedError[1]),
            renderer.CapturedSubPixelOffset
        );
    }

    [Theory]
    [MemberData(nameof(TargetSpaceConversionData))]
    public void Render_InTargetSpace_CorrectlyAppliesPosition(
        float[] transformPos,
        int[] expectedOrigin,
        float[] expectedError)
    {
        FakePositionalRenderer renderer = new() { RenderInTargetSpace = true };
        _ = new GameObject(
            new Transform { Pos = (transformPos[0], transformPos[1]) },
            renderer
        );
        IRenderTarget target = new FakeRenderTarget(5, 5);

        renderer.Render(target, (-1.5f, 1.5f));

        target.AssertDrawnColor(BasicColor.White, [(expectedOrigin[0], expectedOrigin[1])]);
        AssertVectorApproximately(
            (expectedError[0], expectedError[1]),
            renderer.CapturedSubPixelOffset
        );
    }

    private static void AssertVectorApproximately(Vector expected, Vector? actual)
    {
        const float VectorEpsilon = 0.0001f;

        _ = Assert.NotNull(actual);
        Assert.InRange(actual.Value.X, expected.X - VectorEpsilon, expected.X + VectorEpsilon);
        Assert.InRange(actual.Value.Y, expected.Y - VectorEpsilon, expected.Y + VectorEpsilon);
    }

    private sealed class FakePositionalRenderer(Vector offset = default) : PositionalRenderer
    {
        public int RenderCount { get; private set; }

        public IRenderTarget CapturedTarget { get; private set; }

        public Vector CapturedSubPixelOffset { get; private set; }

        protected override Vector Offset { get; } = offset;

        private protected override void RenderPositionally(IRenderTarget target, Vector sub)
        {
            RenderCount++;
            CapturedTarget = target;
            CapturedSubPixelOffset = sub;

            target.Draw((0, 0), BasicColor.White);
        }
    }
}

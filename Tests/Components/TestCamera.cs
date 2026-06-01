using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Components;

public class TestCamera
{
    public static readonly TheoryData<int[], int[], int[], int[]>
        PositionConversionData = new()
        {
            { [0, 0], [0, 0], [0, 0], [0, 0] },
            { [10, 10], [0, 0], [0, 0], [5, 5] },
            { [10, 10], [0, 0], [1, 0], [6, 5] },
            { [10, 10], [0, 0], [0, 1], [5, 4] },
            { [10, 10], [0, 0], [-1, 0], [4, 5] },
            { [10, 10], [0, 0], [0, -1], [5, 6] },
            { [10, 10], [2, 0], [0, 0], [7, 5] },
            { [10, 10], [2, 0], [-2, 0], [5, 5] },
            { [10, 10], [2, 0], [1, 1], [8, 4] },
            { [8, 6], [3, -2], [-3, 2], [4, 3] },
            { [8, 6], [3, -2], [-2, 3], [5, 2] }
        };

    [Theory]
    [MemberData(nameof(PositionConversionData))]
    public void GameToTargetPos_MapsCorrectly(
        int[] targetSize,
        int[] transformPos,
        int[] gamePos,
        int[] targetPos)
    {
        FakeTarget target = new((targetSize[0], targetSize[1]));
        Game game = new();
        Camera camera = new() { Target = target };
        game.World.Add(
            new Transform { Pos = (transformPos[0], transformPos[1]) },
            camera);

        Assert.Equal(
            (targetPos[0], targetPos[1]),
            camera.GameToTargetPos((gamePos[0], gamePos[1])));
    }

    [Theory]
    [MemberData(nameof(PositionConversionData))]
    public void TargetToGamePos_MapsCorrectly(
        int[] targetSize,
        int[] transformPos,
        int[] gamePos,
        int[] targetPos)
    {
        FakeTarget target = new((targetSize[0], targetSize[1]));
        Game game = new();
        Camera camera = new() { Target = target };
        game.World.Add(
            new Transform { Pos = (transformPos[0], transformPos[1]) },
            camera);

        Assert.Equal(
            (gamePos[0], gamePos[1]),
            camera.TargetToGamePos((targetPos[0], targetPos[1])));
    }

    [Fact]
    public void Tick_CallsPrintOnTarget()
    {
        FakeTarget target = new((0, 0));
        Game game = new();
        game.World.Add(new Camera { Target = target });

        game.Systems.Install(new RenderSystem());
        game.Start();

        game.RunTicks(5);

        Assert.Equal(5, target.PrintCount);
    }

    private sealed class FakeTarget(VectorInt size) : ICameraTarget
    {
        private readonly IRenderTarget renderTarget = new FrameBuffer(size.X, size.Y);

        public int PrintCount { get; private set; }

        public VectorInt Size { get; } = size;

        IRenderTarget ICameraTarget.GetRenderTarget()
        {
            return renderTarget;
        }

        void ICameraTarget.Update()
        {
            PrintCount++;
        }
    }
}

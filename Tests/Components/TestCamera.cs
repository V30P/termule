using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.RenderSystem;
using Termule.Engine.Types.Content;
using Termule.Engine.Types.Vectors;

namespace Termule.Tests.Components;

public class TestCamera
{
    private class FakeTarget(VectorInt size) : ICameraTarget
    {
        public int PrintCount { get; private set; }
        public VectorInt Size { get; } = size;

        public FrameBuffer Buffer { get; set; } = new(size.X, size.Y);

        public void Update()
        {
            PrintCount++;
        }
    }

    public static IEnumerable<object[]> PositionConversionData =
    [
        [(0, 0), (0, 0), (0, 0), (0, 0)],

        [(10, 10), (0, 0), (0, 0), (5, 5)],
        [(10, 10), (0, 0), (1, 0), (6, 5)],
        [(10, 10), (0, 0), (0, 1), (5, 4)],
        [(10, 10), (0, 0), (-1, 0), (4, 5)],
        [(10, 10), (0, 0), (0, -1), (5, 6)],

        [(10, 10), (2, 0), (0, 0), (7, 5)],
        [(10, 10), (2, 0), (-2, 0), (5, 5)],
        [(10, 10), (2, 0), (1, 1), (8, 4)],

        [(8, 6), (3, -2), (-3, 2), (4, 3)],
        [(8, 6), (3, -2), (-2, 3), (5, 2)]
    ];

    [Fact]
    public void BackgroundCell_ShouldFillBuffer()
    {
        var background = new Cell(BasicColor.White, 'T', BasicColor.Black);
        FakeTarget target = new((5, 5));
        var game = Game.Create();
        game.Root.Add(
            new Camera { Target = target, BackgroundCell = background });

        game.Systems.Install(new RenderSystem());
        game.Prepare();

        game.RunFrame();

        for (var x = 0; x < target.Size.X; x++)
        {
            for (var y = 0; y < target.Size.Y; y++)
            {
                Assert.Equal(background, target.Buffer[x, y]);
            }
        }
    }

    [Theory]
    [MemberData(nameof(PositionConversionData))]
    public void GameToTargetPos_ShouldMapCorrectly(VectorInt targetSize, VectorInt transformPos, VectorInt gamePos,
        VectorInt targetPos)
    {
        FakeTarget target = new(targetSize);
        var game = Game.Create();
        var camera = new Camera { Target = target };
        game.Root.Add(new Transform { Pos = transformPos }, camera);

        Assert.Equal(targetPos, camera.GameToTargetPos(gamePos));
    }


    [Theory]
    [MemberData(nameof(PositionConversionData))]
    public void TargetToGamePos_ShouldMapCorrectly(VectorInt targetSize, VectorInt transformPos, VectorInt gamePos,
        VectorInt targetPos)
    {
        FakeTarget target = new(targetSize);
        var game = Game.Create();
        var camera = new Camera { Target = target };
        game.Root.Add(new Transform { Pos = transformPos }, camera);

        Assert.Equal(gamePos, camera.TargetToGamePos(targetPos));
    }

    [Fact]
    public void Tick_ShouldCallPrintOnTarget()
    {
        FakeTarget target = new((0, 0));
        var game = Game.Create();
        game.Root.Add(new Camera { Target = target });

        game.Systems.Install(new RenderSystem());
        game.Prepare();

        game.RunForFrames(5);

        Assert.Equal(5, target.PrintCount);
    }
}
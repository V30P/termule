using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;
using static Termule.Tests.Components.Utilities;

namespace Termule.Tests.Components;

public class TestCircleRenderer
{
    public static IEnumerable<object[]> OutlineCircleData =
    [
        [1f, new VectorInt[] { (2, 3), (3, 2), (3, 4), (4, 3) }],
        [
            2f,
            new VectorInt[]
            {
                (3, 5), (3, 1), (5, 3), (1, 3), (4, 5), (4, 1), (5, 2), (1, 2), (2, 1), (2, 5), (1, 4), (5, 4)
            }
        ],
        [
            3f,
            new VectorInt[]
            {
                (3, 6), (3, 0), (6, 3), (0, 3), (4, 6), (4, 0), (6, 2), (0, 2), (2, 0), (2, 6), (0, 4), (6, 4),
                (5, 6), (5, 0), (6, 1), (0, 1), (1, 0), (1, 6), (0, 5), (6, 5)
            }
        ]
    ];

    public static IEnumerable<object[]> FilledCircleData =
    [
        [
            1f, new VectorInt[] { (1, 2), (2, 1), (2, 2), (2, 3), (3, 2) }
        ],
        [
            2f,
            new VectorInt[]
            {
                (1, 4), (2, 4), (3, 4), (1, 3), (2, 3), (3, 3), (0, 2), (1, 2), (2, 2), (3, 2), (4, 2), (0, 1),
                (1, 1), (2, 1), (3, 1), (4, 1), (1, 0), (2, 0), (3, 0), (0, 3), (4, 3)
            }
        ],
        [
            3f,
            new VectorInt[]
            {
                (0, 0), (1, 0), (2, 0), (3, 0), (4, 0), (0, 1), (1, 1), (2, 1), (3, 1), (4, 1), (0, 2), (1, 2),
                (2, 2), (3, 2), (4, 2), (0, 3), (1, 3), (2, 3), (3, 3), (4, 3), (0, 4), (1, 4), (2, 4), (3, 4),
                (4, 4)
            }
        ]
    ];

    public static IEnumerable<object[]> ViewOriginData =
    [
        [(1f, 1f), (0f, 0f), (1, 1)],
        [(1f, 1f), (2f, 2f), (1, 1)],
        [(2.25f, 1.75f), (1f, 1f), (2, 2)]
    ];

    [Fact]
    public void SettingRadius_ToNegative_Throws()
    {
        CircleRenderer renderer = new();

        ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => renderer.Radius = -1);

        Assert.Equal("Radius", ex.ParamName);
    }

    [Fact]
    public void Render_WhenRadiusIsZero_DrawsSingleCenterCell()
    {
        FrameBuffer frame = new(1, 1);
        CircleRenderer renderer = new() { Color = BasicColor.White, TargetSpace = true };
        GameObject _ = [new Transform { Pos = (0, 0) }, renderer];

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, [
            (0, 0)
        ]);
    }

    [Theory]
    [MemberData(nameof(OutlineCircleData))]
    public void Render_DrawsExpectedOutlineCells(float radius, VectorInt[] expectedCells)
    {
        FrameBuffer frame = new(7, 7);
        CircleRenderer renderer = new() { Radius = radius, Color = BasicColor.White, TargetSpace = true };
        _ = new GameObject(new Transform { Pos = (3, 3) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, expectedCells);
    }

    [Theory]
    [MemberData(nameof(ViewOriginData))]
    public void Render_RespectsViewOrigin(Vector transformPos, Vector viewOrigin, VectorInt expectedCenter)
    {
        FrameBuffer frame = new(3, 3);
        CircleRenderer renderer = new() { Color = BasicColor.White, TargetSpace = true };
        _ = new GameObject(new Transform { Pos = transformPos }, renderer);

        renderer.Render(frame, viewOrigin);

        AssertDrawnCells(frame, BasicColor.White, [
            expectedCenter
        ]);
    }

    [Fact]
    public void Render_WhenDoubleWideIsTrue_DuplicatesCellsHorizontally()
    {
        FrameBuffer frame = new(5, 3);
        CircleRenderer renderer = new() { Radius = 1, Color = BasicColor.White, TargetSpace = true, DoubleWide = true };

        _ = new GameObject(new Transform { Pos = (1, 1) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, [
            (0, 0), (1, 0),
            (2, 1), (3, 1),
            (0, 2), (1, 2)
        ]);
    }

    [Theory]
    [MemberData(nameof(FilledCircleData))]
    public void Render_WhenFilledIsTrue_FillsInteriorCells(float radius, VectorInt[] expectedCells)
    {
        FrameBuffer frame = new(5, 5);
        CircleRenderer renderer = new() { Radius = radius, Filled = true, Color = BasicColor.White, TargetSpace = true };
        _ = new GameObject(new Transform { Pos = (2, 2) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnCells(frame, BasicColor.White, expectedCells);
    }
}
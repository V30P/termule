using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;
using static Termule.Tests.Components.Utilities;

namespace Termule.Tests.Components;

public class TestLineRenderer
{
    public static readonly TheoryData<float[][], int[][]> SingleSegmentData = new()
    {
        { [[0, 0], [3, 0]], [[0, 0], [1, 0], [2, 0], [3, 0]] },
        { [[1, 0], [1, 3]], [[1, 0], [1, 1], [1, 2], [1, 3]] },
        { [[0, 0], [3, 3]], [[0, 0], [1, 1], [2, 2], [3, 3]] },
        { [[3, 1], [0, 1]], [[3, 1], [2, 1], [1, 1], [0, 1]] },
        { [[0, 0], [1, 3]], [[0, 0], [0, 1], [1, 2], [1, 3]] }
    };

    public static readonly TheoryData<int[], int[], int[][], char[]> BoxDrawingData = new()
    {
        {
            [0, 1],
            [2, 1],
            [[0, 1], [1, 1], [2, 1]],
            ['╶', '─', '╴']
        },
        {
            [1, 0],
            [1, 2],
            [[1, 0], [1, 1], [1, 2]],
            ['╷', '│', '╵']
        },
        {
            [0, 0],
            [2, 2],
            [[0, 0], [1, 0], [1, 1], [2, 1], [2, 2]],
            ['╶', '┐', '└', '┐', '╵']
        },
        {
            [0, 2],
            [2, 0],
            [[0, 2], [1, 2], [1, 1], [2, 1], [2, 0]],
            ['╶', '┘', '┌', '┘', '╷']
        }
    };

    [Theory]
    [MemberData(nameof(SingleSegmentData))]
    public void Render_DrawsSingleSegment(float[][] points, int[][] expectedCells)
    {
        FrameBuffer frame = new(6, 6);
        Vector[] pointPositions = new Vector[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            pointPositions[i] = (points[i][0], points[i][1]);
        }

        VectorInt[] expectedCellPositions = new VectorInt[expectedCells.Length];
        for (int i = 0; i < expectedCells.Length; i++)
        {
            expectedCellPositions[i] = (expectedCells[i][0], expectedCells[i][1]);
        }

        LineRenderer renderer = new()
        {
            RenderInTargetSpace = true,
            Color = BasicColor.White,
            Points = [.. pointPositions]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnColor(frame, BasicColor.White, expectedCellPositions);
    }

    [Fact]
    public void Render_DrawsPolylineSegments()
    {
        FrameBuffer frame = new(4, 4);
        LineRenderer renderer = new()
        {
            RenderInTargetSpace = true,
            Color = BasicColor.White,
            Points = [(0, 0), (2, 0), (2, 2)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnColor(frame, BasicColor.White, [(0, 0), (1, 0), (2, 0), (2, 1), (2, 2)]);
    }

    [Fact]
    public void Render_WhenLessThanTwoPoints_DoesNotDraw()
    {
        FrameBuffer frame = new(2, 2);
        LineRenderer renderer = new()
        {
            RenderInTargetSpace = true,
            Color = BasicColor.White,
            Points = [(1, 1)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnColor(frame, BasicColor.White, []);
    }

    [Theory]
    [MemberData(nameof(BoxDrawingData))]
    public void Render_WhenUseBoxDrawingCharsIsTrue_DrawsExpectedChars(
        int[] start,
        int[] end,
        int[][] expectedPoints,
        char[] expectedGlyphs
    )
    {
        FrameBuffer frame = new(3, 3);
        Dictionary<VectorInt, char> expectedChars = [];
        for (int i = 0; i < expectedPoints.Length; i++)
        {
            int[] point = expectedPoints[i];
            expectedChars[new VectorInt(point[0], point[1])] = expectedGlyphs[i];
        }

        LineRenderer renderer = new()
        {
            RenderInTargetSpace = true,
            Color = BasicColor.White,
            Points = [(start[0], start[1]), (end[0], end[1])],
            UseBoxDrawingCharacters = true
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (0, 0));

        AssertDrawnChars(frame, expectedChars);
    }

    [Fact]
    public void Render_InWorldSpace_UsingBoxDrawingCharacters_FlipsVerticalConnections()
    {
        FrameBuffer frame = new(3, 3);
        LineRenderer renderer = new()
        {
            UseBoxDrawingCharacters = true,
            Color = BasicColor.White,
            Points = [(0, 0), (1, 1)]
        };
        _ = new GameObject(new Transform { Pos = (0, 0) }, renderer);

        renderer.Render(frame, (-1.5f, 1.5f));

        Assert.Equal(frame[1, 1], new Cell(character: '╶', charColor: BasicColor.White));
        Assert.Equal(frame[2, 1], new Cell(character: '┘', charColor: BasicColor.White));
        Assert.Equal(frame[2, 0], new Cell(character: '╷', charColor: BasicColor.White));
    }
}

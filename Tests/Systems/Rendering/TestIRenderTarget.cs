using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;
using Termule.Tests.Common;

namespace Termule.Tests.Systems.Rendering;

public class TestIRenderTarget
{
    public class TestDraw()
    {
        public static readonly TheoryData<BasicColor?, char?, BasicColor?> DrawData = new()
        {
            { null, null, null },
            { BasicColor.White, null, null },
            { null, 'X', null },
            { null, null, BasicColor.White },
            { BasicColor.White, 'X', BasicColor.White }
        };

        [Theory]
        [MemberData(nameof(DrawData))]
        public void DrawsProvidedValues(
            BasicColor? color,
            char? glyph,
            BasicColor? glyphColor)
        {
            IRenderTarget target = new FakeRenderTarget(1, 1);
            Cell expectedCell = new(
                color ?? default,
                glyph ?? '\0',
                glyphColor ?? default
            );

            target.Draw((0, 0), color, glyph, glyphColor);

            Assert.Equal(expectedCell, target[0, 0]);
        }

        [Fact]
        public void IgnoresOutOfBoundsPositions()
        {
            IRenderTarget target = new FakeRenderTarget(10, 5);
            target.Draw((-1, 0), BasicColor.White);
            target.Draw((0, -1), BasicColor.White);
            target.Draw((10, 0), BasicColor.White);
            target.Draw((0, 5), BasicColor.White);

            for (int x = 0; x < target.Size.X; x++)
            {
                for (int y = 0; y < target.Size.Y; y++)
                {
                    Assert.Equal(default, target[x, y]);
                }
            }
        }

        [Fact]
        public void CoversExistingValues()
        {
            IRenderTarget target = new FakeRenderTarget(1, 1);
            target.Draw((0, 0), BasicColor.White, 'X', BasicColor.White);

            target.Draw((0, 0), null, 'X');
            Assert.Equal(new Cell(BasicColor.White, 'X'), target[0, 0]);

            target.Draw((0, 0), BasicColor.White);
            Assert.Equal(new Cell(BasicColor.White), target[0, 0]);
        }

        [Fact]
        public void WhenLayerBoxDrawingGlyphsIsTrue_LayersMatchingBoxDrawingGlyphs()
        {
            IRenderTarget target = new FakeRenderTarget(1, 1);

            target.Draw((0, 0), glyph: '─', glyphColor: BasicColor.White);
            target.Draw((0, 0), glyph: '│', glyphColor: BasicColor.White);

            Assert.Equal('┼', target[0, 0].Glyph);
        }

        [Fact]
        public void WhenLayerBoxDrawingGlyphsIsTrue_DoesNotLayerNonMatchingBoxDrawingChars()
        {
            IRenderTarget target = new FakeRenderTarget(1, 1);

            target.Draw((0, 0), glyph: '─', glyphColor: BasicColor.White);
            target.Draw((0, 0), glyph: '│', glyphColor: BasicColor.Red);

            Assert.Equal('│', target[0, 0].Glyph);
        }

        [Fact]
        public void WhenLayerBoxDrawingGlyphsIsFalse_DoesNotLayerBoxDrawingGlyphs()
        {
            IRenderTarget target = new FakeRenderTarget(1, 1);

            target.Draw((0, 0), glyph: '─', glyphColor: BasicColor.White);
            target.Draw(
                (0, 0),
                glyph: '│',
                glyphColor: BasicColor.White,
                layerBoxDrawingChars: false
            );

            Assert.Equal('│', target[0, 0].Glyph);
        }
    }

    public class TestDrawContent
    {
        private static readonly IContent TestContent = new FakeContent(
            new Cell[,]
            {
                {
                    new(BasicColor.Red, '1', BasicColor.Green),
                    new(BasicColor.Green, '2', BasicColor.Blue)
                },
                {
                    new(BasicColor.Blue, '3', BasicColor.White),
                    new(BasicColor.White, '4', BasicColor.Red)
                }
            }
        );

        [Fact]
        public void DoesNotDrawDefaultValues()
        {
            Cell baseCell = new(BasicColor.White, 'X', BasicColor.White);
            IRenderTarget target = new FakeRenderTarget(1, 1);
            target.Draw((0, 0), baseCell.Color, baseCell.Glyph, baseCell.GlyphColor);

            target.DrawContent((0, 0), new FakeContent(new Cell[,] { { default } }));

            Assert.Equal(baseCell, target[0, 0]);
        }

        [Fact]
        public void DrawsContent()
        {
            IRenderTarget target = new FakeRenderTarget(2, 2);

            target.DrawContent((0, 0), TestContent);

            for (int x = 0; x < TestContent.Size.X; x++)
            {
                for (int y = 0; y < TestContent.Size.Y; y++)
                {
                    Assert.Equal(TestContent[x, y], target[x, y]);
                }
            }
        }

        [Fact]
        public void DrawsContentAtPosition()
        {
            IContent content = new FakeContent(new Cell[,] { { new(BasicColor.White) } });
            IRenderTarget target = new FakeRenderTarget(3, 3);

            target.DrawContent((1, 1), new FakeContent(new Cell[,] { { new(BasicColor.White) } }));

            Assert.Equal(content[0, 0], target[1, 1]);
        }

        [Fact]
        public void WhenFlipXIsTrue_DrawsContentFlippedOnX()
        {
            IRenderTarget target = new FakeRenderTarget(2, 2);

            target.DrawContent((0, 0), TestContent, flipX: true);

            for (int x = 0; x < TestContent.Size.X; x++)
            {
                for (int y = 0; y < TestContent.Size.Y; y++)
                {
                    Assert.Equal(
                        TestContent[TestContent.Size.X - x - 1, y],
                        target[x, y]
                    );
                }
            }
        }

        [Fact]
        public void WhenFlipYIsTrue_DrawsContentFlippedOnY()
        {
            IRenderTarget target = new FakeRenderTarget(2, 2);

            target.DrawContent((0, 0), TestContent, flipY: true);

            for (int x = 0; x < TestContent.Size.X; x++)
            {
                for (int y = 0; y < TestContent.Size.Y; y++)
                {
                    Assert.Equal(
                        TestContent[x, TestContent.Size.Y - y - 1],
                        target[x, y]
                    );
                }
            }
        }
    }

    public class TestDrawLine
    {
        public static readonly TheoryData<int[], int[], int[][]> SingleSegmentData = new()
        {
            { [0, 0], [3, 0], [[0, 0], [1, 0], [2, 0], [3, 0]] },
            { [1, 0], [1, 3], [[1, 0], [1, 1], [1, 2], [1, 3]] },
            { [0, 0], [3, 3], [[0, 0], [1, 1], [2, 2], [3, 3]] },
            { [3, 1], [0, 1], [[3, 1], [2, 1], [1, 1], [0, 1]] },
            { [0, 0], [1, 3], [[0, 0], [0, 1], [1, 2], [1, 3]] }
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
        public void DrawsLine(int[] start, int[] finish, int[][] expectedCells)
        {
            VectorInt[] expectedCellPositions = new VectorInt[expectedCells.Length];
            for (int i = 0; i < expectedCells.Length; i++)
            {
                expectedCellPositions[i] = (expectedCells[i][0], expectedCells[i][1]);
            }

            IRenderTarget target = new FakeRenderTarget(6, 6);

            target.DrawLine(
                (start[0], start[1]),
                (finish[0], finish[1]),
                BasicColor.White
            );

            target.AssertDrawnColor(BasicColor.White, expectedCellPositions);
        }

        [Theory]
        [MemberData(nameof(BoxDrawingData))]
        public void WhenUseBoxDrawingCharsIsTrue_DrawsExpectedChars(
            int[] start,
            int[] finish,
            int[][] expectedPoints,
            char[] expectedGlyphs)
        {
            IRenderTarget target = new FakeRenderTarget(3, 3);
            Dictionary<VectorInt, char> expectedChars = [];
            for (int i = 0; i < expectedPoints.Length; i++)
            {
                int[] point = expectedPoints[i];
                expectedChars[new VectorInt(point[0], point[1])] = expectedGlyphs[i];
            }

            target.DrawLine(
                (start[0], start[1]),
                (finish[0], finish[1]),
                BasicColor.White,
                useBoxDrawingGlyphs: true
            );

            target.AssertDrawnChars(expectedChars);
        }

        [Fact]
        public void WhenFlipVerticalGlyphConnectionsIsTrue_FlipsGlyphs()
        {
            IRenderTarget target = new FakeRenderTarget(1, 2);

            target.DrawLine(
                (0, 0),
                (0, 1),
                BasicColor.White,
                useBoxDrawingGlyphs: true,
                flipVerticalGlyphConnections: true
            );

            target.AssertDrawnChars(
                new Dictionary<VectorInt, char>
                {
                    [(0, 0)] = '╵',
                    [(0, 1)] = '╷'
                }
            );
        }
    }

    public class TestDrawCircle
    {
        public static readonly TheoryData<float, int[][]> PlainCircleData = new()
        {
            { 1f, [[2, 3], [3, 2], [3, 4], [4, 3]] },
            {
                2f,
                [
                    [3, 5], [3, 1], [5, 3], [1, 3], [4, 5], [4, 1], [5, 2], [1, 2], [2, 1], [2, 5],
                    [1, 4], [5, 4]
                ]
            },
            {
                3f,
                [
                    [3, 6], [3, 0], [6, 3], [0, 3], [4, 6], [4, 0], [6, 2], [0, 2], [2, 0], [2, 6],
                    [0, 4], [6, 4], [5, 6], [5, 0], [6, 1], [0, 1], [1, 0], [1, 6], [0, 5], [6, 5]
                ]
            }
        };

        public static readonly TheoryData<float, int[][]> FilledCircleData = new()
        {
            {
                1f,
                [
                    [1, 2], [2, 1], [2, 2], [2, 3], [3, 2]
                ]
            },
            {
                2f,
                [
                    [1, 4], [2, 4], [3, 4], [1, 3], [2, 3],
                    [3, 3], [0, 2], [1, 2], [2, 2], [3, 2],
                    [4, 2], [0, 1], [1, 1], [2, 1], [3, 1],
                    [4, 1], [1, 0], [2, 0], [3, 0], [0, 3],
                    [4, 3]
                ]
            },
            {
                3f,
                [
                    [0, 0], [1, 0], [2, 0], [3, 0], [4, 0],
                    [0, 1], [1, 1], [2, 1], [3, 1], [4, 1],
                    [0, 2], [1, 2], [2, 2], [3, 2], [4, 2],
                    [0, 3], [1, 3], [2, 3], [3, 3], [4, 3],
                    [0, 4], [1, 4], [2, 4], [3, 4], [4, 4]
                ]
            }
        };

        public static readonly TheoryData<float[], float[], int[]> ViewOriginData = new()
        {
            { [1f, 1f], [0f, 0f], [1, 1] },
            { [1f, 1f], [2f, 2f], [1, 1] },
            { [2.25f, 1.75f], [1f, 1f], [2, 2] }
        };

        [Theory]
        [MemberData(nameof(PlainCircleData))]
        public void DrawsCircle(float radius, int[][] expectedCellPositions)
        {
            IRenderTarget target = new FakeRenderTarget(7, 7);

            target.DrawCircle(
                (3, 3),
                radius,
                BasicColor.White
            );

            target.AssertDrawnColor(
                BasicColor.White,
                expectedCellPositions.Select(t => new VectorInt(t[0], t[1]))
            );
        }

        [Fact]
        public void WhenRadiusIsNegative_Throws()
        {
            _ = Assert.Throws<ArgumentOutOfRangeException>(
                () => new FakeRenderTarget(0, 0).DrawCircle((0, 0), -1, default)
            );
        }

        [Fact]
        public void WhenDoubleWideIsTrue_DuplicatesCellsHorizontally()
        {
            IRenderTarget target = new FakeRenderTarget(6, 3);

            target.DrawCircle(
                (2, 1),
                1,
                BasicColor.White,
                doubleWide: true
            );

            target.AssertDrawnColor(
                BasicColor.White,
                [(0, 1), (1, 1), (2, 0), (2, 2), (3, 0), (3, 2), (4, 1), (5, 1)]
            );
        }

        [Fact]
        public void WhenDoubleWideIsTrue_RespectsBias()
        {
            IRenderTarget target = new FakeRenderTarget(3, 1);
            target.DrawCircle(
                (1, 0),
                0,
                BasicColor.White,
                doubleWide: true,
                biasRight: false
            );
            target.AssertDrawnColor(BasicColor.White, [(0, 0), (1, 0)]);

            target = new FakeRenderTarget(3, 1);
            target.DrawCircle(
                (1, 0),
                0,
                BasicColor.White,
                doubleWide: true,
                biasRight: true
            );
            target.AssertDrawnColor(BasicColor.White, [(1, 0), (2, 0)]);
        }

        [Theory]
        [MemberData(nameof(FilledCircleData))]
        public void WhenFilledIsTrue_FillsInteriorCells(float radius, int[][] expectedCells)
        {
            IRenderTarget target = new FakeRenderTarget(5, 5);

            target.DrawCircle((2, 2), radius, BasicColor.White, filled: true);

            target.AssertDrawnColor(
                BasicColor.White,
                expectedCells.Select(t => new VectorInt(t[0], t[1]))
            );
        }
    }
}

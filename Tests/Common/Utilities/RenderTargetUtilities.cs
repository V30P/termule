using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Common;

internal static class RenderTargetUtilities
{
    public static void AssertDrawnColor(
        this IRenderTarget target,
        Color expectedColor,
        IEnumerable<VectorInt> expectedCells)
    {
        List<VectorInt> actualCells = [];

        for (int x = 0; x < target.Size.X; x++)
        {
            for (int y = 0; y < target.Size.Y; y++)
            {
                if (target[x, y].Color == expectedColor)
                {
                    actualCells.Add((x, y));
                }
            }
        }

        Assert.True(expectedCells.ToHashSet().SetEquals(actualCells));
    }

    public static void AssertDrawnChars(
        this IRenderTarget target,
        IReadOnlyDictionary<VectorInt, char> expectedChars)
    {
        for (int x = 0; x < target.Size.X; x++)
        {
            for (int y = 0; y < target.Size.Y; y++)
            {
                if (expectedChars.ContainsKey((x, y)))
                {
                    Assert.Equal(expectedChars[(x, y)], target[x, y].Glyph);
                }
                else
                {
                    Assert.Equal(default, target[x, y].Glyph);
                }
            }
        }
    }
}

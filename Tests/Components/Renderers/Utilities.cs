using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Components;

public static class Utilities
{
    public static void AssertDrawnColor(
        FrameBuffer frame,
        Color expectedColor,
        IReadOnlyCollection<VectorInt> expectedCells)
    {
        List<VectorInt> actualCells = [];

        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                if (frame[x, y].Color == expectedColor)
                {
                    actualCells.Add((x, y));
                }
            }
        }

        Assert.True(expectedCells.ToHashSet().SetEquals(actualCells));
    }

    public static void AssertDrawnChars(
        FrameBuffer frame,
        IReadOnlyDictionary<VectorInt, char> expectedChars)
    {
        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                if (expectedChars.ContainsKey((x, y)))
                {
                    Assert.Equal(expectedChars[(x, y)], frame[x, y].Character);
                }
                else
                {
                    Assert.Equal(default, frame[x, y].Character);
                }
            }
        }
    }
}
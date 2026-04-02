using Termule.Engine.Systems.Display;
using Termule.Engine.Types;

namespace Termule.Tests.Components;

public static class Utilities
{
    public static void AssertDrawnCells(FrameBuffer frame, Color expectedColor,
        IReadOnlyCollection<VectorInt> expectedCells)
    {
        HashSet<VectorInt> actualCells = [];

        for (int x = 0; x < frame.Size.X; x++)
        for (int y = 0; y < frame.Size.Y; y++)
        {
            if (frame[x, y].Color == expectedColor)
            {
                actualCells.Add((x, y));
            }
        }

        VectorInt[] missing = expectedCells.Where(p => !actualCells.Contains(p)).ToArray();
        Assert.True(
            missing.Length == 0,
            $"Expected Cells: {string.Join(", ", expectedCells)}; actual Cells: {string.Join(", ", actualCells)}; missing Cells: {string.Join(", ", missing)}");
    }
}
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Common;

public sealed class FakeRenderTarget(int width, int height) : IRenderTarget
{
    private readonly Cell[,] cells = new Cell[width, height];

    VectorInt IRenderTarget.LowerBound => (0, 0);

    VectorInt IRenderTarget.UpperBound => (width, height);

    ref Cell IRenderTarget.GetCellRef(int x, int y)
    {
        return ref cells[x, y];
    }
}

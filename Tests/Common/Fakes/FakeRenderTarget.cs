using Termule.Engine.Systems.Rendering;
using Termule.Engine.Types;

namespace Termule.Tests.Common;

internal sealed class FakeRenderTarget(int width, int height) : IRenderTarget
{
    private readonly Cell[,] cells = new Cell[width, height];

    VectorInt IRenderTarget.LowerBound => (0, 0);

    VectorInt IRenderTarget.UpperBound => (width, height);

    ref Cell IRenderTarget.this[int x, int y] => ref cells[x, y];
}

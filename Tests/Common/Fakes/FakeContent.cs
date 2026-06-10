using Termule.Engine.Types;

namespace Termule.Tests.Common;

internal sealed class FakeContent(Cell[,] cells) : IContent
{
    VectorInt IContent.Size => (cells.GetLength(0), cells.GetLength(1));

    Cell IContent.this[int x, int y] => cells[x, y];
}

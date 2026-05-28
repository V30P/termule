using Termule.Engine.Types;

namespace Termule.Engine.Systems.Rendering;

/// <summary>
///     <see cref="Image"/>-based <see cref="IRenderTarget"/> implementation.
/// </summary>
public sealed class FrameBuffer : Image, IRenderTarget
{
    internal FrameBuffer(int width, int height) : base(width, height)
    {
    }

    VectorInt IRenderTarget.LowerBound => (0, 0);

    VectorInt IRenderTarget.UpperBound => Size;

    ref Cell IRenderTarget.GetCellRef(int x, int y)
    {
        return ref Cells[x, y];
    }

    internal void Fill(Cell cell)
    {
        for (int x = 0; x < Size.X; x++)
        {
            for (int y = 0; y < Size.Y; y++)
            {
                Cells[x, y] = cell;
            }
        }
    }
}

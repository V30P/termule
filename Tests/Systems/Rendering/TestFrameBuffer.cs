using Termule.Engine.Systems.Display;
using Termule.Engine.Types;

namespace Termule.Tests.Systems.Rendering;

public class TestFrameBuffer
{
    [Fact]
    public void Fill_FillsWithProvidedCell()
    {
        FrameBuffer frame = new(10, 5);

        frame.Fill(new(BasicColor.White, 'X', BasicColor.White));

        for (int x = 0; x < frame.Size.X; x++)
        {
            for (int y = 0; y < frame.Size.Y; y++)
            {
                Assert.Equal(new(BasicColor.White, 'X', BasicColor.White), frame[x, y]);
            }
        }
    }
}

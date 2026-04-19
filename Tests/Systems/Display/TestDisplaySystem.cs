using Termule.Engine.Components.Camera;
using Termule.Engine.Systems.RenderSystem;

namespace Termule.Tests.Systems.Display;

public class TestDisplay
{
    private class FakeDisplay : Engine.Systems.Display.Display
    {
        public int PrintCount { get; private set; }

        public void SetSize(int width, int height)
        {
            Size = (width, height);
        }

        private protected override void PrintBuffer()
        {
            PrintCount++;
        }
    }

    [Fact]
    public void SettingSize_ShouldResizeBuffer()
    {
        FakeDisplay display = new();
        display.SetSize(10, 5);

        Assert.Equal((10, 5), ((ICameraTarget)display).Buffer.Size);
    }

    [Fact]
    public void Update_ShouldCallPrint_AndSwapBuffers()
    {
        FakeDisplay display = new();
        ICameraTarget target = display;
        FrameBuffer startingBuffer = target.Buffer;

        target.Update();
        Assert.NotEqual(startingBuffer, target.Buffer);
        Assert.Equal(1, display.PrintCount);

        target.Update();
        Assert.Equal(startingBuffer, target.Buffer);
        Assert.Equal(2, display.PrintCount);
    }
}
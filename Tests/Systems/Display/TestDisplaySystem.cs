using Termule.Engine.Components;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Rendering;

namespace Termule.Tests.Systems.Display;

public class TestDisplaySystem
{
    private class FakeDisplaySystem : DisplaySystem
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
    public void SettingSize_ResizesBuffer()
    {
        FakeDisplaySystem displaySystem = new();
        displaySystem.SetSize(10, 5);

        Assert.Equal((10, 5), ((ICameraTarget)displaySystem).Buffer.Size);
    }

    [Fact]
    public void Update_CallsPrintAndSwapsBuffers()
    {
        FakeDisplaySystem displaySystem = new();
        ICameraTarget target = displaySystem;
        FrameBuffer startingBuffer = target.Buffer;

        target.Update();
        Assert.NotEqual(startingBuffer, target.Buffer);
        Assert.Equal(1, displaySystem.PrintCount);

        target.Update();
        Assert.Equal(startingBuffer, target.Buffer);
        Assert.Equal(2, displaySystem.PrintCount);
    }
}
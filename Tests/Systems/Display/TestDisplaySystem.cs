using Termule.Engine.Components;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Rendering;

namespace Termule.Tests.Systems.Display;

public class TestDisplaySystem
{
    [Fact]
    public void SettingSize_ResizesBuffer()
    {
        FakeDisplaySystem displaySystem = new();
        displaySystem.SetSize(10, 5);

        Assert.Equal((10, 5), ((ICameraTarget) displaySystem).GetRenderTarget().Size);
    }

    [Fact]
    public void Update_CallsPrintAndSwapsBuffers()
    {
        FakeDisplaySystem displaySystem = new();
        ICameraTarget target = displaySystem;
        IRenderTarget startingBuffer = target.GetRenderTarget();

        target.Update();
        Assert.NotEqual(startingBuffer, target.GetRenderTarget());
        Assert.Equal(1, displaySystem.PrintCount);

        target.Update();
        Assert.Equal(startingBuffer, target.GetRenderTarget());
        Assert.Equal(2, displaySystem.PrintCount);
    }

    private sealed class FakeDisplaySystem : DisplaySystem
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
}

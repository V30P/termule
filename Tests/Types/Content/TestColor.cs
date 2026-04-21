using Termule.Engine.Types;

namespace Termule.Tests.Types.Content;

public class TestColor
{
    [Fact]
    public void ParameterlessConstructor_CreatesDefaultColor()
    {
        Color color = new();

        Assert.Equal(BasicColor.Default, color.Basic);
        Assert.Null(color.Full);
    }

    [Fact]
    public void BasicColorConstructor_SetsBasic()
    {
        Color color = BasicColor.Blue;

        Assert.Equal(BasicColor.Blue, color.Basic);
        Assert.Null(color.Full);
    }

    [Fact]
    public void FullColorConstructor_SetsFull()
    {
        FullColor full = new(100, 150, 200);

        Color color = (full.R, full.G, full.B);

        Assert.NotNull(color.Full);
        Assert.Equal(100, color.Full.Value.R);
        Assert.Equal(150, color.Full.Value.G);
        Assert.Equal(200, color.Full.Value.B);
    }

    [Fact]
    public void ImplicitOperatorFromBasicColor_CreatesColor()
    {
        Color color = BasicColor.Red;

        Assert.Equal(BasicColor.Red, color.Basic);
        Assert.Null(color.Full);
    }

    [Fact]
    public void ImplicitOperatorFronTuple_CreatesColor()
    {
        Color color = (255, 128, 64);

        Assert.NotNull(color.Full);
        Assert.Equal(255, color.Full.Value.R);
        Assert.Equal(128, color.Full.Value.G);
        Assert.Equal(64, color.Full.Value.B);
    }
}
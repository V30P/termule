using Termule.Engine.Types;

namespace Termule.Tests.Types.Content;

public class TestColor
{
    [Fact]
    public void BasicColorConstructor_SetsBasic()
    {
        Color color = BasicColor.Blue;

        Assert.Equal(BasicColor.Blue, color.Basic);
    }

    [Fact]
    public void FullColorConstructor_SetsFull()
    {
        FullColor fullColor = new(0.1f, 0.2f, 0.3f);
        Color color = new(fullColor);

        _ = Assert.NotNull(color.Full);
        Assert.Equal(0.1f, color.Full.Value.R);
        Assert.Equal(0.2f, color.Full.Value.G);
        Assert.Equal(0.3f, color.Full.Value.B);
    }

    [Fact]
    public void FloatComponentConstructor_SetsFullColor()
    {
        Color color = new(0.5f, 0.6f, 0.7f);

        _ = Assert.NotNull(color.Full);
        Assert.Equal(0.5f, color.Full.Value.R);
        Assert.Equal(0.6f, color.Full.Value.G);
        Assert.Equal(0.7f, color.Full.Value.B);
    }

    [Fact]
    public void ImplicitOperatorFromBasicColor_CreatesColor()
    {
        Color color = BasicColor.Red;

        Assert.Equal(BasicColor.Red, color.Basic);
        Assert.Null(color.Full);
    }

    [Fact]
    public void ImplicitOperatorFromFullColor_CreatesColor()
    {
        FullColor fullColor = new(0.25f, 0.5f, 0.75f);
        Color color = fullColor;

        _ = Assert.NotNull(color.Full);
        Assert.Equal(0.25f, color.Full.Value.R);
        Assert.Equal(0.5f, color.Full.Value.G);
        Assert.Equal(0.75f, color.Full.Value.B);
    }

    [Fact]
    public void ImplicitOperatorFromTuple_CreatesColor()
    {
        Color color = (0.1f, 0.5f, 0.9f);

        _ = Assert.NotNull(color.Full);
        Assert.Equal(0.1f, color.Full.Value.R);
        Assert.Equal(0.5f, color.Full.Value.G);
        Assert.Equal(0.9f, color.Full.Value.B);
    }
}

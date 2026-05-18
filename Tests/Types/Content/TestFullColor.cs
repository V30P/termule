using Termule.Engine.Types;

namespace Termule.Tests.Types.Content;

public class TestFullColor
{
    [Fact]
    public void Constructor_WhenAssigningComponentsToBoundaryValues_Functions()
    {
        FullColor black = new(0, 0, 0);
        Assert.Equal(0, black.R);
        Assert.Equal(0, black.G);
        Assert.Equal(0, black.B);

        FullColor white = new(1f, 1f, 1f);
        Assert.Equal(1f, white.R);
        Assert.Equal(1f, white.G);
        Assert.Equal(1f, white.B);
    }

    [Fact]
    public void Constructor_WhenBlueOutOfRange_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, 0, 1.1f));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, 0, -0.1f));
    }

    [Fact]
    public void Constructor_WhenGreenOutOfRange_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, 1.1f, 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, -0.1f, 0));
    }

    [Fact]
    public void Constructor_WhenRedOutOfRange_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(1.1f, 0, 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(-0.1f, 0, 0));
    }

    [Fact]
    public void ImplicitOperatorFromTuple_CreatesFullColor()
    {
        FullColor color = (0.33f, 0.66f, 0.99f);

        Assert.Equal(0.33f, color.R);
        Assert.Equal(0.66f, color.G);
        Assert.Equal(0.99f, color.B);
    }
}

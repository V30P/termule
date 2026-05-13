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

        FullColor white = new(255, 255, 255);
        Assert.Equal(255, white.R);
        Assert.Equal(255, white.G);
        Assert.Equal(255, white.B);
    }

    [Fact]
    public void Constructor_WhenBlueOutOfRange_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, 0, 256));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, 0, -1));
    }

    [Fact]
    public void Constructor_WhenGreenOutOfRange_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, 256, 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(0, -1, 0));
    }

    [Fact]
    public void Constructor_WhenRedOutOfRange_Throws()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(256, 0, 0));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new FullColor(-1, 0, 0));
    }
}
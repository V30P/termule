using Termule.Engine.Types;

namespace Termule.Tests.Types;

public class TestConnections
{
    [Fact]
    public void FromChar_ReturnsCorrectConnections()
    {
        Assert.Equal(Connections.Up, Connections.FromChar('╵'));
        Assert.Equal(Connections.Right, Connections.FromChar('╶'));
        Assert.Equal(Connections.Up | Connections.Down, Connections.FromChar('│'));
    }

    [Fact]
    public void FromChar_UnmappedChar_ReturnsNone()
    {
        Assert.Equal(Connections.None, Connections.FromChar('a'));
    }

    [Fact]
    public void ToChar_ReturnsCorrectChars()
    {
        Assert.Equal('╵', Connections.Up.ToChar());
        Assert.Equal('╶', Connections.Right.ToChar());
        Assert.Equal('╷', Connections.Down.ToChar());
        Assert.Equal('╴', Connections.Left.ToChar());
        Assert.Equal('│', (Connections.Up | Connections.Down).ToChar());
        Assert.Equal('─', (Connections.Right | Connections.Left).ToChar());
    }
}
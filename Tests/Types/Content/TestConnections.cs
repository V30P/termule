using Termule.Engine.Types;

namespace Termule.Tests.Types;

public class TestConnections
{
    [Fact]
    public void FromChar_ReturnsCorrectConnections()
    {
        Assert.Equal(Connections.Up, ConnectionsExtensions.FromChar('╵'));
        Assert.Equal(Connections.Right, ConnectionsExtensions.FromChar('╶'));
        Assert.Equal(Connections.Up | Connections.Down, ConnectionsExtensions.FromChar('│'));
    }

    [Fact]
    public void FromChar_UnmappedChar_ReturnsNone()
    {
        Assert.Equal(Connections.None, ConnectionsExtensions.FromChar('a'));
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

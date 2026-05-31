using Termule.Engine.Types;

namespace Termule.Tests.Types;

public class TestConnections
{
    [Fact]
    public void FromChar_ReturnsCorrectConnections()
    {
        Assert.Equal(Connections.Up, ConnectionsExtensions.FromGlyph('╵'));
        Assert.Equal(Connections.Right, ConnectionsExtensions.FromGlyph('╶'));
        Assert.Equal(Connections.Up | Connections.Down, ConnectionsExtensions.FromGlyph('│'));
    }

    [Fact]
    public void FromChar_UnmappedChar_ReturnsNone()
    {
        Assert.Equal(Connections.None, ConnectionsExtensions.FromGlyph('a'));
    }

    [Fact]
    public void ToChar_ReturnsCorrectChars()
    {
        Assert.Equal('╵', Connections.Up.ToGlyph());
        Assert.Equal('╶', Connections.Right.ToGlyph());
        Assert.Equal('╷', Connections.Down.ToGlyph());
        Assert.Equal('╴', Connections.Left.ToGlyph());
        Assert.Equal('│', (Connections.Up | Connections.Down).ToGlyph());
        Assert.Equal('─', (Connections.Right | Connections.Left).ToGlyph());
    }
}

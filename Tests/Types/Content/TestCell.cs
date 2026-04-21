using Termule.Engine.Types;

namespace Termule.Tests.Types;

public class TestCell
{
    [Fact]
    public void ParameterlessConstructor_CreatesDefaultCell()
    {
        Cell cell = new();

        Assert.Equal(default, cell.Color);
        Assert.Equal('\0', cell.Char);
        Assert.Equal(default, cell.CharColor);
    }

    [Fact]
    public void ValueConstructor_SetsProperties()
    {
        Color bg = BasicColor.Red;
        Color fg = BasicColor.Blue;

        Cell cell = new(bg, 'A', fg);

        Assert.Equal(bg, cell.Color);
        Assert.Equal('A', cell.Char);
        Assert.Equal(fg, cell.CharColor);
    }

    [Fact]
    public void Properties_AreSettable()
    {
        Cell cell = new();
        Color bg = BasicColor.Green;
        Color fg = BasicColor.Yellow;

        cell.Color = bg;
        cell.Char = 'B';
        cell.CharColor = fg;

        Assert.Equal(bg, cell.Color);
        Assert.Equal('B', cell.Char);
        Assert.Equal(fg, cell.CharColor);
    }

    [Fact]
    public void EqualityOperator_ComparesAllFields()
    {
        Cell c1 = new(BasicColor.Red, 'X', BasicColor.Blue);
        Cell c2 = new(BasicColor.Red, 'X', BasicColor.Blue);
        Cell c3 = new(BasicColor.Green, 'X', BasicColor.Blue);

        Assert.True(c1 == c2);
        Assert.False(c1 == c3);
    }

    [Fact]
    public void InequalityOperator_ComparesAllFields()
    {
        Cell c1 = new(BasicColor.Red, 'X', BasicColor.Blue);
        Cell c2 = new(BasicColor.Green, 'X', BasicColor.Blue);

        Assert.True(c1 != c2);
    }

    [Fact]
    public void Equals_ComparesAllFields()
    {
        Cell c1 = new(BasicColor.Red, 'X', BasicColor.Blue);
        Cell c2 = new(BasicColor.Red, 'X', BasicColor.Blue);
        Cell c3 = new(BasicColor.Green, 'X', BasicColor.Blue);

        Assert.True(c1.Equals(c2));
        Assert.False(c1.Equals(c3));
    }

    [Fact]
    public void GetHashCode_IsConsistent()
    {
        Cell c1 = new(BasicColor.Red, 'X', BasicColor.Blue);
        Cell c2 = new(BasicColor.Red, 'X', BasicColor.Blue);

        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
    }
}
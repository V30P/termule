using Termule.Engine.Systems.Resources;
using Termule.Engine.Types;

namespace Termule.Tests.Types.Content;

public class TestImage
{
    [Fact]
    public void ConstructorWithDimensions_CreatesImageWithProperDimensions()
    {
        Image image = new(10, 5);

        Assert.Equal(10, image.Size.X);
        Assert.Equal(5, image.Size.Y);
    }

    [Fact]
    public void ConstructorWithDimensions_GivenNegativeDimension_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Image(-1, 5));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Image(10, -1));
    }

    [Fact]
    public void CopyConstructor_DuplicatesCells()
    {
        Image original = new(2, 2);

        original[0, 0] = new Cell(BasicColor.Red, 'A', BasicColor.Blue);
        original[1, 1] = new Cell(BasicColor.Green, 'B', BasicColor.Yellow);

        Image copy = new(original);

        Assert.Equal(original.Size, copy.Size);
        Assert.Equal(original[0, 0], copy[0, 0]);
        Assert.Equal(original[1, 1], copy[1, 1]);
        Assert.NotSame(original, copy);
    }

    [Fact]
    public void GetIndexer_ReturnsCell()
    {
        Image image = new(3, 3);
        Cell cell = new(BasicColor.Red, 'X', BasicColor.Blue);
        image[1, 2] = cell;

        Assert.Equal(cell, image[1, 2]);
    }

    [Fact]
    public void SetIndexer_SetsCell()
    {
        Image image = new(3, 3);
        Cell cell = new(BasicColor.Green, 'Y', BasicColor.Cyan);

        image[2, 1] = cell;
        
        Assert.Equal(cell, image[2, 1]);
    }
}
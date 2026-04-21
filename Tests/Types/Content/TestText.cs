using Termule.Engine.Types;

namespace Termule.Tests.Types.Content;

public class TestText
{
    [Fact]
    public void ParameterlessConstructor_CreatesEmptyText()
    {
        Text text = new();

        Assert.Equal((0, 0), ((IContent)text).Size);
        Assert.Null(text.Value);
        Assert.Equal(default, text.Color);
    }

    [Fact]
    public void SettingValue_ToSingleLine_RecalculatesSizeAndCells()
    {
        Text text = new() { Value = "Hello" };

        Assert.Equal((5, 1), ((IContent)text).Size);
        Assert.Equal('H', ((IContent)text)[0, 0].Char);
        Assert.Equal('e', ((IContent)text)[1, 0].Char);
        Assert.Equal('l', ((IContent)text)[2, 0].Char);
        Assert.Equal('l', ((IContent)text)[3, 0].Char);
        Assert.Equal('o', ((IContent)text)[4, 0].Char);
    }

    [Fact]
    public void SettingValue_ToMultiLine_RecalculateSizeAndCells()
    {
        Text text = new() { Value = "Hi\nWorld" };

        Assert.Equal((5, 2), ((IContent)text).Size);
        Assert.Equal('H', ((IContent)text)[0, 0].Char);
        Assert.Equal('i', ((IContent)text)[1, 0].Char);
        Assert.Equal('W', ((IContent)text)[0, 1].Char);
        Assert.Equal('o', ((IContent)text)[1, 1].Char);
        Assert.Equal('r', ((IContent)text)[2, 1].Char);
        Assert.Equal('l', ((IContent)text)[3, 1].Char);
        Assert.Equal('d', ((IContent)text)[4, 1].Char);
    }

    [Fact]
    public void SettingColor_AppliesColorToChars()
    {
        Text text = new() { Color = BasicColor.Red, Value = "Test" };

        Assert.Equal(BasicColor.Red, ((IContent)text)[0, 0].CharColor);
        Assert.Equal(BasicColor.Red, ((IContent)text)[1, 0].CharColor);
        Assert.Equal(BasicColor.Red, ((IContent)text)[2, 0].CharColor);
        Assert.Equal(BasicColor.Red, ((IContent)text)[3, 0].CharColor);
    }

    [Fact]
    public void SettingValue_ToNull_ResetsCells()
    {
        Text text = new() { Value = "Hello" };

        text.Value = null;

        Assert.Equal((0, 0), ((IContent)text).Size);
        Assert.Null(text.Value);
    }

    [Fact]
    public void SettingValue_ToEmpty_ResetsCells()
    {
        Text text = new() { Value = "Hello" };
        text.Value = "";
        Assert.Equal((0, 0), ((IContent)text).Size);
        Assert.Equal("", text.Value);
    }


    [Fact]
    public void GetIndexer_ReturnsCellValue()
    {
        Text text = new() { Value = "Hi" };

        Assert.Equal(new Cell { Char = 'H' }, ((IContent)text)[0, 0]);
    }
    
    [Fact]
    public void GetIndexer_ForFillerPositions_ReturnsDefaultCell()
    {
        Text text = new() { Value = "AB\nC" };

        Assert.Equal(default, ((IContent)text)[1, 1]);   
    }

    [Fact]
    public void GetIndexer_OutOfBounds_Throws()
    {
        Text text = new() { Value = "Hi" };

        Assert.Throws<IndexOutOfRangeException>(() => ((IContent)text)[3, 0]);
        Assert.Throws<IndexOutOfRangeException>(() => ((IContent)text)[0, 1]);
    }

    [Fact]
    public void SettingColor_AfterValueSet_RecolorsExistingCells()
    {
        Text text = new() { Value = "AB" };
        Assert.Equal(default, ((IContent)text)[0, 0].CharColor);

        text.Color = BasicColor.Red;
        Assert.Equal(BasicColor.Red, ((IContent)text)[0, 0].CharColor);
    }

    [Fact]
    public void SettingValue_ToLongerLine_UpdatesWidth()
    {
        Text text = new() { Value = "Short" };
        Assert.Equal(5, ((IContent)text).Size.X);

        text.Value = "Much longer line";
        Assert.Equal(16, ((IContent)text).Size.X);
    }
}
namespace Termule.Types;

public sealed class Image : Content
{
    public Image(int width, int height)
        : base(width, height)
    {
    }

    public Image(Content content)
        : base(content)
    {
    }

    public Cell this[int x, int y]
    {
        get => this.Cells[x, y];
        set => this.Cells[x, y] = value;
    }
}

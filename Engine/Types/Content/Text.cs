using System.Diagnostics;

namespace Termule.Engine.Types;

/// <summary>
///     Content that represents textual content.
/// </summary>
public sealed class Text : IContent
{
    private Cell[][] cells = [];
    private VectorInt size = (0, 0);
    
    /// <summary>
    ///     Gets or sets the string of characters for this text.
    /// </summary>
    public string Value
    {
        get;

        set
        {
            if (field == value)
            {
                return;
            }
            
            field = value;
            if (field == null)
            {
                cells = [];
                size = (0, 0);

                return;
            }
            
            string[] lines = field.Split('\n');
            size = size with { Y = lines.Length };
            
            cells = new Cell[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                cells[i] = line.Select(c => new Cell(default, c, Color)).ToArray();

                if (line.Length > size.X)
                {
                    size = size with { X = line.Length };
                }
            }
        }
    }

    /// <summary>
    ///     Gets or sets the color of this text's characters.
    /// </summary>
    public Color Color { get; set; }

    VectorInt IContent.Size => size;
    
    Cell IContent.this[int x, int y] => cells[y].Length > x ? cells[y][x] : default;
}
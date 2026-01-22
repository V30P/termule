using Termule.Types;
using Termule.Components;

namespace Termule.Systems.RenderSystem;

internal sealed class Frame : Content
{
    internal readonly HashSet<Renderer>[,] Contributors;
    internal readonly Dictionary<Renderer, HashSet<VectorInt>> Contributions = [];

    public Cell this[int x, int y] => Cells[x, y];

    internal Frame(int x, int y) : base(x, y)
    {
        Contributors = new HashSet<Renderer>[Size.X, Size.Y];
    }

    internal void Contribute(Renderer renderer, VectorInt pos, Color? color = null, char? character = null, Color? characterColor = null)
    {
        Cell cell = Cells[pos.X, pos.Y];
        bool madeChange = false;

        // Tracks if a change has been made so we can credit the renderer
        void ChangeCell(Func<Cell, Cell> update)
        {
            cell = update(cell);
            madeChange = true;
        }

        if (color is Color colorValue)
        {
            ChangeCell(_ => new() { Color = colorValue });
        }
        if (character is char characterValue)
        {
            ChangeCell(c => c with { Char = characterValue, CharColor = Color.Default });
        }
        if (characterColor is Color characterColorValue)
        {
            ChangeCell(c => c with { CharColor = characterColorValue });
        }

        if (madeChange)
        {
            Cells[pos.X, pos.Y] = cell;
            Credit(renderer, pos);
        }
    }

    private void Credit(Renderer renderer, VectorInt pos)
    {
        if (renderer == null)
        {
            return;
        }

        if (Contributors[pos.X, pos.Y] is not HashSet<Renderer> contributors)
        {
            Contributors[pos.X, pos.Y] = contributors = [];
        }
        contributors.Add(renderer);

        if (!Contributions.TryGetValue(renderer, out HashSet<VectorInt> contributions))
        {
            contributions = [];
            Contributions.Add(renderer, contributions);
        }
        contributions.Add(pos);
    }
}
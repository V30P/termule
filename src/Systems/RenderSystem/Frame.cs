namespace Termule.Systems.RenderSystem;

using Types;
using Components;

public sealed class Frame : Content
{
    internal Frame(Content background)
    : base(background)
    {
        this.Contributors = new HashSet<Renderer>[this.Size.X, this.Size.Y];
    }

    internal HashSet<Renderer>[,] Contributors { get; }

    internal Dictionary<Renderer, HashSet<VectorInt>> Contributions { get; } = [];

    public Cell this[int x, int y] => this.Cells[x, y];

    public void Contribute(Renderer renderer, VectorInt pos, Color? color = null, char? character = null, Color? characterColor = null)
    {
        ArgumentNullException.ThrowIfNull(renderer);
        if (pos.X < 0 || pos.X >= this.Size.X || pos.Y < 0 || pos.Y >= this.Size.Y)
        {
            throw new ArgumentOutOfRangeException(nameof(pos), pos, "Contribution must be within the bounds of the Frame");
        }

        Cell cell = this.Cells[pos.X, pos.Y];
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

#pragma warning disable SA1101 // Prefix local calls with this
        if (character is char characterValue)
        {
            ChangeCell(c => c with { Char = characterValue, CharColor = BasicColor.Default });
        }

        if (characterColor is Color characterColorValue)
        {
            ChangeCell(c => c with { CharColor = characterColorValue });
#pragma warning restore SA1101 // Prefix local calls with this
        }

        if (madeChange)
        {
            this.Cells[pos.X, pos.Y] = cell;
            this.Credit(renderer, pos);
        }
    }

    private void Credit(Renderer renderer, VectorInt pos)
    {
        if (this.Contributors[pos.X, pos.Y] is not HashSet<Renderer> contributors)
        {
            this.Contributors[pos.X, pos.Y] = contributors = [];
        }

        contributors.Add(renderer);

        if (!this.Contributions.TryGetValue(renderer, out HashSet<VectorInt> contributions))
        {
            contributions = [];
            this.Contributions.Add(renderer, contributions);
        }

        contributions.Add(pos);
    }
}
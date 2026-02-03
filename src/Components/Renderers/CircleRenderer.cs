namespace Termule.Components;

using Systems.RenderSystem;
using Types;

public sealed class CircleRenderer : TransformRenderer
{
    public Color Color { get; set; }

    public bool Filled { get; set; }

    public bool DoubleWide { get; set; }

    public float Radius
    {
        get;

        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(this.Radius), value, "Radius cannot be negative");
            }

            field = value;
        }
    }

    private protected override void Render(Frame frame, VectorInt framespacePos)
    {
        IEnumerable<VectorInt> positions = GetCirclePositions(this.Radius, this.Filled);
        if (this.DoubleWide)
        {
            positions = DoubleUp(positions, framespacePos);
        }

        positions = positions.Select(p => p + framespacePos);
        foreach (VectorInt pos in positions)
        {
            if ((uint)pos.X < frame.Size.X && (uint)pos.Y < frame.Size.Y)
            {
                frame.Contribute(this, pos, this.Color);
            }
        }
    }

    private static HashSet<VectorInt> GetCirclePositions(float radius, bool filled)
    {
        HashSet<VectorInt> positions = [];
        foreach (VectorInt pos in MidpointCircle((int)radius))
        {
            positions.Add((pos.X, pos.Y));
            positions.Add((pos.X, -pos.Y));
            positions.Add((pos.Y, -pos.X));
            positions.Add((-pos.Y, -pos.X));
            positions.Add((-pos.X, -pos.Y));
            positions.Add((-pos.X, pos.Y));
            positions.Add((-pos.Y, pos.X));
            positions.Add((pos.Y, pos.X));

            if (filled)
            {
                for (int x = -pos.X + 1; x < pos.X; x++)
                {
                    positions.Add((x, pos.Y));
                    positions.Add((x, -pos.Y));
                }

                for (int x = -pos.Y + 1; x < pos.Y; x++)
                {
                    positions.Add((x, pos.X));
                    positions.Add((x, -pos.X));
                }
            }
        }

        return positions;
    }

    // Generates pixel positions for one octant of a circle with given radius
    private static List<VectorInt> MidpointCircle(int radius)
    {
        List<VectorInt> positions = [];

        int y = radius;
        int p = 1 - radius;
        for (int x = 0; x <= y; x++)
        {
            positions.Add((x, y));
            p += (2 * (p < 0 ? (2 * x) : (x - --y))) + 1;
        }

        return positions;
    }

    private static List<VectorInt> DoubleUp(IEnumerable<VectorInt> positions, Vector center)
    {
        List<VectorInt> widenedPositions = [];

        float fraction = center.X - (float)Math.Truncate(center.X);
        VectorInt offset = fraction > 0.5f ? (1, 0) : (-1, 0);
        foreach (VectorInt position in positions)
        {
            VectorInt widenedPosition = (position.X * 2, position.Y);
            widenedPositions.Add(widenedPosition);
            widenedPositions.Add(widenedPosition + offset);
        }

        return widenedPositions;
    }
}
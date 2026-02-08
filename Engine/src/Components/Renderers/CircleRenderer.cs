namespace Termule.Components;

using Systems.RenderSystem;
using Types;

/// <summary>
/// A <see cref="TransformRenderer"/> implementation to render a circle.
/// </summary>
public sealed class CircleRenderer : TransformRenderer
{
    /// <summary>
    /// Gets or sets the color to render the circle.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all cells inside the circle should be filled.
    /// </summary>
    public bool Filled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether all circle cells should be duplicated horizontally.
    /// </summary>
    /// <remarks>
    /// This is useful for making more round circles in the terminal.
    /// </remarks>
    public bool DoubleWide { get; set; }

    /// <summary>
    /// Gets or sets the radius of circle to render.
    /// </summary>
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

    /// <summary>
    /// Computes the integer positions that form a circle of the given radius (centered at the origin).
    /// If <paramref name="filled"/> is true the interior of the circle is included.
    /// </summary>
    /// <param name="radius">The circle radius.</param>
    /// <param name="filled">Whether interior points should be included.</param>
    /// <returns>A set of integer positions that form the circle (and interior when <paramref name="filled"/> is true).</returns>
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

    /// <summary>
    /// Generates pixel positions for one octant of a circle with the given radius.
    /// </summary>
    /// <param name="radius">The radius for the octant computation.</param>
    /// <returns>A list of integer positions for the octant.</returns>
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

    /// <summary>
    /// Expands horizontally to make the shape double-wide, adjusting by the renderer's center subpixel fraction.
    /// </summary>
    /// <param name="positions">The original positions to widen.</param>
    /// <param name="center">The center to use when deciding horizontal offset.</param>
    /// <returns>A new list of widened integer positions.</returns>
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
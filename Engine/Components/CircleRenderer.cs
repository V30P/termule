using Termule.Systems.Display;
using Termule.Types.Content;
using Termule.Types.Vectors;

namespace Termule.Components;

/// <summary>
///     Renders a circle at the local <see cref="Transform" />'s position.
/// </summary>
public sealed class CircleRenderer : TransformRenderer
{
    /// <summary>
    ///     Gets or sets the color to render the circle with.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether all cells inside the circle should be filled.
    /// </summary>
    public bool Filled { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether all circle cells should be duplicated horizontally.
    /// </summary>
    /// <remarks>
    ///     This is useful for making more round circles in the terminal.
    /// </remarks>
    public bool DoubleWide { get; set; }

    /// <summary>
    ///     Gets or sets the radius of the circle to render.
    /// </summary>
    public float Radius
    {
        get;

        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Radius), value, "Radius cannot be negative");
            }

            field = value;
        }
    }

    private protected override void Render(FrameBuffer frame, VectorInt frameSpacePos)
    {
        IEnumerable<VectorInt> positions = GetCirclePositions(Radius, Filled);
        if (DoubleWide)
        {
            positions = DoubleUp(positions, frameSpacePos);
        }

        positions = positions.Select(p => p + frameSpacePos);
        foreach (var pos in positions)
        {
            if ((uint)pos.X < frame.Size.X && (uint)pos.Y < frame.Size.Y)
            {
                frame.Contribute(this, pos, Color);
            }
        }
    }

    private static HashSet<VectorInt> GetCirclePositions(float radius, bool filled)
    {
        HashSet<VectorInt> positions = [];
        foreach (var pos in MidpointCircle((int)radius))
        {
            positions.Add((pos.X, pos.Y));
            positions.Add((pos.X, -pos.Y));
            positions.Add((pos.Y, -pos.X));
            positions.Add((-pos.Y, -pos.X));
            positions.Add((-pos.X, -pos.Y));
            positions.Add((-pos.X, pos.Y));
            positions.Add((-pos.Y, pos.X));
            positions.Add((pos.Y, pos.X));

            if (!filled)
            {
                continue;
            }

            for (var x = -pos.X + 1; x < pos.X; x++)
            {
                positions.Add((x, pos.Y));
                positions.Add((x, -pos.Y));
            }

            for (var x = -pos.Y + 1; x < pos.Y; x++)
            {
                positions.Add((x, pos.X));
                positions.Add((x, -pos.X));
            }
        }

        return positions;
    }

    private static List<VectorInt> MidpointCircle(int radius)
    {
        List<VectorInt> positions = [];

        var y = radius;
        var p = 1 - radius;
        for (var x = 0; x <= y; x++)
        {
            positions.Add((x, y));
            p += 2 * (p < 0 ? 2 * x : x - --y) + 1;
        }

        return positions;
    }

    private static List<VectorInt> DoubleUp(IEnumerable<VectorInt> positions, Vector center)
    {
        List<VectorInt> widenedPositions = [];

        var fraction = center.X - (float)Math.Truncate(center.X);
        VectorInt offset = fraction > 0.5f ? (1, 0) : (-1, 0);
        foreach (var position in positions)
        {
            VectorInt widenedPosition = (position.X * 2, position.Y);
            widenedPositions.Add(widenedPosition);
            widenedPositions.Add(widenedPosition + offset);
        }

        return widenedPositions;
    }
}
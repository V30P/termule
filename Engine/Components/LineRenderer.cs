using Termule.Systems.Display;
using Termule.Types.Content;
using Termule.Types.Vectors;

namespace Termule.Components;

/// <summary>
///     Renders a line or series of lines relative to the local <see cref="Transform" />'s position.
/// </summary>
public sealed class LineRenderer : TransformRenderer
{
    /// <summary>
    ///     Gets or sets the collection of line points relative to this renderer's transform.
    /// </summary>
    public IEnumerable<Vector> Points { get; set; } = [];

    /// <summary>
    ///     Gets or sets the color used to draw the lines.
    /// </summary>
    public Color Color { get; set; }

    private protected override void Render(FrameBuffer frame, VectorInt frameSpacePos)
    {
        if (!Points.Any())
        {
            return;
        }

        var framePoints = Points
            .Select(p => (frameSpacePos + new Vector(p.X, DisplaySpace ? p.Y : -p.Y)).FloorToInt());
        var vectorInts = framePoints as VectorInt[] ?? framePoints.ToArray();
        var lastPoint = vectorInts.First();
        foreach (var point in vectorInts.Skip(1))
        {
            var positions = GetLinePositions(lastPoint, point);
            var visiblePositions = positions
                .Where(pos => (uint)pos.X < frame.Size.X && (uint)pos.Y < frame.Size.Y);
            foreach (var pos in visiblePositions)
            {
                frame.Contribute(this, pos, Color);
            }

            lastPoint = point;
        }
    }

    private static List<VectorInt> GetLinePositions(VectorInt p1, VectorInt p2)
    {
        int dx, dy;
        NormalizeEndpoints();

        // Swap x and y to get rid of steep slopes
        var needsSwapping = MathF.Abs(dy) > dx;
        if (needsSwapping)
        {
            p1 = new VectorInt(p1.Y, p1.X);
            p2 = new VectorInt(p2.Y, p2.X);

            NormalizeEndpoints();
        }

        // Reflect over the x-axis to get rid of negative slopes
        var needsReflection = dy < 0;
        if (needsReflection)
        {
            p1 = p1 with { Y = -p1.Y };
            p2 = p2 with { Y = -p2.Y };

            NormalizeEndpoints();
        }

        var positions = Bresenham(p1, p2);

        // Undo transforms
        if (needsReflection)
        {
            positions = [.. positions.Select(p => new VectorInt(p.X, -p.Y))];
        }

        if (needsSwapping)
        {
            positions = [.. positions.Select(p => new VectorInt(p.Y, p.X))];
        }

        return positions;

        // Makes sure p1 is always left of p2
        void NormalizeEndpoints()
        {
            if (p2.X - p1.X < 0)
            {
                (p1, p2) = (p2, p1);
            }

            dx = p2.X - p1.X;
            dy = p2.Y - p1.Y;
        }
    }

    // Generates pixel positions for the line between two points as long as p1.x < p2.x and 0 <= slope < 1
    private static List<VectorInt> Bresenham(VectorInt p1, VectorInt p2)
    {
        List<VectorInt> positions = [];
        var dx = p2.X - p1.X;
        var dy = p2.Y - p1.Y;

        var p = 2 * dy - dx;
        var y = p1.Y;
        for (var x = p1.X; x <= p2.X; x++)
        {
            positions.Add((x, y));
            if (p > 0)
            {
                y++;
                p += 2 * (dy - dx);
            }
            else
            {
                p += 2 * dy;
            }
        }

        return positions;
    }
}
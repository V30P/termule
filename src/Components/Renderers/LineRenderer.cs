namespace Termule.Components;

using Systems.RenderSystem;
using Types;

public sealed class LineRenderer : TransformRenderer
{
    public IEnumerable<Vector> Points { get; set; } = [];

    public Color Color { get; set; }

    private protected override void Render(Frame frame, VectorInt framespacePos)
    {
        if (!this.Points.Any())
        {
            return;
        }

        IEnumerable<VectorInt> framePoints = this.Points
            .Select(p => (framespacePos + new Vector(p.X, this.ScreenSpace ? p.Y : -p.Y)).FloorToInt());
        VectorInt lastPoint = framePoints.First();
        foreach (VectorInt point in framePoints.Skip(1))
        {
            foreach (VectorInt pos in GetLinePositions(lastPoint, point))
            {
                if ((uint)pos.X < frame.Size.X && (uint)pos.Y < frame.Size.Y)
                {
                    frame.Contribute(this, pos, this.Color);
                }
            }

            lastPoint = point;
        }
    }

    private static List<VectorInt> GetLinePositions(VectorInt p1, VectorInt p2)
    {
        int dx, dy;
        NormalizeEndpoints();

        // Swap x and y to get rid of steep slopes
        bool needsSwapping = MathF.Abs(dy) > dx;
        if (needsSwapping)
        {
            p1 = new VectorInt(p1.Y, p1.X);
            p2 = new VectorInt(p2.Y, p2.X);

            NormalizeEndpoints();
        }

        // Reflect over the x-axis to get rid of negative slopes
        bool needsReflection = dy < 0;
        if (needsReflection)
        {
            p1 = p1 with { Y = -p1.Y };
            p2 = p2 with { Y = -p2.Y };

            NormalizeEndpoints();
        }

        List<VectorInt> positions = Bresenham(p1, p2);

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
        int dx = p2.X - p1.X;
        int dy = p2.Y - p1.Y;

        int p = (2 * dy) - dx;
        int y = p1.Y;
        for (int x = p1.X; x <= p2.X; x++)
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
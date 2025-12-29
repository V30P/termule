namespace Termule.Rendering;

public sealed class LineRenderer : TransformRenderer
{
    public IEnumerable<Vector> Points;
    public Color Color;

    private protected override void Render(Frame frame, VectorInt posInFrame)
    {
        if (Points == null)
        {
            return;
        }

        IEnumerable<VectorInt> framePoints = Points
            .Select(p => (posInFrame + new Vector(p.X, ScreenSpace ? p.Y : -p.Y)).RoundToInt());
        VectorInt lastPoint = framePoints.First();
        foreach (VectorInt point in framePoints.Skip(1))
        {
            foreach (VectorInt cell in GetLineCells(lastPoint, point))
            {
                if ((uint)cell.X < frame.Size.X && (uint)cell.Y < frame.Size.Y)
                {
                    frame.Contribute(this, cell, Color);
                }
            }

            lastPoint = point;
        }
    }

    public static List<VectorInt> GetLineCells(VectorInt p1, VectorInt p2)
    {
        float dx, dy;
        NormalzieEndpoints();

        // Swap x and y to get rid of steep slopes
        bool needsSwapping = MathF.Abs(dy) > dx;
        if (needsSwapping)
        {
            p1 = new VectorInt(p1.Y, p1.X);
            p2 = new VectorInt(p2.Y, p2.X);

            NormalzieEndpoints();
        }

        // Reflect over the x-axis to get rid of negative slopes
        bool needsReflection = dy < 0;
        if (needsReflection)
        {
            p1 = p1 with { Y = -p1.Y };
            p2 = p2 with { Y = -p2.Y };

            NormalzieEndpoints();
        }

        List<VectorInt> cells = Bresenham(p1, p2);

        // Undo transforms
        if (needsReflection)
        {
            cells = [.. cells.Select(p => new VectorInt(p.X, -p.Y))];
        }

        if (needsSwapping)
        {
            cells = [.. cells.Select(p => new VectorInt(p.Y, p.X))];
        }

        return cells;

        // Makes sure p1 is always left of p2
        void NormalzieEndpoints()
        {
            if (p2.X - p1.X < 0)
            {
                (p1, p2) = (p2, p1);
            }

            dx = p2.X - p1.X;
            dy = p2.Y - p1.Y;
        }
    }

    private static List<VectorInt> Bresenham(VectorInt p1, VectorInt p2)
    {
        float slope = ((float)p2.Y - p1.Y) / (p2.X - p1.X);

        List<VectorInt> cells = [];
        float error = 0;
        int y = p1.Y;
        for (int x = p1.X; x <= p2.X; x++)
        {
            cells.Add(new VectorInt(x, y));
            error += slope;

            if (error > 0.5f)
            {
                y++;
                error -= 1f;
            }
        }

        return cells;
    }
}
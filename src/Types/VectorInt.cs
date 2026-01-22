namespace Termule.Types;

public readonly record struct VectorInt(int X = 0, int Y = 0)
{
    public readonly float Magnitude => MathF.Sqrt((X * X) + (Y * Y));
    public readonly Vector Normalized => Magnitude > 0 ? ((Vector)this) / Magnitude : (0, 0);

    public static implicit operator VectorInt((int x, int y) t)
    {
        return new(t.x, t.y);
    }

    public static VectorInt operator +(VectorInt v1, VectorInt v2)
    {
        return new(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static VectorInt operator -(VectorInt v1, VectorInt v2)
    {
        return new(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static VectorInt operator *(VectorInt v, int i)
    {
        return new(v.X * i, v.Y * i);
    }

    public static VectorInt operator /(VectorInt v, int i)
    {
        return new(v.X / i, v.Y / i);
    }

    public static VectorInt operator -(VectorInt v)
    {
        return v * -1;
    }

    public override readonly int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"({X}, {Y})";
    }
}
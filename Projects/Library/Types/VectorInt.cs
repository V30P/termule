namespace Termule;

public struct VectorInt(int x = 0, int y = 0)
{
    public int X = x, Y = y;

    public readonly float Magnitude => MathF.Sqrt((X * X) + (Y * Y));
    public readonly Vector Normalized => Magnitude > 0 ? ((Vector)this) / Magnitude : (0, 0);

    public static implicit operator VectorInt((int x, int y) t)
    {
        return new(t.x, t.y);
    }

    public static explicit operator VectorInt(Vector v)
    {
        return new((int)v.X, (int)v.Y);
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

    public static bool operator ==(VectorInt v1, VectorInt v2)
    {
        return v1.X == v2.X && v1.Y == v2.Y;
    }

    public static bool operator !=(VectorInt v1, VectorInt v2)
    {
        return !(v1 == v2);
    }

    public override readonly bool Equals(object o)
    {
        return base.Equals(o);
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
namespace Termule;

public struct Vector(float x = 0, float y = 0)
{
    public float X = x, Y = y;

    public readonly float Magnitude => MathF.Sqrt((X * X) + (Y * Y));
    public readonly Vector Normalized => Magnitude > 0 ? this / Magnitude : (0, 0);

    public static implicit operator Vector((float x, float y) t)
    {
        return new(t.x, t.y);
    }

    public static implicit operator Vector(VectorInt v)
    {
        return new(v.X, v.Y);
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
        return new(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        return new(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static Vector operator *(Vector v, float f)
    {
        return new(v.X * f, v.Y * f);
    }

    public static Vector operator /(Vector v, float f)
    {
        return new(v.X / f, v.Y / f);
    }

    public static Vector operator -(Vector v)
    {
        return v * -1;
    }

    public static bool operator ==(Vector v1, Vector v2)
    {
        return v1.X == v2.X && v1.Y == v2.Y;
    }

    public static bool operator !=(Vector v1, Vector v2)
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

    public readonly VectorInt RoundToInt()
    {
        return (VectorInt)new Vector(MathF.Round(X), MathF.Round(Y));
    }
}
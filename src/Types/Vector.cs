namespace Termule.Types;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
public readonly record struct Vector(float X = 0, float Y = 0)
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
{
    public readonly float Magnitude => MathF.Sqrt((this.X * this.X) + (this.Y * this.Y));
    public readonly Vector Normalized => this.Magnitude > 0 ? this / this.Magnitude : (0, 0);

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

    public override readonly int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override readonly string ToString()
    {
        return $"({this.X}, {this.Y})";
    }

    public readonly VectorInt RoundToInt()
    {
        return new VectorInt((int)MathF.Round(this.X), (int)MathF.Round(this.Y));
    }

    public readonly VectorInt FloorToInt()
    {
        return new VectorInt((int)MathF.Floor(this.X), (int)MathF.Floor(this.Y));
    }
}
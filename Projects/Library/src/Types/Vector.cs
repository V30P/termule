namespace Termule;

public struct Vector(float x = 0, float y = 0)
{
    public float x = x, y = y;

    public readonly float magnitude => MathF.Sqrt(x * x + y * y);
    public readonly Vector normalized => magnitude > 0 ? this / magnitude : (0, 0);

    public static implicit operator Vector((float x, float y) t) => new Vector(t.x, t.y);
    public static implicit operator Vector(VectorInt v) => new Vector(v.x, v.y);

    public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.x + v2.x, v1.y + v2.y);
    public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.x - v2.x, v1.y - v2.y);
    public static Vector operator *(Vector v, float f) => new Vector(v.x * f, v.y * f);
    public static Vector operator /(Vector v, float f) => new Vector(v.x / f, v.y / f);
    public static Vector operator -(Vector v) => v * -1;

    public static bool operator ==(Vector v1, Vector v2) => v1.x == v2.x && v1.y == v2.y;
    public static bool operator !=(Vector v1, Vector v2) => !(v1 == v2);
    public override readonly bool Equals(object o) => base.Equals(o);
    public override readonly int GetHashCode() => base.GetHashCode();

    public override readonly string ToString() => $"({x}, {y})";

    public readonly VectorInt RoundToInt() => (VectorInt) new Vector(MathF.Round(x), MathF.Round(y));
}
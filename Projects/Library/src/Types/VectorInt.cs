namespace Termule;

public struct VectorInt(int x = 0, int y = 0)
{
    public int x = x, y = y;

    public readonly float magnitude => MathF.Sqrt(x * x + y * y);
    public readonly Vector normalized => magnitude > 0 ? ((Vector) this) / magnitude : (0, 0);

    public static implicit operator VectorInt((int x, int y) t) => new VectorInt(t.x, t.y);
    public static explicit operator VectorInt(Vector v) => new VectorInt((int) v.x, (int) v.y);

    public static VectorInt operator +(VectorInt v1, VectorInt v2) => new VectorInt(v1.x + v2.x, v1.y + v2.y);
    public static VectorInt operator -(VectorInt v1, VectorInt v2) => new VectorInt(v1.x - v2.x, v1.y - v2.y);
    public static VectorInt operator *(VectorInt v, int i) => new VectorInt(v.x * i, v.y * i);
    public static VectorInt operator /(VectorInt v, int i) => new VectorInt(v.x / i, v.y / i);
    public static VectorInt operator -(VectorInt v) => v * -1;

    public static bool operator ==(VectorInt v1, VectorInt v2) => v1.x == v2.x && v1.y == v2.y;
    public static bool operator !=(VectorInt v1, VectorInt v2) => !(v1 == v2);
    public override readonly bool Equals(object o) => base.Equals(o);
    public override readonly int GetHashCode() => base.GetHashCode();

    public override readonly string ToString() => $"({x}, {y})";
}
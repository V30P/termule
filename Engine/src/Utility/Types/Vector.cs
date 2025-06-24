namespace Termule;

public struct Vector
{
    public float x, y;

    public Vector()
    {
        x = y = 0;
    }

    public Vector(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector((float x, float y) t) => new Vector(t.x, t.y);

    public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.x + v2.x, v1.y + v2.y);
    public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.x - v2.x, v1.y - v2.y);
    public static Vector operator *(Vector v, float f) => new Vector(v.x * f, v.y * f);
    public static Vector operator /(Vector v, float f) => new Vector(v.x / f, v.y / f);

    public static Vector operator +(Vector v, (float x, float y) t) => new Vector(v.x + t.x, v.y + t.y);
    public static Vector operator -(Vector v, (float x, float y) t) => new Vector(v.x - t.x, v.y - t.y);

    public override readonly string ToString() => $"({x}, {y})";
    public static float Dot(Vector v1, Vector v2) => v1.x * v2.x + v1.y * v2.y;
}
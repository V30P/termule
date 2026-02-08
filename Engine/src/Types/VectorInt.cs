namespace Termule.Types;

/// <summary>
/// A two-dimensional integer vector.
/// </summary>
/// <param name="X">The X component of the vector.</param>
/// <param name="Y">The Y component of the vector.</param>
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
public readonly record struct VectorInt(int X = 0, int Y = 0)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
{
    /// <summary>
    /// Gets the magnitude (length) of the vector.
    /// </summary>
    public readonly float Magnitude => MathF.Sqrt((this.X * this.X) + (this.Y * this.Y));

    /// <summary>
    /// Gets the normalized (unit-length) vector as a <see cref="Vector"/>.
    /// </summary>
    public readonly Vector Normalized => this.Magnitude > 0 ? ((Vector)this) / this.Magnitude : (0, 0);

    /// <summary>
    /// Creates a <see cref="VectorInt"/> from a tuple of integers.
    /// </summary>
    /// <param name="t">The component values to use.</param>
    public static implicit operator VectorInt((int x, int y) t)
    {
        return new(t.x, t.y);
    }

    /// <summary>
    /// Adds two <see cref="VectorInt"/> values.
    /// </summary>
    /// <param name="v1">The first <see cref="VectorInt"/>.</param>
    /// <param name="v2">The second <see cref="VectorInt"/>.</param>
    /// <returns>The sum as a <see cref="VectorInt"/>.</returns>
    public static VectorInt operator +(VectorInt v1, VectorInt v2)
    {
        return new(v1.X + v2.X, v1.Y + v2.Y);
    }

    /// <summary>
    /// Subtracts one <see cref="VectorInt"/> from another.
    /// </summary>
    /// <param name="v1">The first <see cref="VectorInt"/>.</param>
    /// <param name="v2">The second <see cref="VectorInt"/>.</param>
    /// <returns>The difference as a <see cref="VectorInt"/>.</returns>
    public static VectorInt operator -(VectorInt v1, VectorInt v2)
    {
        return new(v1.X - v2.X, v1.Y - v2.Y);
    }

    /// <summary>
    /// Multiplies a <see cref="VectorInt"/> by a scalar, returning a <see cref="Vector"/>.
    /// </summary>
    /// <param name="v">The <see cref="VectorInt"/> to scale.</param>
    /// <param name="f">The scalar multiplier.</param>
    /// <returns>The scaled <see cref="Vector"/>.</returns>
    public static Vector operator *(VectorInt v, float f)
    {
        return new Vector(v.X * f, v.Y * f);
    }

    /// <summary>
    /// Divides a <see cref="VectorInt"/> by an integer scalar, returning a <see cref="Vector"/>.
    /// </summary>
    /// <param name="v">The <see cref="VectorInt"/> to divide.</param>
    /// <param name="f">The scalar divisor.</param>
    /// <returns>The resulting <see cref="Vector"/>.</returns>
    public static Vector operator /(VectorInt v, int f)
    {
        return new Vector(v.X / f, v.Y / f);
    }

    /// <summary>
    /// Negates a <see cref="VectorInt"/>.
    /// </summary>
    /// <param name="v">The <see cref="VectorInt"/> to negate.</param>
    /// <returns>The negated <see cref="VectorInt"/>.</returns>
    public static VectorInt operator -(VectorInt v)
    {
        return (v.X * -1, v.Y * -1);
    }

    /// <inheritdoc/>
    public override readonly int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        return $"({this.X}, {this.Y})";
    }
}
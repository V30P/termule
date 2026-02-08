namespace Termule.Types;

/// <summary>
/// Represents a two-dimensional vector.
/// </summary>
/// <param name="X">The X component of the vector.</param>
/// <param name="Y">The Y component of the vector.</param>
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
public readonly record struct Vector(float X = 0, float Y = 0)
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
{
    /// <summary>
    /// Gets the magnitude (length) of the vector.
    /// </summary>
    public readonly float Magnitude => MathF.Sqrt((this.X * this.X) + (this.Y * this.Y));

    /// <summary>
    /// Gets the normalized (unit-length) vector.
    /// </summary>
    public readonly Vector Normalized => this.Magnitude > 0 ? this / this.Magnitude : (0, 0);

    /// <summary>
    /// Creates a <see cref="Vector"/> from a tuple of floats.
    /// </summary>
    /// <param name="t">The component values to use.</param>
    public static implicit operator Vector((float x, float y) t)
    {
        return new(t.x, t.y);
    }

    /// <summary>
    /// Creates a <see cref="Vector"/> from a <see cref="VectorInt"/>.
    /// </summary>
    /// <param name="v">The VectorInt to convert.</param>
    public static implicit operator Vector(VectorInt v)
    {
        return new(v.X, v.Y);
    }

    /// <summary>
    /// Adds two <see cref="Vector"/> values.
    /// </summary>
    /// <param name="v1">The first <see cref="Vector"/>.</param>
    /// <param name="v2">The second <see cref="Vector"/>.</param>
    /// <returns>The sum as a <see cref="Vector"/>.</returns>
    public static Vector operator +(Vector v1, Vector v2)
    {
        return new(v1.X + v2.X, v1.Y + v2.Y);
    }

    /// <summary>
    /// Subtracts one <see cref="Vector"/> from another.
    /// </summary>
    /// <param name="v1">The first <see cref="Vector"/>.</param>
    /// <param name="v2">The second <see cref="Vector"/>.</param>
    /// <returns>The difference as a <see cref="Vector"/>.</returns>
    public static Vector operator -(Vector v1, Vector v2)
    {
        return new(v1.X - v2.X, v1.Y - v2.Y);
    }

    /// <summary>
    /// Multiplies a <see cref="Vector"/> by a scalar.
    /// </summary>
    /// <param name="v">The <see cref="Vector"/> to scale.</param>
    /// <param name="f">The scalar multiplier.</param>
    /// <returns>The scaled <see cref="Vector"/>.</returns>
    public static Vector operator *(Vector v, float f)
    {
        return new(v.X * f, v.Y * f);
    }

    /// <summary>
    /// Divides a <see cref="Vector"/> by a scalar.
    /// </summary>
    /// <param name="v">The <see cref="Vector"/> to divide.</param>
    /// <param name="f">The scalar divisor.</param>
    /// <returns>The resulting <see cref="Vector"/>.</returns>
    public static Vector operator /(Vector v, float f)
    {
        return new(v.X / f, v.Y / f);
    }

    /// <summary>
    /// Negates a <see cref="Vector"/>.
    /// </summary>
    /// <param name="v">The <see cref="Vector"/> to negate.</param>
    /// <returns>The negated <see cref="Vector"/>.</returns>
    public static Vector operator -(Vector v)
    {
        return v * -1;
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

    /// <summary>
    /// Rounds this vector to a <see cref="VectorInt"/> by rounding each component to the nearest integer.
    /// </summary>
    /// <returns>The rounded <see cref="VectorInt"/>.</returns>
    public readonly VectorInt RoundToInt()
    {
        return new VectorInt((int)MathF.Round(this.X), (int)MathF.Round(this.Y));
    }

    /// <summary>
    /// Floors this vector to a <see cref="VectorInt"/> by applying <see cref="MathF.Floor"/> to each component.
    /// </summary>
    /// <returns>The floored <see cref="VectorInt"/>.</returns>
    public readonly VectorInt FloorToInt()
    {
        return new VectorInt((int)MathF.Floor(this.X), (int)MathF.Floor(this.Y));
    }
}
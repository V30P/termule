namespace Termule.Engine.Types.Vectors;

/// <summary>
///     Represents a two-dimensional vector.
/// </summary>
/// <param name="X">The X component of the vector.</param>
/// <param name="Y">The Y component of the vector.</param>
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
public readonly record struct Vector(float X = 0, float Y = 0)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
{
    /// <summary>
    ///     Gets the magnitude (length) of the vector.
    /// </summary>
    public float Magnitude => MathF.Sqrt((X * X) + (Y * Y));

    /// <summary>
    ///     Gets the normalized (unit-length) vector.
    /// </summary>
    public Vector Normalized => Magnitude > 0 ? this / Magnitude : (0, 0);

    /// <summary>
    ///     Creates a <see cref="Vector" /> from a tuple of floats.
    /// </summary>
    /// <param name="t">The component values to use.</param>
    public static implicit operator Vector((float x, float y) t)
    {
        return new Vector(t.x, t.y);
    }

    /// <summary>
    ///     Creates a <see cref="Vector" /> from a <see cref="VectorInt" />.
    /// </summary>
    /// <param name="v">The integer vector to convert.</param>
    public static implicit operator Vector(VectorInt v)
    {
        return new Vector(v.X, v.Y);
    }

    /// <summary>
    ///     Adds two vectors.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The sum as a vector.</returns>
    public static Vector operator +(Vector v1, Vector v2)
    {
        return new Vector(v1.X + v2.X, v1.Y + v2.Y);
    }

    /// <summary>
    ///     Subtracts one vector from another.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The difference as a vector.</returns>
    public static Vector operator -(Vector v1, Vector v2)
    {
        return new Vector(v1.X - v2.X, v1.Y - v2.Y);
    }

    /// <summary>
    ///     Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="v">The vector to scale.</param>
    /// <param name="f">The scalar multiplier.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector operator *(Vector v, float f)
    {
        return new Vector(v.X * f, v.Y * f);
    }

    /// <summary>
    ///     Divides a vector by a scalar.
    /// </summary>
    /// <param name="v">The vector to divide.</param>
    /// <param name="f">The scalar divisor.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector operator /(Vector v, float f)
    {
        return new Vector(v.X / f, v.Y / f);
    }

    /// <summary>
    ///     Negates a vector.
    /// </summary>
    /// <param name="v">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    public static Vector operator -(Vector v)
    {
        return v * -1;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    /// <summary>
    ///     Rounds this vector to a <see cref="VectorInt" /> by rounding each component to the nearest integer.
    /// </summary>
    /// <returns>The rounded <see cref="VectorInt" />.</returns>
    public VectorInt RoundToInt()
    {
        return new VectorInt((int)MathF.Round(X), (int)MathF.Round(Y));
    }

    /// <summary>
    ///     Floors this vector to a <see cref="VectorInt" /> by applying <see cref="MathF.Floor" /> to each component.
    /// </summary>
    /// <returns>The floored <see cref="VectorInt" />.</returns>
    public VectorInt FloorToInt()
    {
        return new VectorInt((int)MathF.Floor(X), (int)MathF.Floor(Y));
    }
}
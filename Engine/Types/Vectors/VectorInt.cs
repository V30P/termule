namespace Termule.Engine.Types;

/// <summary>
///     Two-dimensional integer vector.
/// </summary>
/// <param name="X">The X component of the vector.</param>
/// <param name="Y">The Y component of the vector.</param>
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
public readonly record struct VectorInt(int X = 0, int Y = 0)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
{
    /// <summary>
    ///     Gets the magnitude (length) of the vector.
    /// </summary>
    public float Magnitude => MathF.Sqrt(X * X + Y * Y);

    /// <summary>
    ///     Gets the normalized (unit-length) vector as a <see cref="Vector" />.
    /// </summary>
    public Vector Normalized => Magnitude > 0 ? (Vector)this / Magnitude : (0, 0);

    /// <summary>
    ///     Creates a <see cref="VectorInt" /> from a tuple of integers.
    /// </summary>
    /// <param name="t">The component values to use.</param>
    public static implicit operator VectorInt((int x, int y) t)
    {
        return new VectorInt(t.x, t.y);
    }

    /// <summary>
    ///     Adds two integer vectors.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The sum as an integer vector.</returns>
    public static VectorInt operator +(VectorInt v1, VectorInt v2)
    {
        return new VectorInt(v1.X + v2.X, v1.Y + v2.Y);
    }

    /// <summary>
    ///     Subtracts one integer vector from another.
    /// </summary>
    /// <param name="v1">The first vector.</param>
    /// <param name="v2">The second vector.</param>
    /// <returns>The difference as an integer vector.</returns>
    public static VectorInt operator -(VectorInt v1, VectorInt v2)
    {
        return new VectorInt(v1.X - v2.X, v1.Y - v2.Y);
    }

    /// <summary>
    ///     Multiplies an integer vector by a scalar, returning a <see cref="Vector" />.
    /// </summary>
    /// <param name="v">The integer vector to scale.</param>
    /// <param name="f">The scalar multiplier.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector operator *(VectorInt v, float f)
    {
        return new Vector(v.X * f, v.Y * f);
    }

    /// <summary>
    ///     Divides an integer vector by a scalar, returning a <see cref="Vector" />.
    /// </summary>
    /// <param name="v">The integer vector to divide.</param>
    /// <param name="f">The scalar divisor.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector operator /(VectorInt v, int f)
    {
        // ReSharper disable PossibleLossOfFraction
        return new Vector(v.X / f, v.Y / f);
        // ReSharper restore PossibleLossOfFraction
    }

    /// <summary>
    ///     Negates an integer vector.
    /// </summary>
    /// <param name="v">The vector to negate.</param>
    /// <returns>The negated integer vector.</returns>
    public static VectorInt operator -(VectorInt v)
    {
        return (v.X * -1, v.Y * -1);
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
}
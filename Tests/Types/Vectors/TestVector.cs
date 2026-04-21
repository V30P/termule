using Termule.Engine.Types;

namespace Termule.Tests.Types.Vectors;

public class TestVector
{
    [Fact]
    public void ParameterlessConstructor_CreatesZeroVector()
    {
        Vector v = new();

        Assert.Equal(0, v.X);
        Assert.Equal(0, v.Y);
    }

    [Fact]
    public void ValueConstructor_SetsComponents()
    {
        Vector v = new(3.5f, -2.1f);

        Assert.Equal(3.5f, v.X);
        Assert.Equal(-2.1f, v.Y);
    }

    [Fact]
    public void Magnitude_ReturnsCorrectLength()
    {
        Vector v = new(3, 4);

        Assert.Equal(5, v.Magnitude);
    }

    [Fact]
    public void Magnitude_OfZeroVector_ReturnsZero()
    {
        Vector v = new(0, 0);

        Assert.Equal(0, v.Magnitude);
    }

    [Fact]
    public void Normalized_ReturnsUnitVector()
    {
        Vector v = new(3, 4);

        Assert.Equal(1, v.Normalized.Magnitude, 0.0001f);
        Assert.Equal(0.6f, v.Normalized.X, 0.0001f);
        Assert.Equal(0.8f, v.Normalized.Y, 0.0001f);
    }

    [Fact]
    public void Normalized_ZeroVector_ReturnsZeroVector()
    {
        Vector v = new(0, 0);

        Assert.Equal(0, v.Normalized.X);
        Assert.Equal(0, v.Normalized.Y);
    }

    [Fact]
    public void ImplicitOperatorFromTuple_CreatesVector()
    {
        Vector v = (1.5f, -2.5f);

        Assert.Equal(1.5f, v.X);
        Assert.Equal(-2.5f, v.Y);
    }

    [Fact]
    public void ImplicitOperatorFromVectorInt_CreatesVector()
    {
        VectorInt vi = new(3, -4);
        Vector v = vi;

        Assert.Equal(3f, v.X);
        Assert.Equal(-4f, v.Y);
    }

    [Fact]
    public void AddOperator_AddsComponents()
    {
        Vector v1 = new(1, 2);
        Vector v2 = new(3, 4);

        Vector result = v1 + v2;

        Assert.Equal(4, result.X);
        Assert.Equal(6, result.Y);
    }

    [Fact]
    public void SubtractOperator_SubtractsComponents()
    {
        Vector v1 = new(5, 7);
        Vector v2 = new(2, 3);

        Vector result = v1 - v2;

        Assert.Equal(3, result.X);
        Assert.Equal(4, result.Y);
    }

    [Fact]
    public void MultiplyOperator_ScalesVector()
    {
        Vector v = new(2, 3);

        Vector result = v * 2.5f;

        Assert.Equal(5, result.X);
        Assert.Equal(7.5f, result.Y);
    }

    [Fact]
    public void DivideOperator_ScalesVector()
    {
        Vector v = new(6, 8);

        Vector result = v / 2;

        Assert.Equal(3, result.X);
        Assert.Equal(4, result.Y);
    }

    [Fact]
    public void NegateOperator_NegatesComponents()
    {
        Vector v = new(3, -5);

        Vector result = -v;

        Assert.Equal(-3, result.X);
        Assert.Equal(5, result.Y);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        Vector v = new(1.5f, -2.5f);

        Assert.Equal("(1.5, -2.5)", v.ToString());
    }

    [Fact]
    public void RoundToInt_ReturnsRoundedVectorInt()
    {
        Vector v = new(1.6f, -2.4f);

        VectorInt result = v.RoundToInt();

        Assert.Equal(2, result.X);
        Assert.Equal(-2, result.Y);
    }

    [Fact]
    public void FloorToInt_ReturnsFlooredVectorInt()
    {
        Vector v = new(1.9f, -2.1f);

        VectorInt result = v.FloorToInt();

        Assert.Equal(1, result.X);
        Assert.Equal(-3, result.Y);
    }

    [Fact]
    public void GetHashCode_IsConsistent()
    {
        Vector v1 = new(1, 2);
        Vector v2 = new(1, 2);

        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void Equality_WorksCorrectly()
    {
        Vector v1 = new(1, 2);
        Vector v2 = new(1, 2);
        Vector v3 = new(2, 1);
        
        Assert.Equal(v1, v2);
        Assert.NotEqual(v1, v3);
    }
}
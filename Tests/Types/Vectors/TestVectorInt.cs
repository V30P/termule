using Termule.Engine.Types;

namespace Termule.Tests.Types.Vectors;

public class TestVectorInt
{
    [Fact]
    public void ParameterlessConstructor_CreatesZeroVector()
    {
        VectorInt v = new();

        Assert.Equal(0, v.X);
        Assert.Equal(0, v.Y);
    }

    [Fact]
    public void ValueConstructor_SetsComponents()
    {
        VectorInt v = new(3, -2);

        Assert.Equal(3, v.X);
        Assert.Equal(-2, v.Y);
    }

    [Fact]
    public void Magnitude_ReturnsCorrectLength()
    {
        VectorInt v = new(3, 4);

        Assert.Equal(5, v.Magnitude);
    }

    [Fact]
    public void Normalized_ReturnsUnitVector()
    {
        VectorInt v = new(3, 4);

        Assert.Equal(1, v.Normalized.Magnitude, 0.0001f);
        Assert.Equal(0.6f, v.Normalized.X, 0.0001f);
        Assert.Equal(0.8f, v.Normalized.Y, 0.0001f);
    }

    [Fact]
    public void Normalized_ZeroVector_ReturnsZeroVector()
    {
        VectorInt v = new(0, 0);

        Assert.Equal(0, v.Normalized.X);
        Assert.Equal(0, v.Normalized.Y);
    }

    [Fact]
    public void ImplicitOperatorFromTuple_CreatesVectorInt()
    {
        VectorInt v = (5, -3);

        Assert.Equal(5, v.X);
        Assert.Equal(-3, v.Y);
    }

    [Fact]
    public void AddOperator_AddsComponents()
    {
        VectorInt v1 = new(1, 2);
        VectorInt v2 = new(3, 4);

        VectorInt result = v1 + v2;

        Assert.Equal(4, result.X);
        Assert.Equal(6, result.Y);
    }

    [Fact]
    public void SubtractOperator_SubtractsComponents()
    {
        VectorInt v1 = new(5, 7);
        VectorInt v2 = new(2, 3);

        VectorInt result = v1 - v2;

        Assert.Equal(3, result.X);
        Assert.Equal(4, result.Y);
    }

    [Fact]
    public void MultiplyOperator_ScalesVector()
    {
        VectorInt v = new(2, 3);

        Vector result = v * 2.5f;

        Assert.Equal(5, result.X);
        Assert.Equal(7.5f, result.Y);
    }

    [Fact]
    public void DivideOperator_ScalesVector()
    {
        VectorInt v = new(6, 8);

        Vector result = v / 2;

        Assert.Equal(3, result.X);
        Assert.Equal(4, result.Y);
    }

    [Fact]
    public void NegateOperator_NegatesComponents()
    {
        VectorInt v = new(3, -5);

        VectorInt result = -v;

        Assert.Equal(-3, result.X);
        Assert.Equal(5, result.Y);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        VectorInt v = new(5, -3);

        Assert.Equal("(5, -3)", v.ToString());
    }

    [Fact]
    public void GetHashCode_IsConsistent()
    {
        VectorInt v1 = new(1, 2);
        VectorInt v2 = new(1, 2);

        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }

    [Fact]
    public void Equality_WorksCorrectly()
    {
        VectorInt v1 = new(1, 2);
        VectorInt v2 = new(1, 2);
        VectorInt v3 = new(2, 1);
        
        Assert.Equal(v1, v2);
        Assert.NotEqual(v1, v3);
    }
}
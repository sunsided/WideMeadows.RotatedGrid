using System.Numerics;
using FluentAssertions;

namespace WideMeadows.RotatedGrid.Tests;

public class VectorTests
{
    [Fact]
    public void TestNormalize()
    {
        new Vector2(2.0F, 2.0F).Normalized().Round(4).Should().Be(new Vector2(0.7071F, 0.7071F));
    }

    [Fact]
    public void TestRotate()
    {
        var vector = new Vector2(1.0F, 0.0F);
        vector.Rotate(Angle.FromDegrees(0.0)).Round(3).Should().Be(new Vector2(1.0F, 0.0F));
        vector.Rotate(Angle.FromDegrees(90.0)).Round(3).Should().Be(new Vector2(0.0F, 1.0F));
        vector.Rotate(Angle.FromDegrees(180.0)).Round(3).Should().Be(new Vector2(-1.0F, 0.0F));
        vector.Rotate(Angle.FromDegrees(-90.0)).Round(3).Should().Be(new Vector2(0.0F, -1.0F));
        vector.Rotate(Angle.FromDegrees(45.0)).Round(3).Should().Be(new Vector2(1.0F, 1.0F).Normalized().Round(3));
    }

    [Fact]
    public void TestRotateAround()
    {
        var vector = new Vector2(1.0F, 0.0F);

        // Zero rotation (around any point) results in no change.
        vector.RotateAround(vector, Angle.FromDegrees(0.0)).Should().Be(vector);

        // Any rotation around the point itself results in no change.
        vector.RotateAround(vector, Angle.FromDegrees(45.0)).Should().Be(vector);

        // Rotate around the specified pivot vector.
        vector.RotateAround(new Vector2(1.0F, 1.0F), Angle.FromDegrees(90.0)).Should().Be(new Vector2(2.0F, 1.0F));
    }

    [Fact]
    public void TestOrthogonal()
    {
        new Vector2(1.0F, 0.0F).Orthogonal().Should().Be(new Vector2(0.0F, 1.0F));
        new Vector2(0.0F, 1.0F).Orthogonal().Should().Be(new Vector2(-1.0F, 0.0F));
    }

    [Fact]
    public void TestDot()
    {
        new Vector2(1.0F, 0.0F).Dot(new Vector2(1.0F, 1.0F)).Should().Be(1.0F);
        new Vector2(2.0F, 3.0F).Dot(new Vector2(4.0F, -1.0F)).Should().Be(5.0F);
    }
}

using FluentAssertions;

namespace WideMeadows.RotatedGrid.Tests;

public sealed class GridPositionIteratorTests
{
    [Fact]
    public void ItWorks()
    {
        const float width = 10240.0F;
        const float height = 128.0F;
        const float angle = 45.0F;

        var grid = new GridPositionIterator(width, height, 7.0F, 7.0F, 0.0F, 0.0F, Angle.FromDegrees(angle));

        var coordinates = new List<GridCoord>();
        while (grid.MoveNext())
        {
            coordinates.Add(grid.Current);
        }

        coordinates.Count.Should().Be(25863);
        coordinates[0].Round(3).Should().Be(new GridCoord(1.961F, 4.603F).Round(3));
        coordinates[1024].Round(3).Should().Be(new GridCoord(373.192F, 98.648F).Round(3));
        coordinates[25862].Round(3).Should().Be(new GridCoord(10238.039F, 123.397F).Round(3));
    }

    [Fact]
    public void SimplifiedTest()
    {
        const float width = 10.0F;
        const float height = 7.0F;
        const float angle = 0.0F;

        var grid = new GridPositionIterator(width, height, 2.0F, 3.0F, 0.0F, 0.0F, Angle.FromDegrees(angle));

        var coordinates = new List<GridCoord>();
        while (grid.MoveNext())
        {
            coordinates.Add(grid.Current);
        }

        coordinates.Count.Should().Be(15);
        coordinates.Should().ContainInOrder(
            // First row
            new GridCoord(1.0F, 1.5F),
            new GridCoord(3.0F, 1.5F),
            new GridCoord(5.0F, 1.5F),
            new GridCoord(7.0F, 1.5F),
            new GridCoord(9.0F, 1.5F),
            // Second row row
            new GridCoord(1.0F, 3.5F),
            new GridCoord(3.0F, 3.5F),
            new GridCoord(5.0F, 3.5F),
            new GridCoord(7.0F, 3.5F),
            new GridCoord(9.0F, 3.5F),
            // Third row
            new GridCoord(1.0F, 5.5F),
            new GridCoord(3.0F, 5.5F),
            new GridCoord(5.0F, 5.5F),
            new GridCoord(7.0F, 5.5F),
            new GridCoord(9.0F, 5.5F)
            );
    }
}

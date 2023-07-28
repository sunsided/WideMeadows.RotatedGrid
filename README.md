# Rotated grids in C#

🎨 _For halftone dithering and more._

This project provides an enumerator for creating grid coordinates at a specified frequency along
a rotated grid. This can come in useful e.g. when you want to create halftone dithering grids for CMYK processing.

This is a port of the [rotated-grid](https://github.com/sunsided/rotated-grid) Rust crate.
The following screenshot is taken from that repo; this repository does not contain any visualization code.

<div align="center" style="text-align: center">
    <img src="https://raw.githubusercontent.com/sunsided/rotated-grid/main/readme/grid.png" alt="CMYK grid examples" />
</div>

## Examples

Enumeration on an aligned grid (0 degrees rotation). The points are evenly spaced by dx=2
and dy=3, centered inside a 10×7 rectangle. No offset is used.

```csharp
const float width = 10.0F;
const float height = 7.0F;

var grid = new GridPositionIterator(width, height, 2.0F, 3.0F, 0.0F, 0.0F, Angle.Zero);

var coordinates = new List<GridCoord>();
while (grid.MoveNext())
{
    coordinates.Add(grid.Current);
}

coordinates.Count.Should().Be(15);
coordinates.Should().ContainInOrder(
    // First row
    new GridCoord(1.0F, 0.5F),
    new GridCoord(3.0F, 0.5F),
    new GridCoord(5.0F, 0.5F),
    new GridCoord(7.0F, 0.5F),
    new GridCoord(9.0F, 0.5F),
    // Second row row
    new GridCoord(1.0F, 3.5F),
    new GridCoord(3.0F, 3.5F),
    new GridCoord(5.0F, 3.5F),
    new GridCoord(7.0F, 3.5F),
    new GridCoord(9.0F, 3.5F),
    // Third row
    new GridCoord(1.0F, 6.5F),
    new GridCoord(3.0F, 6.5F),
    new GridCoord(5.0F, 6.5F),
    new GridCoord(7.0F, 6.5F),
    new GridCoord(9.0F, 6.5F)
    );
```

Enumeration on a rotated grid; in the example below, 45 degrees rotation are used:

```csharp
const float width = 10.0F;
const float height = 7.0F;
const float angle = 45.0F;

var grid = new GridPositionIterator(width, height, 2.0F, 2.0F, 0.0F, 0.0F, Angle.FromDegrees(angle));

var coordinates = new List<GridCoord>();
while (grid.MoveNext())
{
    coordinates.Add(grid.Current);
}

coordinates.Count.Should().Be(17);
coordinates.Should().ContainInOrder(
    // First row
    new GridCoord(0.7573595F, 2.0857863F),
    new GridCoord(2.1715730F, 0.6715729F),
    // Second row
    new GridCoord(0.7573595F, 4.9142137F),
    new GridCoord(2.1715730F, 3.5000000F),
    new GridCoord(3.5857863F, 2.0857863F),
    new GridCoord(5.0000000F, 0.6715729F),
    // Third row
    new GridCoord(2.1715730F, 6.3284273F),
    new GridCoord(3.5857863F, 4.9142137F),
    new GridCoord(5.0000000F, 3.5000000F),
    new GridCoord(6.4142137F, 2.0857863F),
    new GridCoord(7.8284273F, 0.6715729F),
    // Fourth row
    new GridCoord(5.0000000F, 6.3284273F),
    new GridCoord(6.4142137F, 4.9142137F),
    new GridCoord(9.2426405F, 2.0857863F),
    // Fifth row
    new GridCoord(7.8284273F, 6.3284273F),
    new GridCoord(9.2426405F, 4.9142137F)
);
```

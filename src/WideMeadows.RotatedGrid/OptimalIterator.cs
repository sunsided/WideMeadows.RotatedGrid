using System.Collections;
using System.Numerics;

namespace WideMeadows.RotatedGrid;

internal sealed class OptimalIterator : IEnumerator<Vector2>
{
    private float _y;
    private readonly float _minX;
    private readonly float _maxY;
    private readonly Vector2 _center;
    private readonly Vector2 _extent;
    private readonly Vector2 _delta;
    private readonly Vector2 _offset;
    private readonly Line _rectTop;
    private readonly Line _rectLeft;
    private readonly Line _rectBottom;
    private readonly Line _rectRight;
    private OptimalXIterator? _xIter;

    public OptimalIterator(Vector2 tl, Vector2 tr, Vector2 bl, Vector2 br, Angle alpha, float dx, float dy, float x0,
        float y0)
    {
        var (sin, cos) = alpha.SinCos();

        // Parameters of the axis-aligned rectangle.
        var rectWidth = (tr - tl).Length();
        var rectHeight = (bl - tl).Length();
        var extent = new Vector2(rectWidth, rectHeight);
        var center = (tl + tr + bl + br) * 0.25F;

        // Calculate the rotated rectangle.
        var tlRotated = tl.RotateAround(center, sin, cos);
        var trRotated = tr.RotateAround(center, sin, cos);
        var blRotated = bl.RotateAround(center, sin, cos);
        var brRotated = br.RotateAround(center, sin, cos);

        // Determine line segments describing the rotated rectangle.
        var rectTop = Line.FromPoints(trRotated, tlRotated);
        var rectLeft = Line.FromPoints(tlRotated, blRotated);
        var rectBottom = Line.FromPoints(blRotated, brRotated);
        var rectRight = Line.FromPoints(trRotated, brRotated);

        // Obtain the Axis-Aligned Bounding Box that wraps the rotated rectangle.
        var extentWrappingAABB = new Vector2(
            (float)(extent.X * cos + extent.Y * sin),
            (float)(extent.X * sin + extent.Y * cos)
        );
        var tlWrappingAABB = center - extentWrappingAABB * 0.5F;
        var brWrappingAABB = center + extentWrappingAABB * 0.5F;

        // Determine (half) the number and offset of rows in rotated space.
        var yCountHalf = Math.Floor(extentWrappingAABB.Y / dy * 0.5);
        var startY = center.Y - yCountHalf * dy + y0;
        var y = Math.Ceiling((tlWrappingAABB.Y - startY) / dy) * dy + startY;

        _y = (float)y;
        _minX = tlWrappingAABB.X;
        _maxY = brWrappingAABB.Y;
        _center = center;
        _extent = extentWrappingAABB;
        _delta = new Vector2(dx, dy);
        _offset = new Vector2(x0, y0);
        _rectTop = rectTop;
        _rectLeft = rectLeft;
        _rectBottom = rectBottom;
        _rectRight = rectRight;
    }

    /// <summary>
    ///     Returns the center of the rectangle.
    /// </summary>
    public Vector2 Center => _center;

    /// <inheritdoc />
    public Vector2 Current { get; private set; }

    /// <inheritdoc />
    object IEnumerator.Current => Current;

    /// <inheritdoc />
    public bool MoveNext()
    {
        while (true)
        {
            if (_y > _maxY)
            {
                return false;
            }

            // Exhaust the X coordinate iterator.
            if (_xIter is { } iter)
            {
                if (iter.MoveNext())
                {
                    Current = new Vector2(iter.Current, _y);
                    return true;
                }

                _y += _delta.Y;
            }

            // Obtain the rows.
            var x = _minX;
            var rowStart = new Vector2(x, _y);
            var rowEnd = new Vector2(x + _extent.X, _y);

            // Determine the intersection of the ray from the given row with the rectangle.
            var ray = Line.FromPoints(rowStart, rowEnd);
            if (FindIntersections(ray) is { Start: var start, End: var end })
            {
                _xIter = new OptimalXIterator(_center, _extent, start, end, _delta.X, _offset.X);
            }
        }
    }

    /// <inheritdoc />
    public void Reset()
    {
        _xIter = null;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _xIter?.Dispose();
    }

    /// <summary>
    ///     Finds the intersection point that is furthest from the specified line's origin,
    ///     assuming the line's origin already is an intersection point.
    /// </summary>
    /// <param name="ray"></param>
    /// <returns></returns>
    private (Vector2 Start, Vector2 End)? FindIntersections(Line ray)
    {
        var min = float.PositiveInfinity;
        var max = float.NegativeInfinity;

        var width = _extent.X;
        var height = _extent.Y;

        if (ray.TryCalculateIntersectionT(_rectTop, width, out var t))
        {
            min = Math.Min(min, t);
            max = Math.Max(max, t);
        }

        if (ray.TryCalculateIntersectionT(_rectBottom, width, out var b))
        {
            min = Math.Min(min, b);
            max = Math.Max(max, b);
        }

        if (ray.TryCalculateIntersectionT(_rectLeft, height, out var l))
        {
            min = Math.Min(min, l);
            max = Math.Max(max, l);
        }

        if (ray.TryCalculateIntersectionT(_rectRight, height, out var r))
        {
            min = Math.Min(min, r);
            max = Math.Max(max, r);
        }

        if (float.IsFinite(min) && float.IsFinite(max))
        {
            return (
                ray.ProjectOut(min), ray.ProjectOut(max)
            );
        }

        return null;
    }
}

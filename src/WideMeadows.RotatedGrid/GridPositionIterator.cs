using System.Collections;
using System.Numerics;

namespace WideMeadows.RotatedGrid;

/// <summary>
///     An iterator for positions on a rotated grid.
/// </summary>
public sealed class GridPositionIterator : IEnumerator<GridCoord>
{
    private readonly double _invSin;
    private readonly double _invCos;
    private readonly OptimalIterator _inner;

    public GridPositionIterator(float width, float height, float dx, float dy, float x0, float y0, Angle alpha)
    {
        var alphaNormalized = alpha.Normalized();
        if (alphaNormalized < Angle.Zero || alphaNormalized > Angle.D90)
        {
            throw new ArgumentOutOfRangeException(nameof(alpha), "The angle must be in range 0..PI/2 (zero to 90 degrees)");
        }

        if (dx <= 0.0F)
        {
            throw new ArgumentOutOfRangeException(nameof(dx), "The x step (dx) must be a positive value");
        }

        if (dy <= 0.0F)
        {
            throw new ArgumentOutOfRangeException(nameof(dy), "The y step (dy) must be a positive value");
        }

        var (sin, cos) = alphaNormalized.SinCos();

        var tl = new Vector2(0.0F, 0.0F);
        var tr = new Vector2(width, 0.0F);
        var bl = new Vector2(0.0F, height);
        var br = new Vector2(width, height);

        _invSin = -sin;
        _invCos = cos;
        _inner = new OptimalIterator(tl, tr, bl, br, alphaNormalized, dx, dy, x0, y0);
    }

    /// <inheritdoc />
    public GridCoord Current { get; private set; }

    /// <inheritdoc />
    object IEnumerator.Current => Current;

    /// <inheritdoc />
    public bool MoveNext()
    {
        if (!_inner.MoveNext())
        {
            return false;
        }

        var point = _inner.Current;
        var center = _inner.Center;

        // Un-rotate the point.
        var centered = point - center;
        var unrotated = new Vector2(
            (float)(centered.X * _invCos) - (float)(centered.Y * _invSin),
            (float)(centered.X * _invSin) + (float)(centered.Y * _invCos)) + center;

        Current = new GridCoord(unrotated);
        return true;
    }

    /// <inheritdoc />
    public void Reset() => _inner.Reset();

    /// <inheritdoc />
    public void Dispose() => _inner.Dispose();
}

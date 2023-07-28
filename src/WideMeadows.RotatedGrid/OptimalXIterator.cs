using System.Collections;
using System.Numerics;

namespace WideMeadows.RotatedGrid;

internal sealed class OptimalXIterator : IEnumerator<float>
{
    private readonly float _startX;
    private readonly float _rowEnd;
    private readonly float _dx;
    private float _x;

    public OptimalXIterator(Vector2 center, Vector2 extent, Vector2 rowStart, Vector2 rowEnd, float dx, float x0)
    {
        // Determine the first x coordinate along the row that is
        // an integer multiple of dx away from the center and larger
        // than the start coordinate.
        var xCountHalf = Math.Floor(extent.X / dx * 0.5);
        var startX = center.X - xCountHalf * dx + x0;
        _x = _startX = (float)(Math.Ceiling((rowStart.X - startX) / dx) * dx + startX);
        _dx = dx;
        _rowEnd = rowEnd.X;
    }

    /// <inheritdoc />
    public float Current {get; private set; }

    /// <inheritdoc />
    object IEnumerator.Current => Current;

    /// <inheritdoc />
    public bool MoveNext()
    {
        if (_x > _rowEnd)
        {
            Current = _x = float.NaN;
            return false;
        }

        Current = _x;
        _x += _dx;
        return true;
    }

    /// <inheritdoc />
    public void Reset()
    {
        _x = _startX;
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}

using System.Diagnostics;
using System.Numerics;

namespace WideMeadows.RotatedGrid;

[DebuggerDisplay("{Origin}, {Direction}")]
internal readonly struct Line
{
    private readonly Vector2 _origin;
    private readonly Vector2 _direction;

    public Line(Vector2 origin, Vector2 direction)
    {
        _origin = origin;
        _direction = direction.Normalized();
    }

    public Vector2 Origin => _origin;
    public Vector2 Direction => _direction;

    /// <summary>
    ///     Constructs a line through two points.
    /// </summary>
    public static Line FromPoints(Vector2 a, Vector2 b) => new(a, b - a);

    /// <summary>
    ///     Projects a vector at a given distance alongside a direction
    ///     from the origin of the line.
    /// </summary>
    public Vector2 ProjectOut(float t) => _origin + _direction * t;

    public bool TryCalculateIntersectionT(Line other, float maxU, out float t)
    {
        var det = _direction.Cross(other._direction);
        if (Math.Abs(det) < 1e-6)
        {
            t = default;
            return false;
        }

        var delta = _origin - other._origin;

        // Length along self to the point of intersection.
        t = other._direction.Cross(delta) / det;

        // Project the intersection point out.
        var projected = delta.ProjectOut(_direction, t);

        // Squared length along other to the point of intersection.
        var u = projected.Dot(other._direction);

        return t >= 0.0 && u >= 0.0 && u <= maxU * maxU;
    }

    public float? CalculateIntersectionT(Line other, float maxU)
    {
        if (TryCalculateIntersectionT(other, maxU, out var t))
        {
            return t;
        }

        return null;
    }

    public static Line operator -(Line line) => new(line._origin, -line._direction);

    public static Vector2 operator *(Line line, float t) => line.ProjectOut(t);
}

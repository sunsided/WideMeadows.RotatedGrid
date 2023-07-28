using System.Numerics;

namespace WideMeadows.RotatedGrid;

internal static class VectorExtensions
{
    public static float Cross(this Vector2 vector, Vector2 other) =>
        vector.X * other.Y - vector.Y * other.X;

    public static float Dot(this Vector2 vector, Vector2 other) =>
        vector.X * other.X + vector.Y * other.Y;

    public static Vector2 Orthogonal(this Vector2 vector) =>
        new(-vector.Y, vector.X);

    public static Vector2 Normalized(this Vector2 vector) =>
        vector / vector.Length();

    /// <summary>
    ///     Rotates the vector counterclockwise by the specified angle.
    /// </summary>
    public static Vector2 Rotate(this Vector2 vector, Angle angle)
    {
        var (sin, cos) = angle.SinCos();
        return Rotate(vector, sin, cos);
    }

    /// <summary>
    ///     Rotates the vector counterclockwise by the specified angle expressed as its sine and cosine.
    /// </summary>
    public static Vector2 Rotate(this Vector2 vector, double sin, double cos) =>
        new(
            (float)(vector.X * cos - vector.Y * sin),
            (float)(vector.X * sin + vector.Y * cos)
        );

    /// <summary>
    ///     Rotates the vector counterclockwise by the specified angle expressed as its sine and cosine.
    /// </summary>
    public static Vector2 RotateAround(this Vector2 vector, Vector2 pivot, Angle angle)
    {
        var (sin, cos) = angle.SinCos();
        return RotateAround(vector, pivot, sin, cos);
    }

    /// <summary>
    ///     Rotates the vector counterclockwise by the specified angle expressed as its sine and cosine.
    /// </summary>
    public static Vector2 RotateAround(this Vector2 vector, Vector2 pivot, double sin, double cos)
    {
        var centered = vector - pivot;
        var x = centered.X * cos - centered.Y * sin;
        var y = centered.X * sin + centered.Y * cos;
        return new Vector2((float)x, (float)y) + pivot;
    }

    /// <summary>
    ///     Projects a vector at a given distance alongside a direction
    ///     from the current origin.
    /// </summary>
    /// <remarks>
    ///     This typically assumes that the <paramref name="direction"/> is a normalized
    ///     vector, although it isn't required and depends on the actual use.
    /// </remarks>
    public static Vector2 ProjectOut(this Vector2 vector, Vector2 direction, float t) =>
        vector + direction * t;

    /// <summary>
    ///     Rounds the coordinates to the specified number of decimals.
    ///     This simplifies testing.
    /// </summary>
    internal static Vector2 Round(this Vector2 vector, byte decimals) =>
        new(
            (float)Math.Round(vector.X, (int)decimals),
            (float)Math.Round(vector.Y, (int)decimals)
        );
}

namespace WideMeadows.RotatedGrid;

/// <summary>
///     An angle expressed in radians.
/// </summary>
public readonly struct Angle : IEquatable<Angle>, IComparable<Angle>
{
    private readonly double _radians;

    public static readonly Angle Zero = new(0.0);
    public static readonly Angle D90 = FromDegrees(180);

    private Angle(double radians)
    {
        _radians = radians;
    }

    /// <summary>
    ///     Gets the angle in radians.
    /// </summary>
    public double Radians => _radians;

    /// <summary>
    ///     Constructs an <see cref="Angle"/> instance from an angle specified in degrees.
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <returns>An <see cref="Angle"/>.</returns>
    public static Angle FromDegrees(double degrees) => new(degrees * Math.PI / 180);

    /// <summary>
    ///     Constructs an <see cref="Angle"/> instance from an angle specified in radians.
    /// </summary>
    /// <param name="radians">The angle in radians.</param>
    /// <returns>An <see cref="Angle"/>.</returns>
    public static Angle FromRadians(double radians) => new(radians);

    /// <summary>
    ///     Determines the sine and cosine of the angle.
    /// </summary>
    /// <returns>The sine and cosine of the angle.</returns>
#if NET7_0_OR_GREATER
    public (double Sin, double Cos) SinCos() => Math.SinCos(_radians);
#else
    public (double Sin, double Cos) SinCos() => (Math.Sin(_radians), Math.Cos(_radians));
#endif

    /// <summary>
    ///     Normalizes the specified angle such that it falls into range -PI/2..PI/2.
    /// </summary>
    public Angle Normalized()
    {
        var alpha = _radians;
        const double halfPi = Math.PI / 2;

        while (alpha >= Math.PI)
        {
            alpha -= Math.PI;
        }

        while (alpha >= halfPi)
        {
            alpha -= halfPi;
        }

        while (alpha <= -Math.PI)
        {
            alpha += Math.PI;
        }

        while (alpha <= -halfPi)
        {
            alpha += halfPi;
        }

        return new Angle(alpha);
    }

    public static Angle operator -(Angle alpha) => new(-alpha._radians);

    /// <inheritdoc />
    public bool Equals(Angle other) => _radians.Equals(other._radians);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Angle other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _radians.GetHashCode();

    /// <inheritdoc />
    public int CompareTo(Angle other) => _radians.CompareTo(other._radians);

    public static bool operator <(Angle lhs, Angle rhs) => lhs.CompareTo(rhs) < 0;
    public static bool operator >(Angle lhs, Angle rhs) => lhs.CompareTo(rhs) > 0;
    public static bool operator <=(Angle lhs, Angle rhs) => lhs.CompareTo(rhs) <= 0;
    public static bool operator >=(Angle lhs, Angle rhs) => lhs.CompareTo(rhs) >= 0;
    public static bool operator ==(Angle lhs, Angle rhs) => lhs.CompareTo(rhs) == 0;
    public static bool operator !=(Angle lhs, Angle rhs) => lhs.CompareTo(rhs) != 0;
}

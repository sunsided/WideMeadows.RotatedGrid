using System.Numerics;

namespace WideMeadows.RotatedGrid;

public readonly struct GridCoord : IEquatable<GridCoord>, IEquatable<Vector2>
{
    private readonly Vector2 _coord;

    public GridCoord(float x, float y)
    {
        _coord = new Vector2(x, y);
    }
    
    public GridCoord(Vector2 vec)
    {
        _coord = vec;
    }

    public void Deconstruct(out float x, out float y)
    {
        x = _coord.X;
        y = _coord.Y;
    }

    /// <inheritdoc />
    public bool Equals(GridCoord other) => _coord.Equals(other._coord);
    public bool Equals(Vector2 other) => _coord.Equals(other);

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is GridCoord other && Equals(other) ||
        obj is Vector2 vec && Equals(vec);

    /// <inheritdoc />
    public override int GetHashCode() => _coord.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => _coord.ToString();

    internal GridCoord Round(byte decimals) => new(new Vector2(
        (float)Math.Round(_coord.X, decimals),
        (float)Math.Round(_coord.Y, decimals)));
}

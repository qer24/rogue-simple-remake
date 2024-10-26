using System.Runtime.CompilerServices;
using System;
using System.Globalization;
using System.Numerics;

namespace ProjektFB.Utils;

// Klasa z silnika Unity
public struct Vector2Int
{
    private int _x, _y;

    public int x { get => _x; set => _x = value; }
    public int y { get => _y; set => _y = value; }

    public Vector2Int(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public int this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return x;
                case 1: return y;
                default:
                    throw new IndexOutOfRangeException($"Invalid Vector2Int index addressed: {index}!");
            }
        }

        set
        {
            switch (index)
            {
                case 0: x = value; break;
                case 1: y = value; break;
                default:
                    throw new IndexOutOfRangeException($"Invalid Vector2Int index addressed: {index}!");
            }
        }
    }

    // Returns the length of this vector (RO).
    public float magnitude => MathF.Sqrt((x * x + y * y));

    // Returns the squared length of this vector (RO).
    public int sqrMagnitude => x * x + y * y;

    // Returns the distance between /a/ and /b/.
    public static float Distance(Vector2Int a, Vector2Int b)
    {
        float diffX = a.x - b.x;
        float diffY = a.y - b.y;

        return (float)Math.Sqrt(diffX * diffX + diffY * diffY);
    }

    // Returns a vector that is made from the smallest components of two vectors.
    public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs) { return new Vector2Int(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y)); }

    // Returns a vector that is made from the largest components of two vectors.
    public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs) { return new Vector2Int(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y)); }

    // Multiplies two vectors component-wise.
    public static Vector2Int Scale(Vector2Int a, Vector2Int b) { return new Vector2Int(a.x * b.x, a.y * b.y); }

    // Multiplies every component of this vector by the same component of /scale/.
    public void Scale(Vector2Int scale) { x *= scale.x; y *= scale.y; }

    public void Clamp(Vector2Int min, Vector2Int max)
    {
        x = Math.Max(min.x, x);
        x = Math.Min(max.x, x);
        y = Math.Max(min.y, y);
        y = Math.Min(max.y, y);
    }

    // Converts a Vector2Int to a [[Vector2]].
    public static implicit operator Vector2(Vector2Int v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2Int FloorToInt(Vector2 v)
    {
        return new Vector2Int(
            (int)Math.Floor(v.X),
            (int)Math.Floor(v.Y)
        );
    }

    public static Vector2Int CeilToInt(Vector2 v)
    {
        return new Vector2Int(
            (int)Math.Ceiling(v.X),
            (int)Math.Ceiling(v.Y)
        );
    }

    public static Vector2Int RoundToInt(Vector2 v)
    {
        return new Vector2Int(
            (int)Math.Round(v.X),
            (int)Math.Round(v.Y)
        );
    }

    public static Vector2Int operator -(Vector2Int v)
    {
        return new Vector2Int(-v.x, -v.y);
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x + b.x, a.y + b.y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x - b.x, a.y - b.y);
    }

    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x * b.x, a.y * b.y);
    }

    public static Vector2Int operator *(int a, Vector2Int b)
    {
        return new Vector2Int(a * b.x, a * b.y);
    }

    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(a.x * b, a.y * b);
    }

    public static Vector2Int operator /(Vector2Int a, int b)
    {
        return new Vector2Int(a.x / b, a.y / b);
    }

    public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
    {
        return !(lhs == rhs);
    }

    public override bool Equals(object other)
    {
        if (other is Vector2Int v)
            return Equals(v);
        return false;
    }

    public bool Equals(Vector2Int other)
    {
        return x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        const int p1 = 73856093;
        const int p2 = 83492791;
        return (x * p1) ^ (y * p2);
    }

    /// *listonly*
    public override string ToString()
    {
        return ToString(null, null);
    }

    public string ToString(string format)
    {
        return ToString(format, null);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (formatProvider == null)
            formatProvider = CultureInfo.InvariantCulture.NumberFormat;
        return $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
    }
}

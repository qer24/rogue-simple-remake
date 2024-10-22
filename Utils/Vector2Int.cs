﻿using System.Runtime.CompilerServices;
using System;
using System.Globalization;
using System.Numerics;

namespace ProjektFB.Utils;

// Klasa z silnika Unity
public struct Vector2Int
{
    private int _x, _y;

    public int x => _x;
    public int y => _y;

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
                    throw new IndexOutOfRangeException(String.Format("Invalid Vector2Int index addressed: {0}!", index));
            }
        }

        set
        {
            switch (index)
            {
                case 0: x = value; break;
                case 1: y = value; break;
                default:
                    throw new IndexOutOfRangeException(String.Format("Invalid Vector2Int index addressed: {0}!", index));
            }
        }
    }

    // Returns the length of this vector (RO).
    public float magnitude { get { return MathF.Sqrt((x * x + y * y)); } }

    // Returns the squared length of this vector (RO).
    public int sqrMagnitude { get { return x * x + y * y; } }

    // Returns the distance between /a/ and /b/.
    public static float Distance(Vector2Int a, Vector2Int b)
    {
        float diff_x = a.x - b.x;
        float diff_y = a.y - b.y;

        return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
    }

    // Returns a vector that is made from the smallest components of two vectors.
    public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs) { return new Vector2Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y)); }

    // Returns a vector that is made from the largest components of two vectors.
    public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs) { return new Vector2Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y)); }

    // Multiplies two vectors component-wise.
    public static Vector2Int Scale(Vector2Int a, Vector2Int b) { return new Vector2Int(a.x * b.x, a.y * b.y); }

    // Multiplies every component of this vector by the same component of /scale/.
    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public void Scale(Vector2Int scale) { x *= scale.x; y *= scale.y; }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public void Clamp(Vector2Int min, Vector2Int max)
    {
        x = Math.Max(min.x, x);
        x = Math.Min(max.x, x);
        y = Math.Max(min.y, y);
        y = Math.Min(max.y, y);
    }

    // Converts a Vector2Int to a [[Vector2]].
    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static implicit operator Vector2(Vector2Int v)
    {
        return new Vector2(v.x, v.y);
    }

    // Converts a Vector2Int to a [[Vector3Int]].
    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static explicit operator Vector3Int(Vector2Int v)
    {
        return new Vector3Int(v.x, v.y, 0);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int FloorToInt(Vector2 v)
    {
        return new Vector2Int(
            Mathf.FloorToInt(v.x),
            Mathf.FloorToInt(v.y)
        );
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int CeilToInt(Vector2 v)
    {
        return new Vector2Int(
            Mathf.CeilToInt(v.x),
            Mathf.CeilToInt(v.y)
        );
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int RoundToInt(Vector2 v)
    {
        return new Vector2Int(
            Mathf.RoundToInt(v.x),
            Mathf.RoundToInt(v.y)
        );
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int operator -(Vector2Int v)
    {
        return new Vector2Int(-v.x, -v.y);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x + b.x, a.y + b.y);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x - b.x, a.y - b.y);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int operator *(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x * b.x, a.y * b.y);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int operator *(int a, Vector2Int b)
    {
        return new Vector2Int(a * b.x, a * b.y);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int operator *(Vector2Int a, int b)
    {
        return new Vector2Int(a.x * b, a.y * b);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static Vector2Int operator /(Vector2Int a, int b)
    {
        return new Vector2Int(a.x / b, a.y / b);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
    {
        return !(lhs == rhs);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public override bool Equals(object other)
    {
        if (other is Vector2Int v)
            return Equals(v);
        return false;
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public bool Equals(Vector2Int other)
    {
        return x == other.x && y == other.y;
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public override int GetHashCode()
    {
        const int p1 = 73856093;
        const int p2 = 83492791;
        return (x * p1) ^ (y * p2);
    }

    /// *listonly*
    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public override string ToString()
    {
        return ToString(null, null);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public string ToString(string format)
    {
        return ToString(format, null);
    }

    [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (formatProvider == null)
            formatProvider = CultureInfo.InvariantCulture.NumberFormat;
        return UnityString.Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
    }
}

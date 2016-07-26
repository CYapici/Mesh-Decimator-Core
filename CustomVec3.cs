using System;

public struct CustomVec3
{
    private static CustomVec3 zero = new CustomVec3(0f, 0f, 0f);
    public float X;
    public float Y;
    public float Z;
    public CustomVec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public CustomVec3(float value)
    {
        X = value;
        Y = value;
        Z = value;
    }


    public static CustomVec3 Cross(CustomVec3 vector1, CustomVec3 vector2)
    {
        Cross(ref vector1, ref vector2, out vector1);
        return vector1;
    }

    public static void Cross(ref CustomVec3 vector1, ref CustomVec3 vector2, out CustomVec3 result)
    {
        result = new CustomVec3(vector1.Y * vector2.Z - vector2.Y * vector1.Z,
                             -(vector1.X * vector2.Z - vector2.X * vector1.Z),
                             vector1.X * vector2.Y - vector2.X * vector1.Y);
    }


    public static void Distance(ref CustomVec3 value1, ref CustomVec3 value2, out float result)
    {
        DistanceSquared(ref value1, ref value2, out result);
        result = (float)Math.Sqrt(result);
    }

    public static void DistanceSquared(ref CustomVec3 value1, ref CustomVec3 value2, out float result)
    {
        result = (value1.X - value2.X) * (value1.X - value2.X) +
                 (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                 (value1.Z - value2.Z) * (value1.Z - value2.Z);
    }


    public static float Dot(CustomVec3 vector1, CustomVec3 vector2)
    {
        return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
    }


    public override bool Equals(object obj)
    {
        if ((obj is CustomVec3) && this == (CustomVec3)obj) return true;
        return false;
    }

    public bool Equals(CustomVec3 other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return (int)(X + Y + Z);
    }


    public float Length()
    {
        float result;
        DistanceSquared(ref this, ref zero, out result);
        return (float)Math.Sqrt(result);
    }


    public static CustomVec3 Normalize(CustomVec3 vector)
    {
        Normalize(ref vector, out vector);
        return vector;
    }

    public static void Normalize(ref CustomVec3 value, out CustomVec3 result)
    {
        float factor;
        Distance(ref value, ref zero, out factor);
        factor = 1f / factor;
        result.X = value.X * factor;
        result.Y = value.Y * factor;
        result.Z = value.Z * factor;
    }

    public static bool operator ==(CustomVec3 value1, CustomVec3 value2)
    {
        return value1.X == value2.X
            && value1.Y == value2.Y
            && value1.Z == value2.Z;
    }

    public static bool operator !=(CustomVec3 value1, CustomVec3 value2)
    {
        return !(value1 == value2);
    }

    public static CustomVec3 operator +(CustomVec3 value1, CustomVec3 value2)
    {
        value1.X += value2.X;
        value1.Y += value2.Y;
        value1.Z += value2.Z;
        return value1;
    }


    public static CustomVec3 operator -(CustomVec3 value1, CustomVec3 value2)
    {
        value1.X -= value2.X;
        value1.Y -= value2.Y;
        value1.Z -= value2.Z;
        return value1;
    }


    public static CustomVec3 operator /(CustomVec3 value, float divider)
    {
        float factor = 1 / divider;
        value.X *= factor;
        value.Y *= factor;
        value.Z *= factor;
        return value;
    }


    public static CustomVec3 CalculatePlaneNormal(CustomVec3 a, CustomVec3 b, CustomVec3 c)
    {

        var dir = Cross(b - a, c - a);
        var norm = Normalize(dir);
        return norm;

    }


}

namespace SoftwareRenderer;

public struct Float3(float x, float y, float z)
{
    public float X = x;
    public float Y = y;
    public float Z = z;

    public static float Dot(Float3 a, Float3 b)
    {
        return a.X * b.X + a.Y * b.Y; // TODO: Should this consider X?
    }
    
    public static Float3 operator *(Float3 a, int b)
    {
        return new Float3(a.X * b, a.Y * b, a.Z * b);
    }

    public static Float3 operator +(Float3 a, int b)
    {
        return new Float3(a.X + b, a.Y + b, a.Z + b);
    }
}
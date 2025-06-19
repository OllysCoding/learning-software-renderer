namespace SoftwareRenderer;

public struct Pixel(float r, float g, float b)
{
    private static readonly Random random = new();
    public float R = r;
    public float G = g;
    public float B = b;

    public static Pixel Random()
    {
        return new Pixel(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
    }
    
    public static Pixel Black()
    {
        return new Pixel(0, 0, 0);
    }

    public static Pixel White()
    {
        return new Pixel(1, 1, 1);
    }

    public static Pixel Red()
    {
        return new Pixel(1, 0, 0);
    }
    
    public static Pixel Green()
    {
        return new Pixel(0, 1, 0);
    }
    
    public static Pixel Blue()
    {
        return new Pixel(0, 0, 1);
    }
    
    public static Pixel Yellow()
    {
        return new Pixel(1, 0.8f, 0);
    }
}
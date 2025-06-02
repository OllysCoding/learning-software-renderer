namespace SoftwareRenderer;

public struct Pixel(float r, float g, float b)
{
    public float R = r;
    public float G = g;
    public float B = b;
    
    
    /**
     * constexpr TGAColor white   = {255, 255, 255, 255}; // attention, BGRA order
constexpr TGAColor green   = {  0, 255,   0, 255};
constexpr TGAColor red     = {  0,   0, 255, 255};
constexpr TGAColor blue    = {255, 128,  64, 255};
constexpr TGAColor yellow  = {  0, 200, 255, 255};
     */
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

public class Image
{
    public static void CreateTestImage()
    {
        const int width = 64;
        const int height = 64;
    
        Pixel[,] image = new  Pixel[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                image[x, y] = new Pixel(r: y / (width - 1f), g: x / (width - 1f), b: 0);
                // image[x, y] = new Pixel(r: 1, g: 0, b: 0);
            }
        }

        WriteToFile(image, "test.bmp");
    }

    public static Pixel[,] NewBlank(int width, int height)
    {
        Pixel[,] image = new  Pixel[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                image[x, y] = Pixel.Black();
            }
        }

        return image;
    }
    
    public static void DrawLine(Float3 a, Float3 b, Pixel[,] image, Pixel color)
    {
        var  (aX, aY) = (a.X, a.Y);
        var  (bX, bY) = (b.X, b.Y);
        var isSteep = Math.Abs(a.X - b.X) < Math.Abs(a.Y - b.Y);
        if (isSteep)
        {
            // Swap x with y
            (aX, aY) = (aY, aX);
            (bX, bY) = (bY, bX);
        }
        
        if (aX > bX)
        { 
            // Swap a with b
            (aX, bX) = (bX, aX);
            (aY, bY) = (bY, aY);
        }

        float y = aY;
        float increase = ((bY - aY) / (bX - aX));
        
        for (int x = (int)aX; x < bX; x++)
        {
            var t = (x - aX) / (bX - aX);
            
            if (isSteep)
            {
                image[(int)y, x] = color;
            }
            else
            {
                image[x, (int)y] = color;
            }

            y += increase;
        }
    }

    public static void WriteToFile(Pixel[,] image, string path)
    {
        using BinaryWriter writer = new(File.Open(path, FileMode.Create));
        uint[] ByteCounts = { 14, 40, (uint)image.Length * 4 }; // BMP Header, DIP header, data
        
        writer.Write(("BM"u8.ToArray()));
        writer.Write(ByteCounts[0] + ByteCounts[1] + ByteCounts[2]);
        writer.Write((uint)0);
        writer.Write(ByteCounts[0] + ByteCounts[1]); // Data offset
        writer.Write(ByteCounts[1]);
        writer.Write((uint)image.GetLength(0)); // width
        writer.Write((uint)image.GetLength(1)); // height
        writer.Write((ushort)1); // nuw colour planes?
        writer.Write((ushort)(8 * 4)); // bits per pixel, 1 byte per channel + 1 for alignment
        writer.Write((uint)0); // RBG format, no compression
        writer.Write(ByteCounts[2]); // data pize
        writer.Write(new byte[16]); // ?

        for (int y = 0; y < image.GetLength(1); y++)
        {
            for (int x = 0; x < image.GetLength(0); x++)
            {
                Pixel pixel = image[x, y];
                writer.Write((byte)(pixel.B * 255));
                writer.Write((byte)(pixel.G * 255));
                writer.Write((byte)(pixel.R * 255));
                writer.Write((byte)0); // alignment
            }
        }
    }
}
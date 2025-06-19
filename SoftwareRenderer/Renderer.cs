namespace SoftwareRenderer;

public class Renderer
{
    private Pixel[,] _imageData;
    public readonly int Width;
    public readonly int Height;
    
    public Renderer(int width, int height)
    {
        Width = width;
        Height = height;
        _imageData = new  Pixel[width, height];
        Blank(Pixel.Black());
    }

    private void Blank(Pixel color)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _imageData[x, y] = color;
            }
        }
    }
    
    private bool IsWithinScreenBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
    
    private Float3 ToScreenSpace(Float3 f)
    {
        return new Float3(
            f.X,
            f.Y,
            0
        );
        
        // TODO: Move scaling to transform of some sort
        // return new Float3(
        //     f.X * (Width / 2),
        //     f.Y * (Height / 2),
        //     0
        // );
    }

    public void DrawPixel(int x, int y, Pixel color)
    {
        if (IsWithinScreenBounds(x, y))
        {
            _imageData[x, y] = color;
        }
    }

    public void DrawLine(Float3 worldA, Float3 worldB, Pixel color)
    {
        var a = ToScreenSpace(worldA);
        var b = ToScreenSpace(worldB);

        var (aX, aY) = (a.X, a.Y);
        var (bX, bY) = (b.X, b.Y);
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
                DrawPixel((int)y, x, color);
            }
            else
            {
                DrawPixel(x, (int)y, color);
            }

            y += increase;
        }
    }

    private float getLineSteepness(Float3 a, Float3 b)
    {
        return Math.Abs(a.Y - b.Y);
    }

    private Tuple<float, float, float, float> GetLineForDrawing(Float3 a, Float3 b)
    {
        var (aX, aY) = (a.X, a.Y);
        var (bX, bY) = (b.X, b.Y);
        // var isSteep = Math.Abs(a.X - b.X) < Math.Abs(a.Y - b.Y);
        // if (isSteep)
        // {
        //     // Swap x with y
        //     (aX, aY) = (aY, aX);
        //     (bX, bY) = (bY, bX);
        // }
        // if (aX > bX) // Always left to right
        // {
        //     // Swap a with b
        //     (aX, bX) = (bX, aX);
        //     (aY, bY) = (bY, aY);
        // }
        return new(aX, aY, bX, bY);
    }

    private void DrawFilledTrianglePart(Float3 a1, Float3 b1, Float3 b2)
    {
        // Draw each step of each line
        // If we are in the 'overlap' fill in pixels between
        // b1 should always be the 'longest' line
        
        // TODO: Invert is steep to 'is not steep?'
        var (a1X, a1Y, b1X, b1Y) = GetLineForDrawing(a1, b1);
        var (_, _, b2X, b2Y) = GetLineForDrawing(a1, b1);
        
        // TODO: Invert so we are looping over y, calculating X.
        float y1 = a1Y;
        float y2 = a1Y;
        
        float increase1 = ((b1Y - a1Y) / (b1X - a1X));
        float increase2 = ((b2Y - a1Y) / (b2X - a1X));

        for (int x = (int)aX; x < bX; x++)
        {
            if (isSteep)
            {
                DrawPixel((int)y, x, color);
            }
            else
            {
                DrawPixel(x, (int)y, color);
            }

            y1 += increase1;
            y2 += increase2;
        }
    }

    public void DrawFilledTriangle(Float3 worldA, Float3 worldB, Float3 worldC, Pixel color)
    {
        Float3[] sorted = [ToScreenSpace(worldA), ToScreenSpace(worldB), ToScreenSpace(worldC)];
        Array.Sort(sorted, (one, two) => one.Y.CompareTo(two.Y));
        
        /**
         * Lets think about this, we need to get the "left and right",
         *
         * We have 3 lines, we want two 'draw' operations each with 2 lines.
         *
         * Highest point + lowest point & highest point + second lowest point
         * Highest point + lowest point & second lowest point + lowest point
         */

        DrawFilledTrianglePart(sorted[0], sorted[2], sorted[1]);
        DrawFilledTrianglePart(sorted[2], sorted[0], sorted[1]);
    }

    public void DrawTriangle(Float3 a, Float3 b, Float3 c, Pixel color)
    {
        DrawLine(a, b, color);
        DrawLine(c, b, color);
        DrawLine(c, a, color);
    }
    
    public void RenderScene(Scene scene)
    {
        // Blank frame before rendering
        Blank(Pixel.Black());
        foreach (var model in scene.Models)
        {
            var triangles = model.GetTriangles();
            foreach (var triangle in triangles)
            {
                DrawTriangle(triangle.Item1, triangle.Item2, triangle.Item3, Pixel.Red());
            }
        }
    }

    public void WriteToByteArray(byte[] arr)
    {
        var writerIndex = 0;
        for (int y = 0; y < _imageData.GetLength(1); y++)
        {
            for (int x = 0; x < _imageData.GetLength(0); x++)
            {
                var pixel = _imageData[x, Height - y - 1];
                arr[writerIndex++] = (byte)(pixel.R * 255);
                arr[writerIndex++] = (byte)(pixel.G * 255);
                arr[writerIndex++] = (byte)(pixel.B * 255);
                arr[writerIndex++] = (byte)(1); // Alpha
            }
        }
    }

    public void WriteToFile(String path)
    {
        using BinaryWriter writer = new(File.Open(path, FileMode.Create));
        uint[] byteCounts = { 14, 40, (uint)_imageData.Length * 4 }; // BMP Header, DIP header, data
        
        writer.Write(("BMP"u8.ToArray()));
        writer.Write(byteCounts[0] + byteCounts[1] + byteCounts[2]);
        writer.Write((uint)0);
        writer.Write(byteCounts[0] + byteCounts[1]); // Data offset
        writer.Write(byteCounts[1]);
        writer.Write((uint)_imageData.GetLength(0)); // width
        writer.Write((uint)_imageData.GetLength(1)); // height
        writer.Write((ushort)1); // nuw colour planes?
        writer.Write((ushort)(8 * 4)); // bits per pixel, 1 byte per channel + 1 for alignment
        writer.Write((uint)0); // RBG format, no compression
        writer.Write(byteCounts[2]); // data pize
        writer.Write(new byte[16]); // ?

        for (int y = 0; y < _imageData.GetLength(1); y++)
        {
            for (int x = 0; x < _imageData.GetLength(0); x++)
            {
                Pixel pixel = _imageData[x, y];
                writer.Write((byte)(pixel.B * 255));
                writer.Write((byte)(pixel.G * 255));
                writer.Write((byte)(pixel.R * 255));
                writer.Write((byte)0); // alignment
            }
        }
    }
}
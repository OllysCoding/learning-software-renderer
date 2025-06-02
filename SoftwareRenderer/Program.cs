// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using SoftwareRenderer;

Console.WriteLine("Hello, World!");

// Image.CreateTestImage();

var image = Image.NewBlank(64, 64);

// Float3 a = new Float3(7, 3, 0);
// Float3 b = new Float3(12, 37, 0);
// Float3 c = new Float3(62, 53, 0);

// Image.DrawLine(a, b, image, Pixel.Blue());
// Image.DrawLine(c, b, image, Pixel.Green());
// Image.DrawLine(c, a, image, Pixel.Yellow());
// Image.DrawLine(a, c, image, Pixel.Red());
//
// image[(int)a.X, (int)a.Y] = Pixel.White(); 
// image[(int)b.X, (int)b.Y] = Pixel.White(); 
// image[(int)c.X, (int)c.Y] = Pixel.White(); 

Stopwatch sw = new Stopwatch();

sw.Start();

Random random = new Random();
for (int i=0; i<(1<<24); i++)
{
    var a = new Float3(random.Next(0, 64), random.Next(0, 64), 0);
    var b = new Float3(random.Next(0, 64), random.Next(0, 64), 0);
    var color = new Pixel((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
    Image.DrawLine(a, b, image, color);
}

sw.Stop();

Console.WriteLine("Elapsed={0}",sw.Elapsed);

Image.WriteToFile(image, "triangle.bmp");





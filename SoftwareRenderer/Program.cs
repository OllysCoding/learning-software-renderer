
using System.Diagnostics;
using Raylib_cs;
using SoftwareRenderer;
using Float3 = SoftwareRenderer.Float3;
using Model = SoftwareRenderer.Model;

var width = 500;
var height = 500;
var renderer = new Renderer(width, height);
var scene = new Scene(width, height);
// scene.AddModel(Model.ReadFromObjFile("../../../../resources/diablo3_pose.obj"));

Raylib.InitWindow(width, height, "Renderer");
Texture2D texture = Raylib.LoadTextureFromImage(Raylib.GenImageColor(width, height, Color.Black));
byte[] textureBytes = new byte[width * height * 4]; // RGBA

var timer = new Stopwatch();
long lastFrameTime = 0; // TODO: Debounce

while (!Raylib.WindowShouldClose())
{
    timer.Start();
    
    renderer.RenderScene(scene);
    renderer.DrawTriangle(new Float3(7, 45, 0), new Float3(35, 100, 0), new Float3(45, 60, 0), Pixel.Random());
    renderer.DrawTriangle(new Float3(120, 35, 0), new Float3(90, 5, 0), new Float3(45, 110, 0), Pixel.Random());
    renderer.DrawTriangle(new Float3(115, 83, 0), new Float3(80, 90, 0), new Float3(85, 120, 0), Pixel.Random());
    renderer.WriteToByteArray(textureBytes);
    
    Raylib.UpdateTexture(texture, textureBytes);
    Raylib.BeginDrawing();
    Raylib.DrawTexture(texture, 0, 0, Color.White);
    var realFps = lastFrameTime == 0 ? "?" : (1000 / lastFrameTime).ToString();
    Raylib.DrawText($"{realFps}fps", 15, 15, 15, Color.White);
    
    Raylib.EndDrawing();
    
    timer.Stop();
    lastFrameTime = timer.ElapsedMilliseconds;
    Console.WriteLine($"Rendered frame in {timer.ElapsedMilliseconds}ms");
    timer.Reset();
}

Raylib.CloseWindow();




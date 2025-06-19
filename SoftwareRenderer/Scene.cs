namespace SoftwareRenderer;

public class Scene(int width, int height)
{
    public readonly int Width = width;
    public readonly int Height = height;
    
    public readonly List<Model> Models = new();
    
    public void AddModel(Model model)
    {
        Models.Add(model);
    }
}
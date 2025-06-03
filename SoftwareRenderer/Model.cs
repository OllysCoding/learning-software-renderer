namespace SoftwareRenderer;

public class Model
{
    public Float3[] vertices;
    public Tuple<int, int, int>[] faceVertices;
    
    public Model(Float3[] vertices, Tuple<int, int, int>[] faceVertices)
    {
        this.vertices = vertices;
        this.faceVertices = faceVertices;
    }
    
    
    public static Model readFromObjFile(String path)
    {
  
        using StreamReader reader = new(path);

        List<Float3> vertices = new List<Float3>();
        List<Tuple<int, int, int>> faceVertices = new List<Tuple<int, int, int>>();

        String? line;
        while ((line = reader.ReadLine()) != null)
        {
            switch (line.Split(" "))
            {
                case ["v", var x, var y, var z]:
                    vertices.Add(new Float3(float.Parse(x), float.Parse(y), float.Parse(z)));
                    break;
                case ["f", var x, var y, var z]:
                    faceVertices.Add(new Tuple<int, int, int>(int.Parse(x.Split("/")[0]) - 1, int.Parse(y.Split("/")[0]) - 1, int.Parse(z.Split("/")[0]) - 1));
                    break;
            }
        }
        
        return new Model(vertices.ToArray(), faceVertices.ToArray());
    }
}
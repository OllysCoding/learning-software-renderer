namespace SoftwareRenderer;

public class Model(Float3[] vertices, Tuple<int, int, int>[] faceVertices)
{
    public readonly Float3[] Vertices = vertices;
    public readonly Tuple<int, int, int>[] FaceVertices = faceVertices;

    public Tuple<Float3, Float3, Float3>[] GetTriangles()
    {
        var triangles = new Tuple<Float3, Float3, Float3>[FaceVertices.Length];
        for (int i = 0; i < FaceVertices.Length; i++)
        {
            triangles[i] = new(
                (Vertices[FaceVertices[i].Item1] + 1),
                (Vertices[FaceVertices[i].Item2] + 1),
                (Vertices[FaceVertices[i].Item3] + 1)
            );
        }

        return triangles;
    }
    
    public static Model ReadFromObjFile(String path)
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
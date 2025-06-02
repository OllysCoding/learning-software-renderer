namespace SoftwareRenderer;

public class Model
{
    public static Model readFromObjFile(String path)
    {
  
        using StreamReader reader = new(path);

        String? line;
        while ((line = reader.ReadLine()) != null)
        {
            // Process line
        }
        

    }
}
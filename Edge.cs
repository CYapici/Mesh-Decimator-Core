using System.Collections.Generic;

public class Edge
{
    public Vertex V1;
    public Vertex V2;
    public List<Triangle> FacesList = new List<Triangle>();


    public bool Contains(Vertex v)
    {
        return V1 == v || V2 == v;
    }
    public CustomVec3 HalfEdgePosition
    {
        get
        {
            return (V1.Source + V2.Source) / 2;
        }
    }

}

using System.Collections.Generic;

public class Vertex
{
    public CustomVec3 Source; 
    public int ID;        
    public List<Vertex> Neighbours = new List<Vertex>(); // adjacent vertices
    public List<Triangle> Faces = new List<Triangle>();     // adjacent triangles 
    public float CollapseCost; 
    public Vertex CollapseToCandidate; 
    public float VertexWeight = 0;  
    public int indexAt = 0; 
    public Vertex(CustomVec3 position, int _id, int place)
    {
        Source = position;
        ID = _id;
        indexAt = place; 
    }

    public void Delete(MeshDecimator owningMesh)
    {
        while (Neighbours.Count > 0)
        {
            Neighbours[0].Neighbours.Remove(this);
            Neighbours.Remove(Neighbours[0]);
        } 
        owningMesh.Vertices = RemoveIndices(owningMesh.Vertices, indexAt);
    }



    public void RemoveIfNonNeighbor(Vertex n)
    {
       
        if (!Neighbours.Contains(n)) return;
        for (int i = 0; i < Faces.Count; i++)
        {
            if (Faces[i].HasVertex(n)) return;
        }
        Neighbours.Remove(n);
    }

    private Vertex[] RemoveIndices(Vertex[] IndicesArray, int RemoveAt)
    {
        Vertex[] newIndicesArray = new Vertex[IndicesArray.Length - 1];

        int i = 0;
        int j = 0;
        while (i < IndicesArray.Length)
        {
            if (i != RemoveAt)
            {
                newIndicesArray[j] = IndicesArray[i];
                newIndicesArray[j].indexAt = j;
                j++;
            }

            i++;
        }

        return newIndicesArray;
    }
}

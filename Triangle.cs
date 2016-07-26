using System;

public class Triangle
{ 
    public Vertex[] Vertexes = new Vertex[0x3] { null, null, null };  
    public CustomVec3 Normal;                                
    public Edge[] Edges = new Edge[0x3] { null, null, null }; 
    public int indexAt = 0;   
   
    public void ReplaceVertex(Vertex vold, Vertex vnew)
    {

        if (vold == Vertexes[0])
        {
            Vertexes[0] = vnew;
        }
        else if (vold == Vertexes[1])
        {
            Vertexes[1] = vnew;
        }
        else
        {

            Vertexes[2] = vnew;
        }
        int i;

        vold.Faces.Remove(this);

        vnew.Faces.Add(this);


        for (i = 0; i < 3; i++)
        {
            vold.RemoveIfNonNeighbor(Vertexes[i]);
            Vertexes[i].RemoveIfNonNeighbor(vold);
        }
        for (i = 0; i < 3; i++)
        {

            for (int j = 0; j < 3; j++) if (i != j)
                {
                    Vertex tempVertexJ = Vertexes[j];

                    if (!Vertexes[i].Neighbours.Contains(tempVertexJ))
                    {
                        Vertexes[i].Neighbours.Add(tempVertexJ);
                    }
                }
        }
        ComputeNormal();
    }
    public bool HasVertex(Vertex v)
    {
        return (v == Vertexes[0] || v == Vertexes[1] || v == Vertexes[2]);
    }
    public void ComputeNormal()
    {
        Normal = ComputeNormal(Vertexes[0].Source, Vertexes[1].Source, Vertexes[2].Source);
    }

    public static bool IsWellFormed(CustomVec3 v0, CustomVec3 v1, CustomVec3 v2)
    {
        return v0 != v1 && v1 != v2 && v2 != v0;
    }

    public static CustomVec3 ComputeNormal(CustomVec3 v0, CustomVec3 v1, CustomVec3 v2)
    {
        return CustomVec3.CalculatePlaneNormal(v0, v1, v2);

    }

    public Triangle(Vertex v0, Vertex v1, Vertex v2)
    {
        Vertexes[0] = v0;
        Vertexes[1] = v1;
        Vertexes[2] = v2;
        ComputeNormal();

        // owningMesh.Triangles.Add(this);

        for (int i = 0; i < 3; i++)
        {
            Vertexes[i].Faces.Add(this);
            for (int j = 0; j < 3; j++) if (i != j)
                {
                    Vertex tempVertexJ = Vertexes[j];

                    if (!Vertexes[i].Neighbours.Contains(tempVertexJ))
                    {
                        Vertexes[i].Neighbours.Add(tempVertexJ);
                    }
                }
        }


        Edges[0] = new Edge() { V1 = v0, V2 = v1 };
        Edges[1] = new Edge() { V1 = v1, V2 = v2 };
        Edges[2] = new Edge() { V1 = v2, V2 = v0 };

    }

    public void Delete(MeshDecimator owningMesh)
    {
        int i;

        //owningMesh.Triangles.Remove(this);
        owningMesh.Triangles = RemoveIndices(owningMesh.Triangles, indexAt);

        for (i = 0; i < 3; i++)
        {
            if (Vertexes[i] != null) Vertexes[i].Faces.Remove(this);
        }
        for (i = 0; i < 3; i++)
        {
            int i2 = (i + 1) % 3;
            Vertex ii2Vertex = Vertexes[i2];
            Vertex ii1Vertex = Vertexes[i];
            if (ii1Vertex != null || ii2Vertex == null) continue;

            Vertexes[i].RemoveIfNonNeighbor(ii2Vertex);

            Vertexes[i2].RemoveIfNonNeighbor(ii1Vertex);
        }
    }

    private Triangle[] RemoveIndices(Triangle[] IndicesArray, int RemoveAt)
    {
        Triangle[] newIndicesArray = new Triangle[IndicesArray.Length - 1];

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

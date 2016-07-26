using System;
using System.Collections.Generic;
using System.Threading;
public class MeshDecimator
{

    public int triangleIdx = 0;
    public Dictionary<CustomVec3, Edge> Edges = new Dictionary<CustomVec3, Edge>();
    public Vertex[] Vertices;
    public int verticeIdx = 0;
    public Vertex[] PreCollapseVertices;
    public Triangle[] Triangles;
    private int dChange = 0;


    public Vertex MinCostEdge()
    {
        Vertex mn = Vertices[0];
        for (int i = 0; i < Vertices.Length; i++)
        {
            Vertex temp = Vertices[i];

            if (temp.CollapseCost < mn.CollapseCost)

                mn = temp;

        }
        return mn;
    }

    public void Generate()
    {
        CacheAllEdgeCollapseCosts();

        int len = Vertices.Length;
        PreCollapseVertices = new Vertex[len];
        for (int i = 0; i < len; i++)

            PreCollapseVertices[i] = Vertices[i];


        int targetCount = Vertices.Length - dChange;

        while (Vertices.Length > targetCount)
        {
            Vertex minimumCostVertex = MinCostEdge();
            Collapse(minimumCostVertex, minimumCostVertex.CollapseToCandidate);
        }


    }

    public MeshDecimator()
    {
        dChange = 20;
    }

    public void DecimateMesh(MeshData meshData)
    {

        UnityEngine.Vector3[] verticesArray = meshData.verticesArray;
        int[][] indicesArray = meshData.indicesArray;

        int vLen = verticesArray.Length;
        int tLen = indicesArray[0].Length;

        if (vLen > 100 && tLen > 0)
        {
            CustomVec3[] oldVerices = new CustomVec3[vLen];

            for (int i = 0; i < vLen; i++)
            {
                UnityEngine.Vector3 temp = verticesArray[i];
                oldVerices[i] = (new CustomVec3(temp.x, temp.y, temp.z));

            }


            List<TriangleIdxArray> triangles = new List<TriangleIdxArray>();

            if (indicesArray[0] != null)
            {
                int i;

                for (i = 0; i < indicesArray[0].Length; i += 3)
                {
                    TriangleIdxArray uu = new TriangleIdxArray();
                    int[] temp = indicesArray[0];
                    uu.VertexIndex[0] = temp[i];
                    uu.VertexIndex[1] = temp[i + 1];
                    uu.VertexIndex[2] = temp[i + 2];
                    triangles.Add(uu);

                }
            }

            AddVertex(oldVerices);
            AddFaces(triangles);


            int cached = Triangles.Length;
            Generate();


            int generatedLenV = Vertices.Length;
            UnityEngine.Vector3[] vAr = new UnityEngine.Vector3[generatedLenV];

            CustomVec3 item;
            for (int i = 0; i < generatedLenV; i++)
            {
                item = Vertices[i].Source;
                vAr[i] = new UnityEngine.Vector3(item.X, item.Y, item.Z);
            }


            int generatedTrilen = Triangles.Length;
            int[] verticesArr = new int[generatedTrilen * 3];
            int localIdx = 0;
            Vertex[] temp1;

            for (int i = 0; i < generatedTrilen; i++)
            {
                temp1 = Triangles[i].Vertexes;
                verticesArr[localIdx] = temp1[0].ID;
                verticesArr[localIdx + 1] = temp1[1].ID;
                verticesArr[localIdx + 2] = temp1[2].ID;
                localIdx += 3;
            }

            meshData.verticesArray = vAr;
            meshData.indicesArray[0] = verticesArr;
            // Precompute arrays from lists
            meshData.PrepareArrays();
        }

    }

    internal Edge AddEdges(Edge edge, Triangle borderingTriangle)
    {
        Edge storedEdge = null;
        if (!Edges.TryGetValue(edge.HalfEdgePosition, out storedEdge))
        {
            Edges.Add(edge.HalfEdgePosition, edge);
            storedEdge = edge;
        }
        storedEdge.FacesList.Add(borderingTriangle);
        return storedEdge;
    }

    public void ComputeVertWeights(Func<Vertex, float> getVertexWeight)
    {

        for (int index = 0; index < Vertices.Length; index++)
        {
            Vertex v = Vertices[index];
            v.VertexWeight += getVertexWeight(v);
        }
    } 
     
    
    public float ComputeEdgeCollapseCost(Vertex moveFrom, Vertex moveTo)
    {
        int i;
        float edgelength = (moveTo.Source - moveFrom.Source).Length();
        float curvature = 0;
        List<Triangle> sides = new List<Triangle>();
        int lenFace = moveFrom.Faces.Count;
        for (i = 0; i < lenFace; i++)
        {
            if (moveFrom.Faces[i].HasVertex(moveTo))
            {
                sides.Add(moveFrom.Faces[i]);
            }
        }
        lenFace = moveFrom.Faces.Count;
        for (i = 0; i < lenFace; i++)
        {
            float mincurv = 1;  
            int lenSide = sides.Count;
            for (int j = 0; j < lenSide; j++)
            {
              
                float dotprod = CustomVec3.Dot(moveFrom.Faces[i].Normal, sides[j].Normal);
                //   dot product with face normals. '^' defined in vector  
                mincurv = Math.Min(mincurv, (1 - dotprod) / 2.0f);

            }

            curvature = Math.Max(curvature, mincurv);
        }
        float standardChangeFactor = edgelength * curvature;
        List<Triangle> adjacentTriangles = new List<Triangle>();
        adjacentTriangles.AddRange(moveFrom.Faces);
        CustomVec3 existingTriangle0;
        CustomVec3 existingTriangle1;
        CustomVec3 existingTriangle2;
        Triangle adjacentTriangle;
        CustomVec3 newTriangle0;
        CustomVec3 newTriangle1;
        CustomVec3 newTriangle2;
      
        for (int index = 0; index < adjacentTriangles.Count; index++)
        {
            adjacentTriangle = adjacentTriangles[index];
            existingTriangle0 = adjacentTriangle.Vertexes[0].Source;
            existingTriangle1 = adjacentTriangle.Vertexes[1].Source;
            existingTriangle2 = adjacentTriangle.Vertexes[2].Source;


            newTriangle0 = adjacentTriangle.Vertexes[0].Source;
            newTriangle1 = adjacentTriangle.Vertexes[1].Source;
            newTriangle2 = adjacentTriangle.Vertexes[2].Source;

            
            if (existingTriangle0 == moveFrom.Source) newTriangle0 = moveTo.Source;
            if (existingTriangle1 == moveFrom.Source) newTriangle1 = moveTo.Source;
            if (existingTriangle2 == moveFrom.Source) newTriangle2 = moveTo.Source;
 
            if (Triangle.IsWellFormed(newTriangle0, newTriangle1, newTriangle2))
            {
                CustomVec3 currentFaceNormal = Triangle.ComputeNormal(existingTriangle0, existingTriangle1, existingTriangle2);
                CustomVec3 newFaceNormal = Triangle.ComputeNormal(newTriangle0, newTriangle1, newTriangle2);
                
                if (CustomVec3.Dot(currentFaceNormal, newFaceNormal) == -1)
                {
                    // Must not let this occur.
                    standardChangeFactor = float.MaxValue;
                    break;
                }
            }
            else
            {
                 
                Vertex deletedVertex = null;
                CustomVec3 sourceFrom = moveFrom.Source;
                CustomVec3 sourceTo = moveTo.Source;

                if (newTriangle0 != sourceTo && newTriangle0 != sourceFrom)
                
                    
                    deletedVertex = adjacentTriangle.Vertexes[0];
                 
                if (newTriangle1 != sourceTo && newTriangle1 != sourceFrom)
                 
                  
                    deletedVertex = adjacentTriangle.Vertexes[1];
                
                if (newTriangle2 != sourceTo && newTriangle2 != sourceFrom)
                
                   
                    deletedVertex = adjacentTriangle.Vertexes[2];
               

                standardChangeFactor += deletedVertex.VertexWeight;
            }
        }

        return standardChangeFactor;
    }
     
    public void ComputeEdgeCostAtVertex(Vertex v)
    {
        
        if (v.Neighbours.Count == 0)
        { 
            v.CollapseToCandidate = null;
            v.CollapseCost = -0.01f;
            return;
        }
        v.CollapseCost = float.MaxValue;
        v.CollapseToCandidate = null;

       
        for (int i = 0; i < v.Neighbours.Count; i++)
        {
            Vertex temp = v.Neighbours[i];


            float dist;
            dist = ComputeEdgeCollapseCost(v, temp);
            if (dist < v.CollapseCost)
            {
                v.CollapseToCandidate = temp;  // candidate for edge collapse
                v.CollapseCost = dist;             // cost of the collapse
            }
        }
         
    }

    public void CacheAllEdgeCollapseCosts()
    {
        for (int i = 0; i < Vertices.Length; i++)
        {
            Vertex temp = Vertices[i];
            ComputeEdgeCostAtVertex(temp);
        }
    }
 
    public void Collapse(Vertex fromVertex, Vertex toVertex)
    {
        if (toVertex == null)
        {
            fromVertex.Delete(this);
            return;
        }
        int i;
        List<Vertex> tmp = new List<Vertex>();
        int len = fromVertex.Neighbours.Count;

        for (i = 0; i < len; i++)
        {
            tmp.Add(fromVertex.Neighbours[i]);
        }

        int len1 = fromVertex.Faces.Count - 1;

        for (i = len1; i >= 0; i--)
        {
            if (fromVertex.Faces[i].HasVertex(toVertex))
            {
                fromVertex.Faces[i].Delete(this);
            }
        }
        len1 = fromVertex.Faces.Count - 1;


        for (i = len1; i >= 0; i--)
        {
            fromVertex.Faces[i].ReplaceVertex(fromVertex, toVertex);
        }

        fromVertex.Delete(this);

        for (i = 0; i < tmp.Count; i++)
        {
            ComputeEdgeCostAtVertex(tmp[i]);
        }
    }
     
    public void AddVertex(CustomVec3[] vert)
    {
        int len = vert.Length;
        Vertices = new Vertex[len];

        for (int i = 0; i < len; i++)

            Vertices[i] = new Vertex(vert[i], i, i);
    }
      
    public void AddFaces(List<TriangleIdxArray> tri)
    {
        int trilen = tri.Count;

        Triangles = new Triangle[trilen]; 

        for (int i = 0; i < trilen; i++)
        {
            int[] temp = tri[i].VertexIndex;
            Vertex idx0 = Vertices[temp[0]];
            Vertex idx1 = Vertices[temp[1]];
            Vertex idx2 = Vertices[temp[2]];

            if ((idx0.Source != idx1.Source && idx1.Source != idx2.Source && idx2.Source != idx0.Source))
            {
                Triangle tempTri = new Triangle(idx0, idx1, idx2);
                Triangles[i] = tempTri;
                Edge[] tempEdge = tempTri.Edges;

                for (int j = 0; j < 3; j++)

                    AddEdges(tempEdge[j], tempTri); 
            }

        }
    }
 

}

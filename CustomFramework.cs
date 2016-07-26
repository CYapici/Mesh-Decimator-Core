using System;

public class CustomFramework
{


    public static bool contains(Vertex[] IndicesArray, int RemoveAt)
    {
        return Array.BinarySearch(IndicesArray, RemoveAt) >= 0;
    }
    public static Triangle[] RemoveIndicesTri(Triangle[] IndicesArray, int RemoveAt)
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

    public static Vertex[] RemoveIndicesVert(Vertex[] IndicesArray, int RemoveAt)
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

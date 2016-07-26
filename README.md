Mesh Decimator C# 
====
[![Project Status](http://opensource.box.com/badges/active.svg)](http://opensource.box.com/badges)
[![Project Status](http://opensource.box.com/badges/maintenance.svg)](http://opensource.box.com/badges)
[![Average time to resolve an issue](http://isitmaintained.com/badge/resolution/major/MinHeap Java-perl.svg)](http://isitmaintained.com/project/major/MinHeap Java-perl "Average time to resolve an issue")
[![Percentage of issues still open](http://isitmaintained.com/badge/open/major/MinHeap Java-perl.svg)](http://isitmaintained.com/project/major/MinHeap Java-perl "Percentage of issues still open")

THIS PROJECT HAS BEEN CONVERTED FROM   http://www.melax.com/polychop which is c++     ...
 
 
```  
The dT variable is  desired decimation count to subract from the vertices array.

Custom Vector3 class created in the purpose of using the decimator in different threads because of cpu overhead alternatively  you 
can use XNA Vector3 or UnityEngine Vector3

MeshData class is the adapter class and  transformation class to use MeshDecimator

        class MeshData
    {
        //use your mesh structure including vertexes triangles uvs note that 
        //decimator does not care about color uv other mesh arrays( structures) etc..
        //this app just changes vertice and triangle count
        //be aware of remaining mesh structures must be same size to converted 
    }
 
``` 
```

BASICLY COMPUTES COLLAPSE COSTS IN THE START AND CACHES IT 

FOR EACH VERTEX SELECTS THE MINIMUM COSTED VERTEX FOR IT THEN BASICLY REPLACES THE OLDER ONE WITH NEW THEN DELETES 

ELDER ONE TO COLLAPSE DATA LENGTH. 
```	 
  
Mesh Decimator c# needs you
 
**Mesh Decimator c#** needs contributors for documentation, code and feedbacks..
 
* Contribution guide is avalaible following [MinHeap Java contributing guide](#)
 

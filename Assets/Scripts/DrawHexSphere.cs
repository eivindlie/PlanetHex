using System.Collections;
using System.Collections.Generic;

using PlanetGeneration;

using UnityEngine;

public class DrawHexSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var hexasphere = new HexasphereGenerator(1.0f, 2, 1.0f).Generate();

        var verts = new List<Vector3>();
        var tris = new List<int>();
        var i = 0;
        
        foreach (var tile in hexasphere.Tiles)
        {
            foreach (var point in tile.Boundary)
            {
                verts.Add(point.AsVector());
            }
            
            tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
            tris.Add(i + 2); tris.Add(i + 3); tris.Add(i + 4);
            tris.Add(i + 4); tris.Add(i); tris.Add(i + 2);
            if (tile.Boundary.Count > 5)
            {
                tris.Add(i + 4); tris.Add(i + 5); tris.Add(i);
            }
            
            i += tile.Boundary.Count;
        }
        
        var mesh = new Mesh()
        {
            vertices = verts.ToArray(),
            triangles = tris.ToArray()
        };

        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

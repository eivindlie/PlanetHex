using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlanetGeneration;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour {

    public int TerrainHeight = 5;
    public float radius = 30f;
    private SimplexNoiseGenerator NoiseGenerator = new SimplexNoiseGenerator();

    // Use this for initialization
    void Start() {
        Hexasphere hexasphere = new Hexasphere(radius, 30);

        var verts = new List<Vector3>();
        var tris = new List<int>();
        var i = 0;
        foreach(Region region in hexasphere.Regions)
        {
            foreach (Tile tile in region.GetTiles())
            {
                foreach (var point in tile.Boundary)
                {
                    var height = Mathf.Max(TerrainHeight - NoiseGenerator.getDensity(point.AsVector(), 0, TerrainHeight, octaves: 3, persistence: 0.85f), 0);
                    verts.Add(point.Project(height).AsVector());
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
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlanetGeneration;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCreator : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Mesh mesh = new Hexasphere(30, 20, 0.95f).GetMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        /*mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();*/
        //GetComponent<MeshCollider>().sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

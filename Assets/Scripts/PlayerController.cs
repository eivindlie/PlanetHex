using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlanetGeneration;

public class PlayerController : MonoBehaviour {

    private GameObject SelectedObject;
    private PlanetGenerator PlanetGenerator;
    public GameObject Planet;
    public Camera Camera;

	void Start () {
        PlanetGenerator = Planet.GetComponent<PlanetGenerator>();
	}
	
	void Update () {
        SetSelectedObject();
		if (Input.GetButtonDown("Fire1"))
        {
            if(SelectedObject != null)
            {
                PlanetGenerator.RemoveBlock(SelectedObject);
            }
        }
	}

    void SetSelectedObject()
    {
        RaycastHit hitInfo;
        var hit = Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitInfo, maxDistance: 4f);
        GameObject hitObject;
        if (hit) {
            hitObject = hitInfo.transform.gameObject;
            SelectedObject = hitObject;
        }
        else
        {
            SelectedObject = null;
        }
    }
}

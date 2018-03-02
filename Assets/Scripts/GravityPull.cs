using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPull : MonoBehaviour {

    private static double gravityConstant = 0.1;

    public GameObject gravityCenter;

    private PlanetController planetController;

    Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        this.rigidbody = GetComponent<Rigidbody>();
        this.planetController = gravityCenter.GetComponent<PlanetController>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        var r = gravityCenter.transform.position - transform.position;
        var direction = r.normalized;
        var distance = r.magnitude;
        var g = (float) (gravityConstant * planetController.Mass * rigidbody.mass) / Mathf.Pow(distance, 2);
        rigidbody.AddForce(direction * g * rigidbody.mass);
	}
}

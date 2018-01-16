using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPull : MonoBehaviour {

    public GameObject gravityCenter;

    Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        this.rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        var direction = (gravityCenter.transform.position - transform.position).normalized;
        rigidbody.AddForce(direction * 9.81f * rigidbody.mass);
	}
}

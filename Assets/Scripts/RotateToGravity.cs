using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToGravity : MonoBehaviour {

    public GameObject gravityCenter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var up = (transform.position - gravityCenter.transform.position).normalized;
        transform.up = up;
        /*var angle = Vector3.Angle(up, transform.up);
        var newForward = Quaternion.AngleAxis(angle, Vector3.Cross(transform.up, up)) * transform.forward;
        transform.rotation = Quaternion.LookRotation(transform.forward, up);*/
    }
}

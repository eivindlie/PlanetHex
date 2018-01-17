using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour {

    Rigidbody rigidbody;
    public float speed;
    public float jumpForce;
    public float GroundDistance;

    public Camera camera;
    public float mouseSensitivity = 100.0f;
    public float mouseClampAngle = 80.0f;

    public GameObject gravityCenter;

    void Start () {
        this.rigidbody = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	void Update () {
        //transform.Translate(transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime);
        //transform.Translate(transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);

        if(true || IsGrounded())
        {
            rigidbody.AddForce(transform.forward * Input.GetAxis("Vertical") * speed);
            rigidbody.AddForce(transform.right * Input.GetAxis("Horizontal") * speed);
        }

        if (Input.GetAxis("Jump") > 0 && IsGrounded())
        {
            rigidbody.AddForce(jumpForce * transform.up);
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        var rotX = camera.transform.localRotation.eulerAngles.x;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -mouseClampAngle, mouseClampAngle);
        camera.transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        var rotY = mouseX * mouseSensitivity * Time.deltaTime;
        var newUp = (transform.position - gravityCenter.transform.position).normalized;
        var newForward = (Quaternion.AngleAxis(rotY, transform.up) * transform.forward);
        newForward = (newForward - Vector3.Project(newForward, newUp)).normalized;
        transform.rotation = Quaternion.LookRotation(newForward, newUp);
	}

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, GroundDistance + 0.1f);
    }
}

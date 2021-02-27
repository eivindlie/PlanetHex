using UnityEngine;

namespace Behaviours.Character
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseSensitivity = 100.0f;
        public float clampAngle = 80.0f;
        public new Camera camera;

        private float _rotY = 0.0f; // rotation around the up/y axis
        private float _rotX = 0.0f; // rotation around the right/x axis

        void Start()
        {
            Vector3 rot = transform.localRotation.eulerAngles;
            _rotY = rot.y;
            _rotX = rot.x;
        }

        void Update()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            _rotY = mouseX * mouseSensitivity * Time.deltaTime;
            _rotX += mouseY * mouseSensitivity * Time.deltaTime;

            _rotX = Mathf.Clamp(_rotX, -clampAngle, clampAngle);

            Quaternion cameraRotation = Quaternion.Euler(_rotX, 0, 0.0f);
            var newForward = Quaternion.AngleAxis(_rotY, transform.up) * transform.forward;
            Quaternion lookRotation = Quaternion.LookRotation(newForward, transform.up);
            camera.transform.localRotation = cameraRotation;
            transform.rotation = lookRotation;
        }
    }
}
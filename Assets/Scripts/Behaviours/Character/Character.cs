using UnityEngine;

namespace Behaviours.Character
{
    public class Character : MonoBehaviour
    {
        public GameObject gravitySource;
        public Camera camera;

        private Vector3 _gravitySource;
        private Rigidbody _rigidbody;
        private const float WalkSpeed = 10f;
        private const float RunSpeed = 15f;
        private const float JumpForce = 1000;
        private const float MouseSpeed = 3;
        
        private Vector2 _rotation = new Vector2(0, 0);

        private void Start()
        {
            _gravitySource = gravitySource == null ? new Vector3(0, 0, 0) : gravitySource.transform.position;
            _rigidbody = GetComponent<Rigidbody>();

            var trans = transform;
            RotateToUprightPosition(trans);
            camera.transform.rotation = Quaternion.LookRotation(trans.forward, trans.up);
        }

        private void Update()
        {
            var trans = transform;
            RotateToUprightPosition(trans);
            HandleMovement(trans);
        }

        private void RotateToUprightPosition(Transform trans)
        {
            var up = (trans.position - _gravitySource).normalized;
            var forward = trans.forward;
            forward = (forward - Vector3.Dot(forward, up) * up).normalized;
            trans.rotation = Quaternion.LookRotation(forward, up);
        }

        private void HandleMovement(Transform trans)
        {
            var speed = (Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed) * Time.deltaTime;
            
            if (Input.GetKey(KeyCode.A))
            {
                trans.position -= trans.right * speed;
            }
        
            if (Input.GetKey(KeyCode.D))
            {
                trans.position += trans.right * speed;
            }
        
            if (Input.GetKey(KeyCode.W))
            {
                trans.position += trans.forward * speed;
            }
        
            if (Input.GetKey(KeyCode.S))
            {
                trans.position -= trans.forward * speed;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(trans.up * (JumpForce * _rigidbody.mass));
            }
        }

        private void HandleMouseMove(Transform trans)
        {
            _rotation.y += Input.GetAxis("Mouse X");
            _rotation.x += -Input.GetAxis("Mouse Y");

            var cameraRotation = new Vector2(_rotation.x, 0);
            var characterRotation = new Vector2(0, _rotation.y);
            trans.eulerAngles = characterRotation * MouseSpeed;
            camera.transform.eulerAngles = cameraRotation * MouseSpeed;
        }
    }
}

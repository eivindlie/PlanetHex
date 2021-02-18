using UnityEngine;

namespace Behaviours
{
    public class GravityObject : MonoBehaviour
    {
        public GameObject gravitySource;
        public float gravityStrength = 9.81f;

        private Vector3 _gravitySource;
    
        private Rigidbody _rigidbody;
    
        public void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _gravitySource = gravitySource == null ? new Vector3(0, 0, 0) : gravitySource.transform.position;
        }

        private void Update()
        {
            var direction = (_gravitySource - transform.position).normalized;
            _rigidbody.AddForce(direction * (gravityStrength * _rigidbody.mass));
        }
    }
}

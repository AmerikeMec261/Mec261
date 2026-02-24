using UnityEngine;

public class MoveCubeOnIncline : MonoBehaviour
{
    
        public Rigidbody rb;
        public float forceAmount = 3000f; 

        void FixedUpdate()
        {
            if (rb == null) rb = GetComponent<Rigidbody>();

            
            Vector3 upSlope = Vector3.ProjectOnPlane(-Physics.gravity, transform.up).normalized;
            rb.AddForce(upSlope * forceAmount, ForceMode.Force);
        }
    
}


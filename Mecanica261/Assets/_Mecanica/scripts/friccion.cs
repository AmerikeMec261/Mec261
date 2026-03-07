using UnityEngine;

public class friccion : MonoBehaviour
{
    Rigidbody rb;
    public float force = 2148f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * force);
    }
}

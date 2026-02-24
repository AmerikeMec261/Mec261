using UnityEngine;

public class ForceSim : MonoBehaviour
{
    private Rigidbody rb;
    public float fuerzaAplicada = 2150f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
       
        rb.AddRelativeForce(Vector3.forward * fuerzaAplicada);
    }
}

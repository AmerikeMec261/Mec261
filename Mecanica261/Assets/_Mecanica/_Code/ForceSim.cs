using UnityEngine;

public class ForceSim : MonoBehaviour
{
    private Rigidbody rb;
    public float fuerzaAplicada = 2143f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
       
        rb.AddRelativeForce(Vector3.right * fuerzaAplicada);
    }
}

using UnityEngine;

public class cuboarriba : MonoBehaviour
{
    public Rigidbody rb;
    public float forceAmount = 3000f; // ajusta en el Inspector

    void FixedUpdate()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        // Dirección "hacia arriba" sobre el plano: opuesto a la gravedad proyectada
        Vector3 upSlope = Vector3.ProjectOnPlane(-Physics.gravity, transform.up).normalized;
        rb.AddForce(upSlope * forceAmount, ForceMode.Force);
    }
}

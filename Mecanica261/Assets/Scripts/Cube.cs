using UnityEngine;

public class PlanoInclinado : MonoBehaviour
{
    public float masa = 200f;
    public float angulo = 20f;
    public float gravedad = 9.81f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float fuerza = masa * gravedad * Mathf.Sin(angulo * Mathf.Deg2Rad);

        Vector3 direccion = Vector3.down;
        direccion = Vector3.ProjectOnPlane(direccion, transform.up).normalized;

        rb.AddForce(direccion * fuerza);
    }
}

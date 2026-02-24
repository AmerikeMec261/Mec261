using UnityEngine;

public class LanzamientoFisico : MonoBehaviour
{
    public float velocidadInicial = 20f;
    public float angulo = 45f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float anguloRad = angulo * Mathf.Deg2Rad;

        float velocidadX = velocidadInicial * Mathf.Cos(anguloRad);
        float velocidadY = velocidadInicial * Mathf.Sin(anguloRad);

        Vector3 velocidadFinal = new Vector3(velocidadX, velocidadY, 0);

        rb.linearVelocity = velocidadFinal;
        rb.AddTorque(new Vector3(0, 0, 5f), ForceMode.Impulse);
    }
}

using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject pelotaPrefab;   // Prefab azul de la pelota
    public Transform aro;             // El objeto del aro (Torus)
    public float fuerza = 25f;        // Fuerza del disparo
    public float alturaExtra = 3f;    // Qué tan alto sube antes de caer

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Disparar();
        }
    }

    void Disparar()
    {
        // Crear nueva pelota
        GameObject nuevaPelota = Instantiate(
            pelotaPrefab,
            transform.position,
            Quaternion.identity
        );

        // Obtener Rigidbody
        Rigidbody rb = nuevaPelota.GetComponent<Rigidbody>();

        // Dirección hacia el aro
        Vector3 direccion = aro.position - transform.position;

        // Agregamos altura extra para hacer parábola
        direccion.y += alturaExtra;

        // Normalizamos
        direccion = direccion.normalized;

        // Aplicamos fuerza
        rb.AddForce(direccion * fuerza, ForceMode.Impulse);
    }
}

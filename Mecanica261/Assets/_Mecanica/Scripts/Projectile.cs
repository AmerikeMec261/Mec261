using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Physics")]
    [Tooltip("Velocidad inicial al disparar (m/s)")]
    public float speed = 200f;

    [Header("Life time")]
    [Tooltip("Segundos antes de que el proyectil se destruya si no golpea nada")]
    public float lifetime = 10f;

    // Referencia al Rigidbody del proyectil
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // transform.forward es la dirección hacia donde apunta el proyectil. Al asignarlo a linearVelocity, el proyectil sale disparado en esa direccion
        rb.linearVelocity = transform.forward * speed;

        // Destruye el proyectil después de 'lifetime' segundos si no golpea nada
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignorar colisiones con el propio barco que disparó. Puedes usar layers en Unity para esto
        if (collision.gameObject.CompareTag("Ship"))
            return;

        // Destruimos el proyectil al impactar
        Destroy(gameObject);
    }
}
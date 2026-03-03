using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject pelotaPrefab;

    
    public Vector3 centroAro = new Vector3(0f, 3f, 15f);

    public float fuerza = 25f;
    public float alturaExtra = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Disparar();
        }
    }

    void Disparar()
    {
        GameObject nuevaPelota = Instantiate(
            pelotaPrefab,
            transform.position,
            Quaternion.identity
        );

        Rigidbody rb = nuevaPelota.GetComponent<Rigidbody>();

        
        Vector3 direccion = centroAro - transform.position;

        direccion.y += alturaExtra;

        direccion = direccion.normalized;

        rb.AddForce(direccion * fuerza, ForceMode.Impulse);
    }
}

using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject pelotaPrefab;
    public Transform puntoDisparo;
    public float fuerza = 15f;

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
            puntoDisparo.position,
            puntoDisparo.rotation
        );

        Rigidbody rb = nuevaPelota.GetComponent<Rigidbody>();
        rb.AddForce(puntoDisparo.forward * fuerza, ForceMode.Impulse);
    }
}

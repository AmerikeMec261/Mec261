using UnityEngine;

public class Tiro : MonoBehaviour
{

    public GameObject pelotaPrefab;
    public Transform puntodelanzamiento;
    public float fuerzadelanzamiento=30f;

     void Update()
    {
        if (Input.GetButtonDown("Disparo1") && pelotaPrefab)
        {
            GameObject ball=Instantiate(pelotaPrefab, puntodelanzamiento.position,fuerzadelanzamiento.rotation);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward*fuerzadelanzamiento+Vector3.up*10f,ForceMode.Impulse);
        }
    }
}

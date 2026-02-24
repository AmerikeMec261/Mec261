using UnityEngine;

public class Fuerza : MonoBehaviour
{
    private Rigidbody rb;
    public float fuerzaaplicada = 2147f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.right * fuerzaaplicada);
    }
}

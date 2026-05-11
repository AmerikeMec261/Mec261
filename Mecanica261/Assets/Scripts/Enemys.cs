using UnityEngine;

public class Enemys : MonoBehaviour 
{
    public float health = 100;
    public float speed = 3f;
    public float moveRange = 5f;

    private float StartX;
    private int direction = 1;

    void Start()
    {
        StartX = transform.position.x;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.freezeRotation = true;
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        if (transform.position.x > StartX + moveRange)
        {
            direction = -1;
        }

        if (transform.position.x < StartX - moveRange)
        {
            direction = 1;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Vida Enemigo: " + health);

        if (health <= 0)
        {
            Debug.Log("¡Enemigo eliminado!");
            Destroy(gameObject);
        }
    }
}
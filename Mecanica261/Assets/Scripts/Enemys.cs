using UnityEngine;

public class Enemys : MonoBehaviour
{
    public float health = 100;
    public float speed = 3f;
    public float moveRange = 5f;

    float StartX;
    int direction = 1;

    void Start()
    {
        StartX = transform.position.x;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Bullet") || collision.gameObject.tag == "Bullet")
        {
            health = health - 20f;
            Debug.Log("Enemy health: " + health);

            // Destroy the bullet on impact
            Destroy(collision.gameObject);

            if (health <= 0)
            {
                Debug.Log("Enemy destroyed!");
                Destroy(gameObject);
            }
        }
    }
}
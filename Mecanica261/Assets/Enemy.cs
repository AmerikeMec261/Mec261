using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float speed;

    [Header("Movement")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Transform target;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        Debug.Log(gameObject.name + " Vida restante: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

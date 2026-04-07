using UnityEngine;

public class ExplosiveBullet : MonoBehaviour, IProjectile
{
    [SerializeField] private float speed;
    public float Speed => speed;

    [SerializeField] private float damage;
    public float Damage => damage;

    [SerializeField] private float explosionRadius;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 velocity)
    {
        rb.AddForce(velocity, ForceMode.Impulse);
    }

    public void DealDamage(GameObject target)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponent<IDamageable>();

            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }
}

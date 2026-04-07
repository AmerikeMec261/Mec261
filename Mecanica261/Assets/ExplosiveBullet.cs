using UnityEngine;

public class ExplosiveBullet : MonoBehaviour, IProjectile
{
    private Rigidbody rb;

    [Header("Stats")]
    [SerializeField] private float damage = 50f;
    [SerializeField] private float speed = 25f;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 500f;

    
    public float Damage => damage;
    public float Speed => speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearby in colliders)
        {
            
            if (nearby.TryGetComponent<Rigidbody>(out Rigidbody nearbyRb))
            {
                nearbyRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            
            DealDamage(nearby.gameObject);
        }

        Destroy(gameObject);
    }

    
    public void DealDamage(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}

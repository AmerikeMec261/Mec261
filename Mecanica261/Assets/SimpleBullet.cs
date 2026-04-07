using UnityEngine;

public class SimpleBullet : MonoBehaviour, IProjectile
{
    [SerializeField] private float speed;
    public float Speed => speed;

    [SerializeField] private float damage;
    public float Damage => damage;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public void DealDamage(GameObject target)
    {
        IDamageable dmg = target.GetComponent<IDamageable>();

        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }
}

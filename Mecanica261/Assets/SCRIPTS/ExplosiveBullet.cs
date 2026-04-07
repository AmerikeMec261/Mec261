using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplosiveBullet : MonoBehaviour, IProjectile
{

    [Header("Settings")]
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _damage = 50f;
    [SerializeField] private float _explosionRadius = 5f;

    private Rigidbody _rigidbody;
    public float Speed => _speed;
    public float Damage => _damage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Collider[] turretColliders = GameObject.FindGameObjectWithTag("Turret")
                                               .GetComponentsInChildren<Collider>();
        Collider bulletCollider = GetComponent<Collider>();

        foreach (Collider turretCollider in turretColliders)
        {
            Physics.IgnoreCollision(bulletCollider, turretCollider);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        Destroy(gameObject);
    }

    public void Fire()
    {
        _rigidbody.linearVelocity = transform.forward * _speed;
    }

    public void DealDamage(IDamageable target)
    {
        target.ReceiveDamage(_damage);
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null) { DealDamage(damageable); }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}


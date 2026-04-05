using UnityEngine;

public class explosivebullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _speed = 60f;
    [SerializeField] private float _damage = 50f;

    [Header("Explosion Settings")]
    [SerializeField] private float _explosionRadius = 5f;

    public float Speed => _speed;
    public float Damage => _damage;

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }
    
    public void DealDamage(IDamageable target)
    {
        if (target != null)
        {
            target.TakeDamage(_damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            IDamageable damageableTarget = nearbyObject.GetComponent<IDamageable>();
            
            if (damageableTarget != null)
            {
                DealDamage(damageableTarget);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}

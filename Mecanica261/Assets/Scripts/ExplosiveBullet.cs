using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class ExplosiveBullet : MonoBehaviour, IProjectile
{

    [Header("Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _range = 3f;

    private Rigidbody _rb;

    public float Speed => _speed; 
    public float Damage => _damage;

    public void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 launchVelocity)
    {
        _rb.linearVelocity = launchVelocity;
    }

    public void DealDamage(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ReceiveDamage(_damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _range);
        foreach (Collider hit in hits)
        {
           DealDamage(hit);
        }
        Destroy(gameObject);
    }
}

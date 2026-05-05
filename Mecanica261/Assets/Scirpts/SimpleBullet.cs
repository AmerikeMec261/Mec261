using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SimpleBullet : MonoBehaviour, IProjectile
{

    [Header("Settings")]
    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _damage = 25f;
    [SerializeField] private float _destroyHeight = -10f;

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

    private void Update()
    {
        if (transform.position.y < _destroyHeight)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"La bala chocó con: {collision.gameObject.name}");
        IDamageable damageableTarget = collision.gameObject.GetComponent<IDamageable>();

        if (damageableTarget != null)
        {
            DealDamage(damageableTarget);
        }

        Destroy(gameObject);
    }
}

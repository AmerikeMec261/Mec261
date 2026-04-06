using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 30f;
    [SerializeField] private float _damage = 10f;

    private Rigidbody _rb;

    public float Damage => _damage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void Fire()
    {
        _rb.linearVelocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}
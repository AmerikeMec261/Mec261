using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SimpleBullet : MonoBehaviour, IProjectile
{
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _lifeTime = 5f;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _lifeTime);
    }

    public void Fire()
    {
        if (_rb != null)
        {
            _rb.linearVelocity = transform.forward * _speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemys enemy = collision.gameObject.GetComponent<Enemys>();

        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject); 
        }
    }
}

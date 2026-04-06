using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 20f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    public void DealDamage()
    {
        Debug.Log("Damage: " + _damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable enemy = collision.gameObject.GetComponent<IDamageable>();

        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
        }

        DealDamage();
        Destroy(gameObject);
    }
}
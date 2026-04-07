using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplosiveBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private GameObject _explosionVisual;

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
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider col in hits)
        {
            IDamageable dmg = col.GetComponent<IDamageable>();

            if (dmg != null)
            {
                dmg.TakeDamage(_damage);
            }
        }
        GameObject fx = Instantiate(_explosionVisual, transform.position, Quaternion.identity);
        fx.transform.localScale = Vector3.one * _radius * 2f;

        Destroy(fx, 0.5f);
    }

}

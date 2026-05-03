using UnityEngine;

public class ExplosiveBullet : MonoBehaviour, IProjectile

{
    [Header("Projectile Settings")]
    [SerializeField] private float _velocity = 15f;
    [SerializeField] private float _damagee = 40f;
    [SerializeField] private float _explosiveRadius= 5f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        _rigidbody.AddForce(transform.forward * _velocity, ForceMode.Impulse);
    }

    public void Impact()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosiveRadius);

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.DamageEnemy(_damagee);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Impact();
        Destroy(gameObject);
    }
}
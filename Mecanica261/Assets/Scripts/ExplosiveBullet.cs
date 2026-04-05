using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplosiveBullet : MonoBehaviour, IProjectile
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _damage = 40f;
    [SerializeField] private float _radius = 5f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Fire()
    {
        _rb.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }

    public void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider hit in hits)
        {
            IDamageable enemy = hit.GetComponent<IDamageable>();

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }
        }

        Debug.Log("Explosión: " + _damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamage();
        Destroy(gameObject);
    }
}
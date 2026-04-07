using UnityEngine;
using static IDamage;

[RequireComponent(typeof(Rigidbody))]
public class ExplosiveBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 13f;
    [SerializeField] private float _explosionArea = 10f;

    private float _damage;
    public void SetDamage(float amount)
    {
        _damage = amount;
    }

    public void Fire()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(-transform.forward * _speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionArea);
        IDamageable victim = other.GetComponent<IDamageable>();

        if (victim != null)
        {
            victim.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }


}

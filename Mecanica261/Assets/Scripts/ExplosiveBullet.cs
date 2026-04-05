 using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ExplosiveBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 13f;
    [SerializeField] private float _damage = 30f;
    [SerializeField] private float _explosionArea = 10f;

    public float Speed => _speed;
    public float Damage => _damage;

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
        Destroy(gameObject, 3f);
    }

    public void DealDamage(float amount)
    {
        Debug.Log($"Cayó el misil , {amount} daño hecho");

    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionArea);

        foreach (Collider hit in hits)
        {
            IDamageable damage = hit.GetComponent<IDamageable>();
            if (damage != null)
            {
                damage.ReciveDamage(_damage);
                DealDamage(_damage);


            }

        }

        Destroy(gameObject);

    }

}

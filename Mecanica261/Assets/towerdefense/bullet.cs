using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class bullet : MonoBehaviour, IProjectile
{
   [Header("Settings")]
    [SerializeField] private float _speed  = 20f;
    [SerializeField] private float _damage = 10f;

    public float Speed => _speed;
    public float Damage => _damage;

    public void Fire()
    {
        
    }

    public void DealDamage(IDamageable target)
    {
        target.TakeDamage(_damage);
    }
 
    public void OnHit()
    {
        Destroy(gameObject);
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable objetivo = collision.gameObject.GetComponent<IDamageable>();
 
        if (objetivo != null)
            DealDamage(objetivo);
 
        OnHit();
    }
}

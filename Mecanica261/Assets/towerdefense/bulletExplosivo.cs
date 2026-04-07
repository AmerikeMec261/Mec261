using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class bulletExplosivo : MonoBehaviour, IProjectile
{

   [Header("Settings")]
    [SerializeField] private float _speed          = 15f;
    [SerializeField] private float _damage         = 25f;
    [SerializeField] private float _radioExplosion = 5f;
 
    public float Speed  => _speed;
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
        Collider[] objetivos = Physics.OverlapSphere(transform.position, _radioExplosion);
 
        foreach (Collider col in objetivos)
        {
            IDamageable objetivo = col.GetComponent<IDamageable>();
 
            if (objetivo != null)
                DealDamage(objetivo);
        }
 
        Destroy(gameObject);
    }
 
    private void OnCollisionEnter(Collision collision)
    {
        OnHit();
    }
}

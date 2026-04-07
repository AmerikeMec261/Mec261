using UnityEngine;

public class explosivebullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 60f;
    [SerializeField] private float _damage = 50f;

    [Header("Explosion Settings")]
    [SerializeField] private float _explosionRadius = 5f;

    [Header("Visual Effects")]
    [SerializeField] private int _fragmentsCount = 15; 
    [SerializeField] private float _fragmentExplosionForce = 500f; 
    [SerializeField] private float _fragmentSize = 0.2f;

    public float Speed => _speed;
    public float Damage => _damage;

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }
    
    public void DealDamage(IDamageable target)
    {
        if (target != null)
        {
            target.TakeDamage(_damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"La bala explosiva chocó con: {collision.gameObject.name}");
        Explode();
        CreateVisualFragments();
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            IDamageable damageableTarget = nearbyObject.GetComponent<IDamageable>();
            
            if (damageableTarget != null)
            {
                DealDamage(damageableTarget);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    private void CreateVisualFragments()
    {
        for (int i = 0; i < _fragmentsCount; i++)
        {            
            GameObject fragment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            fragment.transform.position = transform.position + Random.insideUnitSphere * 0.5f;
            
            fragment.transform.localScale = Vector3.one * _fragmentSize;
            
            Destroy(fragment.GetComponent<Collider>());
            
            Rigidbody rb = fragment.AddComponent<Rigidbody>();
            
            rb.AddExplosionForce(_fragmentExplosionForce, transform.position, _explosionRadius);
            
            Destroy(fragment, 2f);
        }
    }
}

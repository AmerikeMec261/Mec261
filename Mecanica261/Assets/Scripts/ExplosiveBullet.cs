 using UnityEngine;

/*[RequireComponent(typeof(Rigidbody))]
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
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //rb.AddForce(-transform.forward * _speed, ForceMode.Impulse);
        rb.linearVelocity = transform.forward * Speed;
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

   
    

}*/
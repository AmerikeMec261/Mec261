using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{

    [Header("Setting")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 10f;

    public float Speed => _speed;
    public float Damage => _damage;    


    public void Fire()
    { Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
     //  rb.AddForce(transform.forward * _speed , ForceMode.Impulse);
     rb.linearVelocity= transform.forward*Speed;
    }

    public void DealDamage(float amount)
    {
        Debug.Log($"Bala normal hizo {amount} de damagge");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Golpeé a: {collision.gameObject.name}");

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.ReciveDamage(_damage);
            DealDamage(_damage);
        }
        else
        {
            Debug.Log("No tiene IDamageable");
        }
        Destroy(gameObject);


     
    }
}
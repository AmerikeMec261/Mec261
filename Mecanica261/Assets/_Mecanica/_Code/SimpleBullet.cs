using UnityEngine;
using static IDamage;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;

    private float _damage; 
    public void SetDamage(float amount)
    {
        _damage = amount;
    }

    public void Fire()
    {       
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable victim = other.GetComponent<IDamageable>();

        if (victim != null)
        {            
            victim.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SimpleBullet : MonoBehaviour,IProjectile
     {
        [Header("Setings")]
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _damage = 20f;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void Fire()
        {
            GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
        }
    

    public void DealDamage()
     {
        Debug.Log("Damage" + _damage);
     }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable enemy = collision.gameObject.GetComponent<IDamageable>();

        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
        }

        DealDamage();
        Destroy(gameObject);
    }

}




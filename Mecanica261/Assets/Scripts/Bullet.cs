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
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
        Destroy(gameObject, 3f);
    }

    public void DealDamage(float amount)
    {
        Debug.Log($"Bala normal hizo {amount} de damagge");
    }
}
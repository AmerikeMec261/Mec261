using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _damage = 10f;

    public float Speed => _speed;

    public float Damage => _damage;

    public void DoDamage()
    {
        throw new System.NotImplementedException();
    }

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;
    }
}

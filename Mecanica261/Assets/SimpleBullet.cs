using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public class SimpleBullet : MonoBehaviour, IProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 60f;

    public void Fire()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * _speed;  
    }
}

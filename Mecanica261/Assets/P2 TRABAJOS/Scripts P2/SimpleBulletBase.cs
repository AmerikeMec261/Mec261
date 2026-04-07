using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicSimpleBullet : MonoBehaviour, IBasicProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;

    public void Fire()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * _speed;
    }
}
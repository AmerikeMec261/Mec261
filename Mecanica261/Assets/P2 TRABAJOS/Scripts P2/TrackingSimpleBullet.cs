using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrackingSimpleBullet : MonoBehaviour, ITrackingProjectile
{
    [Header("Settings")]
    [SerializeField] private float _speed = 20f;

    public void Fire()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * _speed;
    }

    public float Speed()
    {
        return _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

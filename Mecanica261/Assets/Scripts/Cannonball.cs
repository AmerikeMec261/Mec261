using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private float _velocity = 20f;
    [SerializeField] private float _angle = 45f;
    [SerializeField] private Vector3 direction= Vector3.forward;
    [SerializeField] private bool _enabled = false;

    public void SetDirection(Vector3 dir)
    {
        direction=dir.normalized;
    }

    public void PrepareToShoot()
    {
        _enabled=true;
    }

    public void Shoot()
    {
        if (!_enabled) return;
        float angleInRadians=_angle*Mathf.Rad2Deg;
        Vector3 velocityVector=new Vector3(direction.x,Mathf.Sin(angleInRadians),direction.z)*_velocity;
        rb.linearVelocity=velocityVector;
        _enabled=false;
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatMovement : MonoBehaviour
{
    [SerializeField] private Transform _engine;
    [SerializeField] private Transform _rudder;
    [SerializeField] private float _moveForce = 2000f;
    [SerializeField] private float _turnForce = 1500f;
    [SerializeField] private float _maxSpeed = 10f;

    private Rigidbody _rigidbody;

    void Start()
    {
       _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        if (_engine == null) return;
        float vertical = Input.GetAxis("Vertical");
        if (_rigidbody.linearVelocity.magnitude > _maxSpeed) return;
        Vector3 force =_engine.forward * vertical * _moveForce * Time.deltaTime;
        _rigidbody.AddForceAtPosition(force, _engine.position);
    }

    private void Turn()
    {
        if (_rudder == null) return;
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 force = transform.right * horizontal * _turnForce * Time.deltaTime;
        _rigidbody.AddForceAtPosition(force, _rudder.position);
    }
}

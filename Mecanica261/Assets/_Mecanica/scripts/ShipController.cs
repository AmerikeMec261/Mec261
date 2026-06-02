using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{
    [Header("Force Points")]
    [SerializeField] private Transform[] _engineForcePoints;
    [SerializeField] private Transform[] _rudderForcePoints;

    [Header("Richelieu Battleship Values")]
    [SerializeField] private float _maximumSpeed = 16.46f;
    [SerializeField] private float _engineForce = 155000f;
    [SerializeField] private float _rudderForce = 45000f;
    [SerializeField] private float _acceleration = 0.25f;
    [SerializeField] private float _deceleration = 0.12f;
    [SerializeField] private float _rudderSmoothness = 0.35f;
    [SerializeField] private float _maximumRudderAngle = 35f;

    private Rigidbody _rigidbody;
    private float _forwardInput;
    private float _rudderInput;
    private float _currentEnginePower;
    private float _currentRudderInput;

    public float CurrentSpeed => _rigidbody.linearVelocity.magnitude;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _forwardInput = Input.GetAxis("Vertical");
        _rudderInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        UpdateEnginePower();
        UpdateRudderInput();
        ApplyEngineForce();
        ApplyRudderForce();
        ClampMaximumSpeed();
    }

    private void UpdateEnginePower()
    {
        float speedChange = _acceleration;

        if (Mathf.Abs(_forwardInput) < 0.01f)
        {
            speedChange = _deceleration;
        }

        _currentEnginePower = Mathf.Lerp(_currentEnginePower,_forwardInput,Time.fixedDeltaTime * speedChange);
    }

    private void UpdateRudderInput()
    {
        _currentRudderInput = Mathf.Lerp(_currentRudderInput,_rudderInput,Time.fixedDeltaTime * _rudderSmoothness);
    }

    private void ApplyEngineForce()
    {
        for (int i = 0; i < _engineForcePoints.Length; i++)
        {
            Vector3 force = transform.right * _currentEnginePower * _engineForce;
            _rigidbody.AddForceAtPosition(force, _engineForcePoints[i].position);
        }
    }

    private void ApplyRudderForce()
    {
        float rudderAngle = _currentRudderInput * _maximumRudderAngle;

        for (int i = 0; i < _rudderForcePoints.Length; i++)
        {
            Vector3 force = transform.forward * rudderAngle * _rudderForce;
            _rigidbody.AddForceAtPosition(force, _rudderForcePoints[i].position);
        }
    }

    private void ClampMaximumSpeed()
    {
        if (_rigidbody.linearVelocity.magnitude > _maximumSpeed)
        {
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _maximumSpeed;
        }
    }
}
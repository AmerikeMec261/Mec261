using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private Transform _enginePoint;
    [SerializeField] private Transform _rudderPoint;

    [Header("Movement Settings")]
    [Tooltip("Fuerza maxima del motor")]
    [SerializeField] private float _maxEngineForce = 5000f;

    [Tooltip("Tiempo en alcanzar la fuerza maxima")]
    [SerializeField] private float _accelerationTime = 3f;

    [Tooltip("Velocidad maxima del barco")]
    [SerializeField] private float _maxSpeed = 10f;

    [Header("Rudder Settings")]
    [Tooltip("Fuerza maxima del timon")]
    [SerializeField] private float _maxRudderForce = 2000f;

    [Tooltip("Tiempo en alcanzar la fuerza m�xima del timon")]
    [SerializeField] private float _rudderTime = 2f;

    private float _currentEngineForce;
    private float _currentRudderForce;

    private float _verticalInput;
    private float _horizontalInput;

private void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        ApplyRudderForce();
    }

    private void ReadInput()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
    }

    private void ApplyEngineForce()
    {
        if (_enginePoint == null) { return; }

        float targetForce = _verticalInput * _maxEngineForce;

        _currentEngineForce = Mathf.MoveTowards(_currentEngineForce,targetForce,(_maxEngineForce / _accelerationTime) * Time.fixedDeltaTime);

        if (_rigidbody.linearVelocity.magnitude > _maxSpeed) { return; }

        Vector3 force = _enginePoint.forward * _currentEngineForce;

        _rigidbody.AddForceAtPosition(force, _enginePoint.position, ForceMode.Force);
    }

    private void ApplyRudderForce()
    {
        if (_rudderPoint == null) { return; }

        float targetForce = _horizontalInput * _maxRudderForce;

        _currentRudderForce = Mathf.MoveTowards(_currentRudderForce,targetForce,(_maxRudderForce / _rudderTime) * Time.fixedDeltaTime);

        Vector3 force = transform.right * _currentRudderForce;

        _rigidbody.AddForceAtPosition(force, _rudderPoint.position, ForceMode.Force);
    }

    
}
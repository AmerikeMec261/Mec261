using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Engine Settings")]
    [Tooltip("Fuerza maxima del motor")]
    [SerializeField] private float _maxEngineForce = 15000f;

    [Tooltip("Tiempo en alcanzar la fuerza maxima")]
    [SerializeField] private float _engineAccelerationTime = 5f;

    [Tooltip("Max Velocity")]
    [SerializeField] private float _maxSpeed = 10f;

    [Tooltip("Punto donde se aplica la fuerza del motor")]
    [SerializeField] private Transform _enginePoint;

    [Header("Rudder Settings")]
    [Tooltip("Fuerza maxima del rudder")]
    [SerializeField] private float _maxRudderForce = 8000f;

    [Tooltip("Tiempo en alcanzar la fuerza maxima del giro")]
    [SerializeField] private float _rudderAccelerationTime = 2f;

    [Tooltip("Punto donde se aplica la fuerza del rudder")]
    [SerializeField] private Transform _rudderPoint;

    private float _currentEngineForce;
    private float _currentRudderForce;

    private float _inputForward;
    private float _inputTurn;

    #endregion Variables


    #region Unity Methods

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

    #endregion Unity Methods


    #region Methods

    private void ReadInput()
    {
        _inputForward = Input.GetAxis("Vertical");   
        _inputTurn = Input.GetAxis("Horizontal");   
    }


    private void ApplyEngineForce()
    {
        if (_enginePoint == null) { return; }

        float targetForce = _inputForward * _maxEngineForce;

        _currentEngineForce = Mathf.MoveTowards(
            _currentEngineForce,
            targetForce,
            (_maxEngineForce / _engineAccelerationTime) * Time.fixedDeltaTime
        );

        if (_rigidbody.linearVelocity.magnitude > _maxSpeed) { return; }

        Vector3 force = transform.forward * _currentEngineForce;

        _rigidbody.AddForceAtPosition(force, _enginePoint.position, ForceMode.Force);
    }


    private void ApplyRudderForce()
    {
        if (_rudderPoint == null) { return; }

        float targetForce = _inputTurn * _maxRudderForce;

        _currentRudderForce = Mathf.MoveTowards(
            _currentRudderForce,
            targetForce,
            (_maxRudderForce / _rudderAccelerationTime) * Time.fixedDeltaTime
        );

        Vector3 force = transform.right * _currentRudderForce;

        _rigidbody.AddForceAtPosition(force, _rudderPoint.position, ForceMode.Force);
    }

    #endregion Methods
}
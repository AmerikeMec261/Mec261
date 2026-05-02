using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipControl : MonoBehaviour
{
    [Header("Engine")]
    [SerializeField] private float _maximumForwardEngineForce = 5000f;
    [SerializeField] private float _maximumReverseEngineForce = 600f;
    [SerializeField] private float _forwardEngineResponseTime = 1f;
    [SerializeField] private float _reverseEngineResponseTime = 1f;
    [SerializeField] private List<Transform> _propellers = new List<Transform>();

    [Header("Rudder")]
    [SerializeField] private Transform _rudder;
    [SerializeField] private float _maximumRudderAngle = 35f;
    [SerializeField] private float _rudderTurningTime = 15f;
    [SerializeField] private float _rudderLateralForce = 1500f;
    [SerializeField] private float _rudderYawTorque = 100f;
    [SerializeField] private float _rudderReferenceSpeed = 50f;
    [SerializeField] private float _minimumRudderSpeedFactor = 0.15f;

    [Header("Hull Resistance")]
    [SerializeField] private float _lateralResistanceForce = 2500f;
    [SerializeField] private float _lateralResistanceReferenceSpeed = 5f;

    [Header("Debug")]
    [SerializeField] private bool _debugForces = true;
    [SerializeField] private float _debugForceScale = 0.01f;

    private readonly float[] _speedSteps = { -1f, -0.75f, -0.5f, -0.25f, 0f, 0.25f, 0.5f, 0.75f, 1f };

    private Rigidbody _rigidbody;

    private int _currentSpeedStepIndex = 4;
    private int _desiredSpeedStepIndex = 4;

    private float _currentSpeedStep;
    private float _desiredSpeedStep;

    private float _currentRudderAngle;
    private float _targetRudderAngle;
    private bool _rudderLocked;

    private Vector3 ForwardDirection => transform.right;
    private Vector3 SideDirection => -transform.forward;

    public float CurrentForwardSpeed => Vector3.Dot(_rigidbody.linearVelocity, ForwardDirection);
    public float CurrentLateralSpeed => Vector3.Dot(_rigidbody.linearVelocity, SideDirection);

    public float CurrentRudderAngle => _currentRudderAngle;
    public float DesiredRudderAngle => _targetRudderAngle;

    public float CurrentSpeedStep => _currentSpeedStep;
    public float DesiredSpeedStep => _desiredSpeedStep;

    public int CurrentSpeedStepDiscrete => _currentSpeedStepIndex - 4;
    public int DesiredSpeedStepDiscrete => _desiredSpeedStepIndex - 4;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentSpeedStep = _speedSteps[_currentSpeedStepIndex];
        _desiredSpeedStep = _speedSteps[_desiredSpeedStepIndex];
    }

    private void Update()
    {
        HandleSpeedInput();
        HandleRudderInput();
        UpdateEngineResponse();
        UpdateRudder();
    }

    private void FixedUpdate()
    {
        ApplyEngineForces();
        ApplyRudderForce();
        ApplyLateralResistance();
    }

    private void HandleSpeedInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) { ChangeSpeedStep(1); }
        if (Input.GetKeyDown(KeyCode.S)) { ChangeSpeedStep(-1); }
    }

    private void ChangeSpeedStep(int direction)
    {
        _desiredSpeedStepIndex = Mathf.Clamp(_desiredSpeedStepIndex + direction, 0, _speedSteps.Length - 1);
        _desiredSpeedStep = _speedSteps[_desiredSpeedStepIndex];
    }

    private void UpdateEngineResponse()
    {
        float responseTime = _desiredSpeedStep >= 0f ? _forwardEngineResponseTime : _reverseEngineResponseTime;
        float stepChangePerSecond = 1f / responseTime;

        _currentSpeedStep = Mathf.MoveTowards(_currentSpeedStep, _desiredSpeedStep, stepChangePerSecond * Time.deltaTime);
        _currentSpeedStepIndex = GetClosestSpeedStepIndex(_currentSpeedStep);
    }

    private int GetClosestSpeedStepIndex(float speedStep)
    {
        int closestIndex = 0;
        float closestDistance = Mathf.Abs(_speedSteps[0] - speedStep);

        for (int i = 1; i < _speedSteps.Length; i++)
        {
            float distance = Mathf.Abs(_speedSteps[i] - speedStep);
            if (distance < closestDistance) { closestDistance = distance; closestIndex = i; }
        }

        return closestIndex;
    }

    private void HandleRudderInput()
    {
        bool isTurningLeft = Input.GetKey(KeyCode.A);
        bool isTurningRight = Input.GetKey(KeyCode.D);

        if (isTurningLeft) { _rudderLocked = false; _targetRudderAngle = _maximumRudderAngle; }
        else if (isTurningRight) { _rudderLocked = false; _targetRudderAngle = -_maximumRudderAngle; }
        else if (!_rudderLocked) { _targetRudderAngle = 0f; }

        if (Input.GetKeyDown(KeyCode.Q)) { _rudderLocked = true; _targetRudderAngle = _maximumRudderAngle; }
        if (Input.GetKeyDown(KeyCode.E)) { _rudderLocked = true; _targetRudderAngle = -_maximumRudderAngle; }
    }

    private void UpdateRudder()
    {
        float rudderChangePerSecond = _maximumRudderAngle / _rudderTurningTime;
        _currentRudderAngle = Mathf.MoveTowards(_currentRudderAngle, _targetRudderAngle, rudderChangePerSecond * Time.deltaTime);
    }

    private void ApplyEngineForces()
    {
        float maximumEngineForce = _currentSpeedStep >= 0f ? _maximumForwardEngineForce : _maximumReverseEngineForce;
        float totalForce = maximumEngineForce * _currentSpeedStep;
        float forcePerPropeller = totalForce / _propellers.Count;

        for (int i = 0; i < _propellers.Count; i++)
        {
            Vector3 force = ForwardDirection * forcePerPropeller;

            _rigidbody.AddForceAtPosition(force, _propellers[i].position, ForceMode.Force);

            if (_debugForces) { Debug.DrawRay(_propellers[i].position, force * _debugForceScale, Color.green, 1f); }
        }
    }

    private void ApplyRudderForce()
    {
        float forwardSpeed = Vector3.Dot(_rigidbody.linearVelocity, ForwardDirection);
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / _rudderReferenceSpeed);
        float speedFactor = Mathf.Max(_minimumRudderSpeedFactor, normalizedSpeed * normalizedSpeed);

        float rudderFactor = _currentRudderAngle / _maximumRudderAngle;
        float directionMultiplier = forwardSpeed >= 0f ? 1f : -1f;

        Vector3 sideForce = SideDirection * rudderFactor * _rudderLateralForce * speedFactor * directionMultiplier;
        float yawTorque = rudderFactor * _rudderYawTorque * speedFactor * directionMultiplier;

        _rigidbody.AddForceAtPosition(sideForce, _rudder.position, ForceMode.Force);
        _rigidbody.AddTorque(Vector3.up * yawTorque, ForceMode.Force);

        if (_debugForces) { Debug.DrawRay(_rudder.position, sideForce * _debugForceScale, Color.red, 1f); }
    }

    private void ApplyLateralResistance()
    {
        float lateralSpeed = Vector3.Dot(_rigidbody.linearVelocity, SideDirection);
        float resistanceFactor = Mathf.Clamp01(Mathf.Abs(lateralSpeed) / _lateralResistanceReferenceSpeed);
        Vector3 resistanceForce = -SideDirection * Mathf.Sign(lateralSpeed) * _lateralResistanceForce * resistanceFactor;

        _rigidbody.AddForce(resistanceForce, ForceMode.Force);

        if (_debugForces) { Debug.DrawRay(transform.position, resistanceForce * _debugForceScale, Color.cyan, 1f); }
    }
}
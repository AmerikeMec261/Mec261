using UnityEngine;
using NaughtyAttributes;

public class LegAnimator : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("Leg pivots to rotate while this character moves. Even and odd indexes move in opposite directions.")]
    [SerializeField] private Transform[] _legPivots;

    [Header("Settings")]
    [Tooltip("Local axis each leg rotates around.")]
    [SerializeField] private Vector3 _rotationAxis = Vector3.right;

    [Header("Walk")]
    [Tooltip("Maximum angle each leg swings away from its starting rotation while walking.")]
    [SerializeField] private float _walkStepAngle = 25f;
    [Tooltip("How fast the walking cycle plays.")]
    [SerializeField] private float _walkStepSpeed = 8f;

    [Header("Run")]
    [Tooltip("Horizontal speed where the animator switches from walking to running.")]
    [SerializeField] private float _runSpeedThreshold = 6f;
    [Tooltip("Maximum angle each leg swings away from its starting rotation while running.")]
    [SerializeField] private float _runStepAngle = 40f;
    [Tooltip("How fast the running cycle plays.")]
    [SerializeField] private float _runStepSpeed = 12f;

    [Header("Movement Detection")]
    [Tooltip("How fast legs return to their starting rotation when movement stops.")]
    [SerializeField] private float _idleReturnSpeed = 12f;
    [Tooltip("Minimum horizontal movement speed required to animate the legs.")]
    [SerializeField] private float _minimumMoveSpeed = 0.05f;
    [Tooltip("How quickly measured movement speed changes are smoothed.")]
    [SerializeField] private float _speedSmoothTime = 12f;

    private Quaternion[] _startRotations;
    private Vector3 _lastPosition;
    private float _stepTime;
    private float _horizontalSpeed;

    private void Awake()
    {
        _startRotations = new Quaternion[_legPivots.Length];

        for (int i = 0; i < _legPivots.Length; i++)
        {
            if (_legPivots[i] == null) { continue; }

            _startRotations[i] = _legPivots[i].localRotation;
        }

        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (_legPivots.Length == 0) { return; }

        float horizontalSpeed = GetSmoothedHorizontalSpeed();

        if (horizontalSpeed < _minimumMoveSpeed)
        {
            ReturnToIdle();
            return;
        }

        MoveLegs(horizontalSpeed);
    }

    private float GetSmoothedHorizontalSpeed()
    {
        Vector3 movement = transform.position - _lastPosition;
        movement.y = 0f;
        _lastPosition = transform.position;

        float targetSpeed = Time.deltaTime <= 0f ? 0f : movement.magnitude / Time.deltaTime;
        _horizontalSpeed = Mathf.Lerp(_horizontalSpeed, targetSpeed, _speedSmoothTime * Time.deltaTime);

        return _horizontalSpeed;
    }

    private void MoveLegs(float horizontalSpeed)
    {
        bool isRunning = horizontalSpeed >= _runSpeedThreshold;
        float stepAngle = isRunning ? _runStepAngle : _walkStepAngle;
        float stepSpeed = isRunning ? _runStepSpeed : _walkStepSpeed;

        _stepTime += Time.deltaTime * stepSpeed;

        float angle = Mathf.Sin(_stepTime) * stepAngle;

        for (int i = 0; i < _legPivots.Length; i++)
        {
            if (_legPivots[i] == null) { continue; }

            float direction = i % 2 == 0 ? 1f : -1f;
            Quaternion stepRotation = Quaternion.AngleAxis(angle * direction, _rotationAxis);

            _legPivots[i].localRotation = _startRotations[i] * stepRotation;
        }
    }

    private void ReturnToIdle()
    {
        for (int i = 0; i < _legPivots.Length; i++)
        {
            if (_legPivots[i] == null) { continue; }

            _legPivots[i].localRotation = Quaternion.Lerp(_legPivots[i].localRotation, _startRotations[i], _idleReturnSpeed * Time.deltaTime);
        }
    }
}

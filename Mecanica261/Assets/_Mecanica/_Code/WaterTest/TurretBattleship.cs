using System.Collections.Generic;
using UnityEngine;

public class TurretBattleship : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _baseReference;
    [SerializeField] private List<Transform> _cannonPivots = new List<Transform>();

    [Header("Yaw Settings")]
    [SerializeField] private float _rotationDegreesPerSecond = 180f;
    [SerializeField] private float _rotationLimit = 145f;
    [SerializeField] private float _gizmoLength = 2f;

    [Header("Pitch Settings")]
    [SerializeField] private float _projectileSpeed = 250f;
    [SerializeField] private float _pitchDegreesPerSecond = 20f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);

    [Header("Target")]
    [SerializeField] private Transform _target;

    private float _startLocalZ;

    public void SetTarget(Transform target) { _target = target; }
    public void ClearTarget() { _target = null; }

    private void Awake()
    {
        _startLocalZ = NormalizeAngle(transform.localEulerAngles.z);
    }

    private void FixedUpdate()
    {
        RotateTurret();
        RotateCannons();
    }

    private void RotateTurret()
    {
        float currentRelativeRotationZ = Mathf.DeltaAngle(_startLocalZ, transform.localEulerAngles.z);
        float targetRelativeRotationZ = CalculateTargetRelativeRotationZ();

        float rotationStep = _rotationDegreesPerSecond * Time.fixedDeltaTime;
        float newRelativeRotationZ = Mathf.MoveTowards(currentRelativeRotationZ, targetRelativeRotationZ, rotationStep);

        transform.localRotation = Quaternion.Euler(0f, 0f, _startLocalZ + newRelativeRotationZ);
    }

    private void RotateCannons()
    {
        float targetPitch = CalculateTargetPitchY();
        float rotationStep = _pitchDegreesPerSecond * Time.fixedDeltaTime;

        for (int i = 0; i < _cannonPivots.Count; i++)
        {
            float currentPitch = NormalizeAngle(_cannonPivots[i].localEulerAngles.y);
            float newPitch = Mathf.MoveTowardsAngle(currentPitch, targetPitch, rotationStep);

            _cannonPivots[i].localRotation = Quaternion.Euler(_cannonPivots[i].localEulerAngles.x, newPitch, _cannonPivots[i].localEulerAngles.z);
        }
    }

    private float CalculateTargetPitchY()
    {
        if (_target == null) { return 0f; }

        if (TryCalculateLowArcAngle(_target.position, out float lowAngle))
        {
            return Mathf.Clamp(lowAngle, _pitchLimits.x, _pitchLimits.y);
        }

        return _pitchLimits.y;
    }

    private bool TryCalculateLowArcAngle(Vector3 targetPosition, out float lowAngle)
    {
        Vector3 startPosition = GetAverageCannonPosition();
        Vector3 directionToTarget = targetPosition - startPosition;

        float horizontalDistance = new Vector2(directionToTarget.x, directionToTarget.z).magnitude;
        float verticalDistance = directionToTarget.y;
        float gravity = Mathf.Abs(Physics.gravity.y);

        if (horizontalDistance < 0.01f)
        {
            lowAngle = verticalDistance > 0f ? _pitchLimits.y : _pitchLimits.x;
            return true;
        }

        float speedSquared = _projectileSpeed * _projectileSpeed;
        float speedToFourth = speedSquared * speedSquared;
        float discriminant = speedToFourth - gravity * (gravity * horizontalDistance * horizontalDistance + 2f * verticalDistance * speedSquared);

        if (discriminant < 0f)
        {
            lowAngle = 0f;
            return false;
        }

        float sqrt = Mathf.Sqrt(discriminant);
        float denominator = gravity * horizontalDistance;

        lowAngle = Mathf.Atan((speedSquared - sqrt) / denominator) * Mathf.Rad2Deg;
        return true;
    }

    private Vector3 GetAverageCannonPosition()
    {
        Vector3 averagePosition = Vector3.zero;

        for (int i = 0; i < _cannonPivots.Count; i++)
        {
            averagePosition += _cannonPivots[i].position;
        }

        return averagePosition / _cannonPivots.Count;
    }

    private float CalculateTargetRelativeRotationZ()
    {
        if (_target == null) { return 0f; }

        Vector3 directionFromTurretToTarget = _target.position - transform.position;
        directionFromTurretToTarget.y = 0f;

        if (directionFromTurretToTarget.sqrMagnitude <= 0.0001f) { return 0f; }

        directionFromTurretToTarget.Normalize();

        float targetWorldBearing = Mathf.Atan2(directionFromTurretToTarget.z, directionFromTurretToTarget.x) * Mathf.Rad2Deg;
        float baseWorldBearing = GetBaseWorldBearing();

        float desiredLocalRotationZ = -Mathf.DeltaAngle(baseWorldBearing, targetWorldBearing);
        float desiredRelativeRotationZ = Mathf.DeltaAngle(_startLocalZ, desiredLocalRotationZ);

        return Mathf.Clamp(desiredRelativeRotationZ, -_rotationLimit, _rotationLimit);
    }

    private float GetBaseWorldBearing()
    {
        return Mathf.Atan2(_baseReference.right.z, _baseReference.right.x) * Mathf.Rad2Deg;
    }

    private float NormalizeAngle(float angle)
    {
        return Mathf.DeltaAngle(0f, angle);
    }

    private Vector3 GetWorldPointFromLocalRotationZ(float localRotationZ)
    {
        Quaternion baseRotation = transform.parent != null ? transform.parent.rotation : Quaternion.identity;
        Vector3 direction = baseRotation * Quaternion.Euler(0f, 0f, localRotationZ) * -Vector3.right;

        return transform.position + direction * _gizmoLength;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        float startLocalRotationZ = Application.isPlaying ? _startLocalZ : NormalizeAngle(transform.localEulerAngles.z);
        float minimumLocalRotationZ = startLocalRotationZ - _rotationLimit;
        float maximumLocalRotationZ = startLocalRotationZ + _rotationLimit;

        Vector3 previousPoint = GetWorldPointFromLocalRotationZ(minimumLocalRotationZ);

        for (int i = 1; i <= 32; i++)
        {
            float interpolation = i / 32f;
            float interpolatedRotationZ = Mathf.Lerp(minimumLocalRotationZ, maximumLocalRotationZ, interpolation);

            Vector3 nextPoint = GetWorldPointFromLocalRotationZ(interpolatedRotationZ);

            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        Gizmos.DrawLine(transform.position, GetWorldPointFromLocalRotationZ(minimumLocalRotationZ));
        Gizmos.DrawLine(transform.position, GetWorldPointFromLocalRotationZ(maximumLocalRotationZ));
    }
}
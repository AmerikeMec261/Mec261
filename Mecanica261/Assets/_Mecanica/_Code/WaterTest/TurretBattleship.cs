using UnityEngine;

public class TurretBattleship : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _baseReference;

    [Header("Settings")]
    [SerializeField] private float _rotationDegreesPerSecond = 180f;
    [SerializeField] private float _rotationLimit = 145f;
    [SerializeField] private float _gizmoLength = 2f;

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
    }

    private void RotateTurret()
    {
        float currentRelativeRotationZ = Mathf.DeltaAngle(_startLocalZ, transform.localEulerAngles.z);
        float targetRelativeRotationZ = CalculateTargetRelativeRotationZ();

        float rotationStep = _rotationDegreesPerSecond * Time.fixedDeltaTime;
        float newRelativeRotationZ = Mathf.MoveTowards(currentRelativeRotationZ, targetRelativeRotationZ, rotationStep);

        transform.localRotation = Quaternion.Euler(0f, 0f, _startLocalZ + newRelativeRotationZ);
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
        Gizmos.color = Color.yellow;

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
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private RectTransform _aimMarker;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);
    [SerializeField] private ArcPreference _arcPreference = ArcPreference.Auto;

    [Header("Aim Marker Settings")]
    [SerializeField] private LayerMask _aimCollisionLayers;
    [SerializeField] private float _aimMarkerOffset = 0.1f;
    [SerializeField] private float _trajectoryStep = 0.1f;
    [SerializeField] private float _maxTrajectoryTime = 10f;

    [Header("Debug")]
    [SerializeField] private bool _showTrajectoryDebug = true;

    private void Update()
    {
        float yawInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float pitchInput = Input.GetKey(KeyCode.W) ? -1f : Input.GetKey(KeyCode.S) ? 1f : 0f;

        RotateYaw(yawInput);
        RotatePitch(pitchInput);
        UpdateAimMarker();

        if (Input.GetKeyDown(KeyCode.Space)) { FireProjectile(); }
    }

    public float GetMaxHighArcRange()
    {
        float projectileSpeed = _bulletPrefab.GetComponent<IProjectile>().Speed;
        float gravity = Mathf.Abs(Physics.gravity.y);

        float highArcAngle = Mathf.Clamp(45f, _pitchLimits.x, _pitchLimits.y) * Mathf.Deg2Rad;
        float launchHeight = _bulletSpawn.position.y - transform.position.y;

        float verticalSpeed = projectileSpeed * Mathf.Sin(highArcAngle);
        float horizontalSpeed = projectileSpeed * Mathf.Cos(highArcAngle);

        float flightTime = (verticalSpeed + Mathf.Sqrt(verticalSpeed * verticalSpeed + 2f * gravity * launchHeight)) / gravity;
        float maxRange = horizontalSpeed * flightTime;

        return maxRange;
    }

    public void AimAtTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - _yawPivot.position;
        float targetYaw = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        YawTurret(targetYaw);

        if (TryCalculatePreferredArcAngle(targetPosition, out float targetPitch))
        {
            PitchTurret(targetPitch);
        }
        else
        {
            PitchTurret(GetMaxRangePitch());
        }
    }

    private float GetMaxRangePitch()
    {
        return Mathf.Clamp(45f, _pitchLimits.x, _pitchLimits.y);
    }

    private void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();
    }

    private void RotateYaw(float input)
    {
        float currentYaw = NormalizeAngle(_yawPivot.localEulerAngles.y);
        float newYaw = Mathf.Clamp(currentYaw + input * _yawSpeed * Time.deltaTime, _yawLimits.x, _yawLimits.y);
        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles.z);
    }

    private void RotatePitch(float input)
    {
        float currentPitch = NormalizeAngle(_pitchPivot.localEulerAngles.x);
        float newPitch = Mathf.Clamp(currentPitch - input * _pitchSpeed * Time.deltaTime, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(newPitch, _pitchPivot.localEulerAngles.y, _pitchPivot.localEulerAngles.z);
    }

    private void YawTurret(float angleToRotateTo)
    {
        float currentYaw = NormalizeAngle(_yawPivot.localEulerAngles.y);
        float targetYaw = Mathf.Clamp(angleToRotateTo, _yawLimits.x, _yawLimits.y);
        float newYaw = Mathf.MoveTowardsAngle(currentYaw, targetYaw, _yawSpeed * Time.deltaTime);
        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles.z);
    }

    private void PitchTurret(float angleToRotateTo)
    {
        float currentPitch = NormalizeAngle(_pitchPivot.localEulerAngles.x);
        float targetPitch = Mathf.Clamp(angleToRotateTo, _pitchLimits.x, _pitchLimits.y);
        float newPitch = Mathf.MoveTowardsAngle(currentPitch, targetPitch, _pitchSpeed * Time.deltaTime);
        _pitchPivot.localEulerAngles = new Vector3(newPitch, _pitchPivot.localEulerAngles.y, _pitchPivot.localEulerAngles.z);
    }

    private bool TryCalculatePreferredArcAngle(Vector3 targetPosition, out float angle)
    {
        if (!TryCalculateArcAngles(targetPosition, out float lowAngle, out float highAngle))
        {
            angle = 0f;
            return false;
        }

        bool lowValid = IsAngleWithinPitchLimits(lowAngle);
        bool highValid = IsAngleWithinPitchLimits(highAngle);

        if (_arcPreference == ArcPreference.High)
        {
            if (highValid) { angle = highAngle; return true; }
            if (lowValid) { angle = lowAngle; return true; }

            angle = 0f;
            return false;
        }

        if (_arcPreference == ArcPreference.Low)
        {
            if (lowValid) { angle = lowAngle; return true; }
            if (highValid) { angle = highAngle; return true; }

            angle = 0f;
            return false;
        }

        bool lowClear = lowValid && !IsArcBlocked(targetPosition, lowAngle);
        bool highClear = highValid && !IsArcBlocked(targetPosition, highAngle);

        if (lowClear) { angle = lowAngle; return true; }
        if (highClear) { angle = highAngle; return true; }
        if (highValid) { angle = highAngle; return true; }
        if (lowValid) { angle = lowAngle; return true; }

        angle = 0f;
        return false;
    }

    private bool TryCalculateArcAngles(Vector3 targetPosition, out float lowAngle, out float highAngle)
    {
        Vector3 directionToTarget = targetPosition - _bulletSpawn.position;
        float horizontalDistance = new Vector2(directionToTarget.x, directionToTarget.z).magnitude;
        float verticalDistance = directionToTarget.y;
        float gravity = Mathf.Abs(Physics.gravity.y);
        float projectileSpeed = _bulletPrefab.GetComponent<IProjectile>().Speed;

        if (horizontalDistance < 0.01f)
        {
            float verticalAngle = verticalDistance > 0f ? 90f : 0f;
            lowAngle = verticalAngle;
            highAngle = verticalAngle;
            return true;
        }

        float speedSquared = projectileSpeed * projectileSpeed;
        float speedToFourth = speedSquared * speedSquared;
        float discriminant = speedToFourth - gravity * (gravity * horizontalDistance * horizontalDistance + 2f * verticalDistance * speedSquared);

        if (discriminant < 0f)
        {
            lowAngle = 0f;
            highAngle = 0f;
            return false;
        }

        float sqrt = Mathf.Sqrt(discriminant);
        float denominator = gravity * horizontalDistance;

        lowAngle = Mathf.Atan((speedSquared - sqrt) / denominator) * Mathf.Rad2Deg;
        highAngle = Mathf.Atan((speedSquared + sqrt) / denominator) * Mathf.Rad2Deg;
        return true;
    }

    private bool IsArcBlocked(Vector3 targetPosition, float pitchAngle)
    {
        Vector3 startPosition = _bulletSpawn.position;
        float projectileSpeed = _bulletPrefab.GetComponent<IProjectile>().Speed;
        Vector3 gravity = Physics.gravity;

        Vector3 flatDirection = targetPosition - startPosition;
        flatDirection.y = 0f;

        if (flatDirection.sqrMagnitude < 0.001f) { return false; }

        Vector3 horizontalDirection = flatDirection.normalized;
        Quaternion pitchRotation = Quaternion.AngleAxis(-pitchAngle, _bulletSpawn.right);
        Vector3 startVelocity = pitchRotation * (horizontalDirection * projectileSpeed);

        Vector3 previousPoint = startPosition;
        float targetDistance = Vector3.Distance(startPosition, targetPosition);

        for (float time = _trajectoryStep; time <= _maxTrajectoryTime; time += _trajectoryStep)
        {
            Vector3 currentPoint = startPosition + startVelocity * time + 0.5f * gravity * time * time;

            if (Vector3.Distance(startPosition, currentPoint) > targetDistance) { break; }

            if (Physics.Linecast(previousPoint, currentPoint, out RaycastHit hitInfo, _aimCollisionLayers))
            {
                float hitDistanceToTarget = Vector3.Distance(hitInfo.point, targetPosition);
                if (hitDistanceToTarget > 0.5f) { return true; }
            }

            previousPoint = currentPoint;
        }

        return false;
    }

    private bool IsAngleWithinPitchLimits(float angle)
    {
        return angle >= _pitchLimits.x && angle <= _pitchLimits.y;
    }

    private void UpdateAimMarker()
    {
        if (_aimMarker == null) { return; }

        if (TryGetImpactPoint(out RaycastHit hitInfo))
        {
            _aimMarker.position = hitInfo.point + hitInfo.normal * _aimMarkerOffset;
            _aimMarker.rotation = Quaternion.LookRotation(-hitInfo.normal, _yawPivot.up);
        }
    }

    private bool TryGetImpactPoint(out RaycastHit hitInfo)
    {
        Vector3 startPosition = _bulletSpawn.position;
        Vector3 startVelocity = _bulletSpawn.forward * _bulletPrefab.GetComponent<IProjectile>().Speed;
        Vector3 gravity = Physics.gravity;
        Vector3 previousPoint = startPosition;

        if (_showTrajectoryDebug) { Debug.DrawRay(startPosition, startVelocity.normalized * 2f, Color.blue); }

        for (float time = _trajectoryStep; time <= _maxTrajectoryTime; time += _trajectoryStep)
        {
            Vector3 currentPoint = startPosition + startVelocity * time + 0.5f * gravity * time * time;

            if (_showTrajectoryDebug) { Debug.DrawLine(previousPoint, currentPoint, Color.yellow); }

            if (Physics.Linecast(previousPoint, currentPoint, out hitInfo, _aimCollisionLayers))
            {
                if (_showTrajectoryDebug)
                {
                    Debug.DrawLine(previousPoint, hitInfo.point, Color.red);
                    Debug.DrawRay(hitInfo.point, hitInfo.normal * 1.5f, Color.green);
                }

                return true;
            }

            previousPoint = currentPoint;
        }

        if (_showTrajectoryDebug) { Debug.DrawRay(previousPoint, Vector3.up * 2f, Color.magenta); }

        hitInfo = default;
        return false;
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f) { angle -= 360f; }
        return angle;
    }

    private enum ArcPreference
    {
        High,
        Low,
        Auto
    }
}
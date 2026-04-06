using UnityEngine;

public class torreta : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Mouse Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _reticle;


    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 120f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-180f, 180f);    
    [SerializeField] private float _yawOffset = 0f;

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);    
    [SerializeField] private float _pitchOffset = 0f;    
    [SerializeField] private bool _useHighAngle = false;

    [Header("Ballistics Settings")]    
    [SerializeField] private float _projectileVelocity = 20f;

    private Vector3 _tragetPosition;
    private bool _isTargetInRange;


    private void Update()
    {
        HandleMouseTracking();
        UpdateAutoYaw();
        UpdateAutoPitch();
        UpdateReticle();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }
    }

    public void FireProjectile()
    {
        if (_bulletPrefab == null || _bulletSpawn == null)
        {
            Debug.LogWarning("Turret: Falta asignar el Bullet Prefab o el Bullet Spawn.");
            return;
        }

        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();
    }

    public bool CalculateFiringAngles(float distance, out float lowAngle, out float highAngle)
    {
        lowAngle = 0f;
        highAngle = 0f;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float velocitySquared = _projectileVelocity * _projectileVelocity;

        float valueToArcSin = (distance * gravity) / velocitySquared;

        if (valueToArcSin > 1f)
        {
            return false;
        }

        float arcSinValueInRadians = Mathf.Asin(valueToArcSin);
        float arcSinValueInDegrees = arcSinValueInRadians * Mathf.Rad2Deg;

        lowAngle = 0.5f * arcSinValueInDegrees;
        highAngle = 0.5f * (180f - arcSinValueInDegrees);

        return true;
    }

    private void HandleMouseTracking()
    {
        if (Camera.main == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundLayer))
        {
            Vector3 hitPoint = hit.point;

            float gravity = Mathf.Abs(Physics.gravity.y);
            float maxRange = (_projectileVelocity * _projectileVelocity) / gravity;

            Vector2 turretPosXZ = new Vector2(_yawPivot.position.x, _yawPivot.position.z);
            Vector2 hitPosXZ = new Vector2(hitPoint.x, hitPoint.z);
            float distanceToMouse = Vector2.Distance(turretPosXZ, hitPosXZ);

            if (distanceToMouse > maxRange)
            {
                Vector2 dirXZ = (hitPosXZ - turretPosXZ).normalized;
                Vector2 clampedXZ = turretPosXZ + (dirXZ * maxRange);

                _tragetPosition = new Vector3(clampedXZ.x, hitPoint.y, clampedXZ.y);
                _isTargetInRange = false;
            }
            else
            {
                _tragetPosition = hitPoint;
                _isTargetInRange = true;
            }
        }
    }

    private void UpdateAutoYaw()
    {
        Vector3 directionToTarget = _tragetPosition - _yawPivot.position;
        directionToTarget.y = 0f;

        if (directionToTarget.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion desiredWorldRotation = Quaternion.LookRotation(directionToTarget);
        Quaternion desiredLocalRotation = _yawPivot.parent != null
            ? Quaternion.Inverse(_yawPivot.parent.rotation) * desiredWorldRotation
            : desiredWorldRotation;

        float targetYawAngle = desiredLocalRotation.eulerAngles.y + _yawOffset;

        if (targetYawAngle > 180f)
        {
            targetYawAngle -= 360f;
        }

        float clampedYaw = Mathf.Clamp(targetYawAngle, _yawLimits.x, _yawLimits.y);
        float currentYaw = _yawPivot.localEulerAngles.y;
        float newYaw = Mathf.MoveTowardsAngle(currentYaw, clampedYaw, _yawSpeed * Time.deltaTime);

        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles.z);
    }

    private void UpdateAutoPitch()
    {
        Vector2 turretPosXZ = new Vector2(_yawPivot.position.x, _yawPivot.position.z);
        Vector2 targetPosXZ = new Vector2(_tragetPosition.x, _tragetPosition.z);
        float distance = Vector2.Distance(turretPosXZ, targetPosXZ);

        _isTargetInRange = CalculateFiringAngles(distance, out float lowAngle, out float highAngle);

        if (!_isTargetInRange)
        {
            return;
        }

        float targetPitchAngle = _useHighAngle ? highAngle : lowAngle;

        targetPitchAngle += _pitchOffset;

        float clampedPitch = Mathf.Clamp(targetPitchAngle, _pitchLimits.x, _pitchLimits.y);
        float currentPitch = _pitchPivot.localEulerAngles.z;
        float newPitch = Mathf.MoveTowardsAngle(currentPitch, clampedPitch, _pitchSpeed * Time.deltaTime);

        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, newPitch);
    }

    private void UpdateReticle()
    {
        if (_reticle != null)
        {
            _reticle.position = _tragetPosition;
        }
    }


    private void OnDrawGizmos()
    {        
            Gizmos.color = _isTargetInRange ? Color.green : Color.red;
            Gizmos.DrawLine(_bulletSpawn != null ? _bulletSpawn.position : _yawPivot.position, _tragetPosition);
    }
    
}

using UnityEngine;

public class Turret : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Targeting Settings")]
    [SerializeField] private string _enemyTag = "Enemy";
    [SerializeField] private float _searchInterval = 0.5f;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 120f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-180f, 180f);
    [Tooltip("Ajusta este valor (ej. 90, -90, 180) si la torreta mira hacia un lado en lugar de al frente.")]
    [SerializeField] private float _yawOffset = 0f;

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);
    [Tooltip("Ajusta este valor si el cañón queda muy arriba o muy abajo visualmente.")]
    [SerializeField] private float _pitchOffset = 0f;
    [Tooltip("If true, uses the higher parabolic arc. If false, uses the direct, lower arc.")]
    [SerializeField] private bool _useHighAngle = false;

    [Header("Ballistics Settings")]
    [Tooltip("Initial velocity (V0) of the projectile in units/second.")]
    [SerializeField] private float _projectileVelocity = 20f;

    private Transform _target;
    private float _searchTimer;
    private bool _isTargetInRange;

    #endregion Variables

    #region Unity Methods

    private void Update()
    {
        HandleTargetSearch();

        // Regla: Guard Clause (Retorno temprano si no hay objetivo)
        if (_target == null)
        {
            return;
        }

        UpdateAutoYaw();
        UpdateAutoPitch();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    #endregion Unity Methods

    #region Methods

    // --- Métodos Públicos ---

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

    // --- Métodos Privados ---

    private void HandleTargetSearch()
    {
        if (_target != null && _target.gameObject.activeInHierarchy)
        {
            return;
        }

        _searchTimer += Time.deltaTime;

        if (_searchTimer >= _searchInterval)
        {
            _searchTimer = 0f;
            FindClosestEnemy();
        }
    }

    private void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);

        if (enemies.Length == 0)
        {
            _target = null;
            return;
        }

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        _target = closestEnemy;
    }

    private void UpdateAutoYaw()
    {
        Vector3 directionToTarget = _target.position - _yawPivot.position;
        directionToTarget.y = 0f;

        if (directionToTarget.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion desiredWorldRotation = Quaternion.LookRotation(directionToTarget);
        Quaternion desiredLocalRotation = _yawPivot.parent != null
            ? Quaternion.Inverse(_yawPivot.parent.rotation) * desiredWorldRotation
            : desiredWorldRotation;

        // Sumamos el offset visual aquí
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
        Vector2 targetPosXZ = new Vector2(_target.position.x, _target.position.z);
        float distance = Vector2.Distance(turretPosXZ, targetPosXZ);

        _isTargetInRange = CalculateFiringAngles(distance, out float lowAngle, out float highAngle);

        if (!_isTargetInRange)
        {
            return;
        }

        float targetPitchAngle = _useHighAngle ? highAngle : lowAngle;

        // Sumamos el offset visual aquí
        targetPitchAngle += _pitchOffset;

        float clampedPitch = Mathf.Clamp(targetPitchAngle, _pitchLimits.x, _pitchLimits.y);
        float currentPitch = _pitchPivot.localEulerAngles.z;
        float newPitch = Mathf.MoveTowardsAngle(currentPitch, clampedPitch, _pitchSpeed * Time.deltaTime);

        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, newPitch);
    }

    #endregion Methods

    #region Unity Special Methods

    private void OnDrawGizmos()
    {
        if (_target != null && _yawPivot != null)
        {
            Gizmos.color = _isTargetInRange ? Color.green : Color.red;
            Gizmos.DrawLine(_bulletSpawn != null ? _bulletSpawn.position : _yawPivot.position, _target.position);
        }
    }

    #endregion Unity Special Methods
}
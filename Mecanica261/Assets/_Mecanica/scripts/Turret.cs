using UnityEngine;

public class Turret : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _targetPoint;

    [Header("Bullets")]
    [SerializeField] private GameObject _simpleBullet;
    [SerializeField] private GameObject _explosiveProjectile;

    [Header("Limits")]
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 80f);

    [Header("Target Area")]
    [SerializeField] private Vector2 _targetXLimits;
    [SerializeField] private Vector2 _targetZLimits;
    [SerializeField] private float _targetY = 0f;

    private GameObject _currentBullet;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _currentBullet = _simpleBullet;
    }

    private void Update()
    {
        UpdateTarget();
        RotateYaw();
        RotatePitch();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _currentBullet = _simpleBullet;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentBullet = _explosiveProjectile;
        }
    }

    #endregion

    #region Methods

    private void UpdateTarget()
    {
        float targetPositionX = Mathf.Lerp(_targetXLimits.x, _targetXLimits.y, Input.mousePosition.x / Screen.width);
        float targetPositionZ = Mathf.Lerp(_targetZLimits.x, _targetZLimits.y, Input.mousePosition.y / Screen.height);

        _targetPoint.position = new Vector3(_yawPivot.position.x + targetPositionX, _targetY, _yawPivot.position.z + targetPositionZ);
    }

    private void RotateYaw()
    {
        Vector3 directionToTarget = _targetPoint.position - _yawPivot.position;
        directionToTarget.y = 0f;

        float yawAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg - 90f;
        yawAngle = Mathf.Clamp(yawAngle, _yawLimits.x, _yawLimits.y);

        _yawPivot.localEulerAngles = new Vector3(0f, yawAngle, 0f);
    }

    private void RotatePitch()
    {
        IProjectile projectile = _currentBullet.GetComponent<IProjectile>();
        if (projectile == null) return;

        float projectileSpeed = projectile.Speed;
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 directionToTarget = _targetPoint.position - _bulletSpawn.position;
        Vector3 horizontalDirection = directionToTarget;
        horizontalDirection.y = 0f;

        float horizontalDistance = horizontalDirection.magnitude;
        float verticalDistance = directionToTarget.y;

        float projectileSpeedSquared = projectileSpeed * projectileSpeed;

        float discriminant = projectileSpeedSquared * projectileSpeedSquared - gravity * (gravity * horizontalDistance * horizontalDistance + 2 * verticalDistance * projectileSpeedSquared);

        if (discriminant < 0f) return;

        float squareRoot = Mathf.Sqrt(discriminant);

        float pitchAngle = Mathf.Atan((projectileSpeedSquared + squareRoot) / (gravity * horizontalDistance)) * Mathf.Rad2Deg;

        pitchAngle = Mathf.Clamp(pitchAngle, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3(-pitchAngle, 0f, 0f);
    }

    private void Fire()
    {
        GameObject spawnedBullet = Instantiate(_currentBullet, _bulletSpawn.position, _bulletSpawn.rotation);

        Rigidbody rigidbody = spawnedBullet.GetComponent<Rigidbody>();
        IProjectile projectile = spawnedBullet.GetComponent<IProjectile>();

        if (rigidbody == null || projectile == null)
        {
            Destroy(spawnedBullet);
            return;
        }

        rigidbody.linearVelocity = _bulletSpawn.forward * projectile.Speed;
        projectile.Fire();
    }

    #endregion
} 
// Trabajo en clase: Quitar las abreviaciones. Utilizar el ángulo alto en lugar del bajo. Utilizar DRY para evitar repetir codigo. 
using UnityEngine;

public class Turret : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [Tooltip("Pivot horizontal de la torreta.")]
    [SerializeField] private Transform _yawPivot;
    [Tooltip("Pivot vertical del cañón.")]
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _targetPoint;

    [Header("Bullets")]
    [SerializeField] private GameObject _simpleBullet;
    [SerializeField] private GameObject _explosiveProjectile;

    [Header("Yaw Settings")]
    [Tooltip("Límites de rotación horizontal.")]
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [Tooltip("Límites de rotación vertical.")]
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 80f);

    [Header("Target Area")]
    [SerializeField] private Vector2 _targetXLimits = new Vector2(-15f, 15f);
    [SerializeField] private Vector2 _targetZLimits = new Vector2(3f, 25f);
    [Tooltip("Altura fija del objetivo.")]
    [SerializeField] private float _targetY = 0f;

    private GameObject _currentBullet;
    private Vector3 _yawInitialRotation;
    private Vector3 _pitchInitialRotation;

    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        _currentBullet = _simpleBullet;

        _yawInitialRotation = _yawPivot.localEulerAngles;
        _pitchInitialRotation = _pitchPivot.localEulerAngles;
    }

    private void Update()
    {
        UpdateTargetPoint();
        RotateYaw();
        RotatePitch();
        ChangeBulletType();
        HandleFireInput();
    }

    #endregion Unity Methods

    #region Methods

    public void FireProjectile()
    {
        GameObject projectileObject = Instantiate(_currentBullet, _bulletSpawn.position, _bulletSpawn.rotation);

        IProjectile projectile = projectileObject.GetComponent<IProjectile>();

        if (projectile == null)
        {
            return;
        }

        projectile.Fire();
    }

    private void UpdateTargetPoint()
    {
        float normalizedMouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float normalizedMouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;

        float targetX = Mathf.Lerp(_targetXLimits.x, _targetXLimits.y, (normalizedMouseX + 1f) * 0.5f);
        float targetZ = Mathf.Lerp(_targetZLimits.x, _targetZLimits.y, (normalizedMouseY + 1f) * 0.5f);

        Vector3 targetPosition = new Vector3(
            _yawPivot.position.x + targetX,
            _targetY,
            _yawPivot.position.z + targetZ
        );

        _targetPoint.position = targetPosition;
    }

    private void RotateYaw()
    {
        Vector3 directionToTarget = _targetPoint.position - _yawPivot.position;
        directionToTarget.y = 0f;

        if (directionToTarget == Vector3.zero)
        {
            return;
        }

        float yawAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg - 90f;
        float clampedYawAngle = Mathf.Clamp(yawAngle, _yawLimits.x, _yawLimits.y);

        _yawPivot.localEulerAngles = new Vector3(
            _yawInitialRotation.x,
            _yawInitialRotation.y + clampedYawAngle,
            _yawInitialRotation.z
        );
    }

    private void RotatePitch()
    {
        IProjectile currentProjectile = _currentBullet.GetComponent<IProjectile>();

        if (currentProjectile == null)
        {
            return;
        }

        float speed = currentProjectile.Speed;
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 startPosition = _bulletSpawn.position;
        Vector3 flatDirectionToTarget = _targetPoint.position - startPosition;
        flatDirectionToTarget.y = 0f;

        float horizontalDistance = flatDirectionToTarget.magnitude;
        float deltaY = _targetPoint.position.y - startPosition.y;

        if (horizontalDistance <= 0.01f)
        {
            return;
        }

        float speedSquared = speed * speed;
        float speedFourth = speedSquared * speedSquared;

        float sqrtValueContent = speedFourth - gravity * (gravity * horizontalDistance * horizontalDistance + 2f * deltaY * speedSquared);

        if (sqrtValueContent < 0f)
        {
            return;
        }

        float sqrtValue = Mathf.Sqrt(sqrtValueContent);
        float highAngle = Mathf.Atan((speedSquared + sqrtValue) / (gravity * horizontalDistance)) * Mathf.Rad2Deg;
        float clampedPitchAngle = Mathf.Clamp(highAngle, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3(
            _pitchInitialRotation.x - clampedPitchAngle,
            _pitchInitialRotation.y,
            _pitchInitialRotation.z
        );
    }

    private void ChangeBulletType()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _currentBullet = _simpleBullet;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentBullet = _explosiveProjectile;
        }
    }

    private void HandleFireInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        FireProjectile();
    }

    #endregion Methods
}
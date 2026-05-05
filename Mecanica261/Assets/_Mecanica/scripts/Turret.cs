using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _target;

    [Header("Bullets")]
    [SerializeField] private GameObject _simpleBullet;
    [SerializeField] private GameObject _explosiveProjectile;

    [Header("Limits")]
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 80f);

    [Header("Detection")]
    [SerializeField] private float _detectionRange = 100f;
    [SerializeField] private float _fireInterval = 3f;

    private GameObject _currentBullet;
    private float _fireTimer;

    private void Start()
    {
        _currentBullet = _simpleBullet;
    }

    private void Update()
    {
        UpdateBulletSelection();

        if (_target == null) 
        { 
            return;
        } 

        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget > _detectionRange)
        {
            _fireTimer = 0f;
            return;
        }

        RotateYaw();
        RotatePitch();

        _fireTimer += Time.deltaTime;

        if (_fireTimer >= _fireInterval)
        {
            Fire();
            _fireTimer = 0f;
        }
    }

    private void RotateYaw()
    {
        Vector3 directionToTarget = _target.position - _yawPivot.position;
        directionToTarget.y = 0f;

        if (directionToTarget.sqrMagnitude <= 0f) 
        { 
            return;
        }

        float yawAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg - 90f;
        yawAngle = Mathf.Clamp(yawAngle, _yawLimits.x, _yawLimits.y);
        _yawPivot.localEulerAngles = new Vector3(0f, yawAngle, 0f);
    }

    private void RotatePitch()
    {
        if (!TryGetLaunchAngle(out float launchAngle)) 
        { 
            return; 
        }

        launchAngle = Mathf.Clamp(launchAngle, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(-launchAngle, 0f, 0f);
    }

    private bool TryGetLaunchAngle(out float launchAngle)
    {
        launchAngle = 0f;

        if (_currentBullet == null) 
        { 
            return false; 
        }

        IProjectile projectile = _currentBullet.GetComponent<IProjectile>();
        if (projectile == null) 
        { 
            return false; 
        }

        float speed = projectile.Speed;
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 directionToTarget = _target.position - _bulletSpawn.position;
        Vector3 horizontalDirection = directionToTarget;
        horizontalDirection.y = 0f;

        float horizontalDistance = horizontalDirection.magnitude;
        float verticalDistance = directionToTarget.y;

        if (horizontalDistance <= 0f) 
        { 
            return false; 
        }

        float speedSquared = speed * speed;
        float discriminant = speedSquared * speedSquared - gravity * (gravity * horizontalDistance * horizontalDistance + 2f * verticalDistance * speedSquared);

        if (discriminant < 0f) 
        { 
            return false; 
        }

        float squareRoot = Mathf.Sqrt(discriminant);
        float highAngle = Mathf.Atan((speedSquared + squareRoot) / (gravity * horizontalDistance));
        launchAngle = highAngle * Mathf.Rad2Deg;
        return true;
    }

    private void Fire()
    {
        if (_currentBullet == null) 
        { 
            return; 
        }

        GameObject bulletInstance = Instantiate(_currentBullet, _bulletSpawn.position, _bulletSpawn.rotation);
        Rigidbody bulletRigidbody = bulletInstance.GetComponent<Rigidbody>();
        IProjectile projectile = bulletInstance.GetComponent<IProjectile>();

        if (bulletRigidbody == null || projectile == null)
        {
            Destroy(bulletInstance);
            return;
        }

        bulletRigidbody.linearVelocity = _bulletSpawn.forward * projectile.Speed;
        projectile.Fire();
    }

    private void UpdateBulletSelection()
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
}
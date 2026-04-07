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
        float x = Mathf.Lerp(_targetXLimits.x, _targetXLimits.y, Input.mousePosition.x / Screen.width);
        float z = Mathf.Lerp(_targetZLimits.x, _targetZLimits.y, Input.mousePosition.y / Screen.height);

        _targetPoint.position = new Vector3(
            _yawPivot.position.x + x,
            _targetY,
            _yawPivot.position.z + z
        );
    }

    private void RotateYaw()
    {
        Vector3 dir = _targetPoint.position - _yawPivot.position;
        dir.y = 0f;

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg - 90f;
        angle = Mathf.Clamp(angle, _yawLimits.x, _yawLimits.y);

        _yawPivot.localEulerAngles = new Vector3(0f, angle, 0f);
    }

    private void RotatePitch()
    {
        IProjectile projectile = _currentBullet.GetComponent<IProjectile>();
        if (projectile == null) return;

        float speed = projectile.Speed;
        float g = Mathf.Abs(Physics.gravity.y);

        Vector3 dir = _targetPoint.position - _bulletSpawn.position;
        Vector3 flat = dir; flat.y = 0f;

        float x = flat.magnitude;
        float y = dir.y;

        float v2 = speed * speed;

        float inside = v2 * v2 - g * (g * x * x + 2 * y * v2);

        if (inside < 0f) return;

        float sqrt = Mathf.Sqrt(inside);

     
        float angle = Mathf.Atan((v2 - sqrt) / (g * x)) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3(-angle, 0f, 0f);
    }

    private void Fire()
    {
        GameObject obj = Instantiate(_currentBullet, _bulletSpawn.position, _bulletSpawn.rotation);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        IProjectile projectile = obj.GetComponent<IProjectile>();

        if (rb == null || projectile == null) return;

        float speed = projectile.Speed;
        float g = Mathf.Abs(Physics.gravity.y);

        Vector3 dir = _targetPoint.position - _bulletSpawn.position;
        Vector3 flat = dir; flat.y = 0f;

        float x = flat.magnitude;
        float y = dir.y;

        float v2 = speed * speed;

        float inside = v2 * v2 - g * (g * x * x + 2 * y * v2);

        if (inside < 0f)
        {
            Destroy(obj);
            return;
        }

        float sqrt = Mathf.Sqrt(inside);
        float angle = Mathf.Atan((v2 - sqrt) / (g * x));

        float vx = Mathf.Cos(angle) * speed;
        float vy = Mathf.Sin(angle) * speed;

        Vector3 velocity =
            flat.normalized * vx +
            Vector3.up * vy;

        rb.linearVelocity = velocity;

        projectile.Fire();
    }

    #endregion
}
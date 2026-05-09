using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    #region Variables

    [Header("Dependencies")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Rigidbody _projectilePrefab;

    [Header("Shooting Settings")]
    [SerializeField] private float _launchSpeed = 50f;
    [SerializeField] private bool _useHighAngle = true;
    [SerializeField] private float _fireRate = 1f;

    private float _currentCooldown;

    #endregion Variables


    #region Unity Methods

    private void Update()
    {
        HandleShooting();
    }

    #endregion Unity Methods


    #region Methods

    private void HandleShooting()
    {
        if (_target == null) { return; }

        _currentCooldown -= Time.deltaTime;

        if (_currentCooldown > 0f) { return; }

        Shoot();

        _currentCooldown = 1f / _fireRate;
    }


    private void Shoot()
    {
        if (_projectilePrefab == null || _firePoint == null) { return; }

        Vector3 velocity = CalculateLaunchVelocity();

        if (velocity == Vector3.zero) { return; }

        Rigidbody projectileInstance = Instantiate(_projectilePrefab,_firePoint.position,Quaternion.identity);

        projectileInstance.linearVelocity = velocity;
    }


    private Vector3 CalculateLaunchVelocity()
    {
        Vector3 start = _firePoint.position;
        Vector3 targetPosition = _target.position;

        Vector3 direction = targetPosition - start;

        float y = direction.y;
        direction.y = 0f;

        float x = direction.magnitude;

        float gravity = Physics.gravity.magnitude;
        float speedSquared = _launchSpeed * _launchSpeed;

        float underRoot = speedSquared * speedSquared - gravity * (gravity * x * x + 2 * y * speedSquared);

        if (underRoot < 0f) { return Vector3.zero; }

        float root = Mathf.Sqrt(underRoot);

        float angle;

        if (_useHighAngle)
        {
            angle = Mathf.Atan((speedSquared + root) / (gravity * x));
        }
        else
        {
            angle = Mathf.Atan((speedSquared - root) / (gravity * x));
        }

        Vector3 velocity = direction.normalized * _launchSpeed * Mathf.Cos(angle);
        velocity.y = _launchSpeed * Mathf.Sin(angle);

        return velocity;
    }

    #endregion Methods
}

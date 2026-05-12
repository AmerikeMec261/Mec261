using System.Runtime.CompilerServices;
using UnityEngine;

public class New_Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _gunPointLeft;
    [SerializeField] private Transform _gunPointRight;
    [SerializeField] private GameObject _normalBullet;
    [SerializeField] private GameObject _explosiveBullet;
    [SerializeField] private bool _useHighArc = false;

    [Header("Deteccion")]
    [SerializeField] private float _detectionRange = 50f;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Yaw")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private float _yawMin = -80f;
    [SerializeField] private float _yawMax = 80f;

    [Header("Pitch")]
    [SerializeField] private float _pitchSpeed = 45f;
    [SerializeField] private float _pitchMin = -5f;
    [SerializeField] private float _pitchMax = 45f;

    private GameObject _currentBullet;
    private Transform _currentTarget;
    private float _currentYaw = 0f;
    private float _currentPitch = 0f;
    private bool _fireLeft = true;

    private Quaternion _yawOrigin;
    private Quaternion _pitchOrigin;

    private void Start()
    {
        _currentBullet = _normalBullet;
        _yawOrigin = _yawPivot.localRotation;
        _pitchOrigin = _pitchPivot.localRotation;
    }

    private void Update()
    {
        DetectEnemy();

        if (_currentTarget != null)
        {
            RotateYaw();
            RotatePitch();
        }
        else
        {
            ReturnToOrigin();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            _currentBullet = _currentBullet == _normalBullet ? _explosiveBullet : _normalBullet;

        if (Input.GetKeyDown(KeyCode.Space) && _currentTarget != null)
            FireProjectile();
    }

    private void DetectEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRange, _enemyLayer);

        if (hits.Length > 0)
        {
            float closest = Mathf.Infinity;
            foreach (Collider hit in hits)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closest)
                {
                    closest = dist;
                    _currentTarget = hit.transform;
                }
            }
        }
        else
        {
            _currentTarget = null;
        }
    }

    private void RotateYaw()
    {
        Vector3 localDirection = _yawPivot.parent.InverseTransformPoint(_currentTarget.position);
        localDirection.y = 0f;

        float targetYaw = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;
        _currentYaw = Mathf.Clamp(targetYaw, _yawMin, _yawMax);

        float currentY = _yawPivot.localEulerAngles.y > 180f ? _yawPivot.localEulerAngles.y - 360f : _yawPivot.localEulerAngles.y;

        float newYaw = Mathf.MoveTowardsAngle(currentY, _currentYaw, _yawSpeed * Time.deltaTime);
        _yawPivot.localEulerAngles = new Vector3(0f, newYaw, 0f);
    }

    private void RotatePitch()
    {
        Vector3 localTarget = _pitchPivot.InverseTransformPoint(_currentTarget.position);
        float targetPitch = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
        _currentPitch = Mathf.Clamp(targetPitch, _pitchMin, _pitchMax);

        float currentX = _pitchPivot.localEulerAngles.x > 180f ? _pitchPivot.localEulerAngles.x - 360f : _pitchPivot.localEulerAngles.x;

        float newPitch = Mathf.MoveTowardsAngle(currentX, _currentPitch, _pitchSpeed * Time.deltaTime);
        _pitchPivot.localEulerAngles = new Vector3(newPitch, 0f, 0f);
    }

    private void ReturnToOrigin()
    {
        _yawPivot.localRotation = Quaternion.Lerp(_yawPivot.localRotation, _yawOrigin, _yawSpeed * Time.deltaTime);
        _pitchPivot.localRotation = Quaternion.Lerp(_pitchPivot.localRotation, _pitchOrigin, _pitchSpeed * Time.deltaTime);
    }

    public void FireProjectile()
    {
        
        Transform spawnPoint = _fireLeft ? _gunPointLeft : _gunPointRight;
        _fireLeft = !_fireLeft;

        GameObject bullet = Instantiate(_currentBullet, spawnPoint.position, spawnPoint.rotation);
        IProjectile projectile = bullet.GetComponent<IProjectile>();

        if (projectile == null) { Destroy(bullet); return; }

        if (!SolveBallisticAngle(spawnPoint.position, _currentTarget.position, projectile.Speed, out float launchAngle))
        {
            Debug.LogWarning("[Torreta] Sin solucion — sube el Speed de la bala.");
            Destroy(bullet);
            return;
        }

        Vector3 horizontalDir = new Vector3(_currentTarget.position.x - spawnPoint.position.x, 0f, _currentTarget.position.z - spawnPoint.position.z).normalized;

        Vector3 velocity = horizontalDir * projectile.Speed * Mathf.Cos(launchAngle);
        velocity.y = projectile.Speed * Mathf.Sin(launchAngle);

        projectile.Fire(velocity);
    }

    private bool SolveBallisticAngle(Vector3 origin, Vector3 target, float speed, out float launchAngle)
    {
        float gravity = Physics.gravity.magnitude;
        float horizontalDistance = Vector3.Distance(new Vector3(origin.x, 0f, origin.z), new Vector3(target.x, 0f, target.z));
        float heightDifference = target.y - origin.y;
        float speedSquare = speed * speed;
        float speedFourth = speedSquare * speedSquare;
        float discriminant = speedFourth - gravity * (gravity * horizontalDistance * horizontalDistance + 2f * heightDifference * speedSquare);

        if (discriminant < 0f) { launchAngle = 0f; return false; }

        float squareRoot = Mathf.Sqrt(discriminant);
        float lowAngle = Mathf.Atan((speedSquare - squareRoot) / (gravity * horizontalDistance));
        float highAngle = Mathf.Atan((speedSquare + squareRoot) / (gravity * horizontalDistance));

        launchAngle = _useHighArc ? highAngle : lowAngle;
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

    //https://www.youtube.com/watch?v=XTJEgWcHd2o
    //https://gist.github.com/gengen1988/bc95661693dafc2d3bae486b86ef323a
    //Inspiración en cuanto a la creación de la torreta y un poco en el disparo
}

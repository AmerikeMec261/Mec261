
using UnityEngine;

public class Torreta : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _normalBullet;
    [SerializeField] private GameObject _explosiveBullet;
    [SerializeField] private bool _useHighArc = false;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);
    

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);


    private GameObject _currentBullet;
    private float _currentPitch = 0f;
    private float _currentYaw = 0f;
    private Vector3 _targetPoint;

    private void Start()
    {
        _currentBullet=_normalBullet; 
    }

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_currentBullet, _bulletSpawn.position, _bulletSpawn.rotation);
        IProjectile projectile = currentBullet.GetComponent<IProjectile>();

        if (projectile == null) return;

        if (!SolveBallisticAngle(_bulletSpawn.position, _targetPoint, projectile.Speed, out float launchAngle))
        {
            Destroy(currentBullet);
            return;
        }

        Vector3 horizontalDirection = new Vector3(
            _targetPoint.x - _bulletSpawn.position.x,
            0f,
            _targetPoint.z - _bulletSpawn.position.z
        ).normalized;

        Vector3 velocity = horizontalDirection * projectile.Speed * Mathf.Cos(launchAngle);
        velocity.y = projectile.Speed * Mathf.Sin(launchAngle);

        projectile.Fire(velocity);
    }

    private bool SolveBallisticAngle(Vector3 originPosition, Vector3 targetPosition, float projectileSpeed, out float launchAngle)
    {
        float gravity = Physics.gravity.magnitude;

        Vector3 horizontalVector = new Vector3(
            targetPosition.x - originPosition.x,
            0f,
            targetPosition.z - originPosition.z
        );

        float horizontalDistance = horizontalVector.magnitude;

        float heightDifference = targetPosition.y - originPosition.y;

        float speedSquared = projectileSpeed * projectileSpeed;
        float speedFourth = speedSquared * speedSquared;

        float discriminant = speedFourth - gravity * (gravity * horizontalDistance * horizontalDistance + 2 * heightDifference * speedSquared);

        if (discriminant < 0f)
        {
            launchAngle = 0f;
            return false;
        }

        float squareRoot = Mathf.Sqrt(discriminant);

        float lowAngle = Mathf.Atan((speedSquared - squareRoot) / (gravity * horizontalDistance));
        float highAngle = Mathf.Atan((speedSquared + squareRoot) / (gravity * horizontalDistance));

        launchAngle = _useHighArc ? highAngle : lowAngle;

        return true;
    }

    private void Update()
    {
        RotateMouse();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            _currentBullet=_currentBullet==_normalBullet?_explosiveBullet:_normalBullet;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void RotateYaw(float input)
    {
        float yawChange = input * _yawSpeed * Time.deltaTime;
        float newYaw = Mathf.MoveTowardsAngle(_yawPivot.localEulerAngles.y > 180f ? _yawPivot.localEulerAngles.y - 360f:_yawPivot.localEulerAngles.y,_currentYaw,_yawSpeed*Time.deltaTime);
        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles.z);
    }

    private void RotatePitch(float input)
    {
        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float curentX = _pitchPivot.localEulerAngles.x >180f?_pitchPivot.localEulerAngles.x-360f:_pitchPivot.localEulerAngles.x;
        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, curentX);
        float newPitch = Mathf.MoveTowardsAngle(curentX, _currentPitch, _pitchSpeed * Time.deltaTime);
    }


    private void RotateMouse()
    {
        Vector3 yawDirection = _targetPoint - _yawPivot.position;
        yawDirection.y = 0f;

        if (yawDirection != Vector3.zero)
        {
            Quaternion targetYaw = Quaternion.LookRotation(yawDirection);
            _yawPivot.rotation = Quaternion.Lerp(_yawPivot.rotation, targetYaw, _yawSpeed * Time.deltaTime);
        }

        Vector3 localTarget = _pitchPivot.InverseTransformPoint(_targetPoint); //El inverseTransformPoint calcula una posición relativa basandose en la posición, rotación, y escala del objeto, mientras que usar Vector3 -1 es una inversión matemática de los valores x,y,z
        float targetPitch = -Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
        _currentPitch = Mathf.Clamp(targetPitch, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localRotation = Quaternion.Lerp( _pitchPivot.localRotation,Quaternion.Euler(_currentPitch, 0f, 0f),_pitchSpeed * Time.deltaTime); 
    }
}  
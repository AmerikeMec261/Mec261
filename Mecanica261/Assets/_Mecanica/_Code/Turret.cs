using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _target;

    [Header("Modes")]
    [SerializeField] private bool _autoAimEnabled = false; // Presiona 'T' para cambiar de modo

    [Header("Manual Settings")]
    [SerializeField] private float _manualSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Auto Settings")]
    [SerializeField] private float _autoRotationSpeed = 5f;

    private float _currentYaw;
    private float _currentPitch;

    private void Update()
    {
        // Alternar modo con la tecla T
        if (Input.GetKeyDown(KeyCode.T)) _autoAimEnabled = !_autoAimEnabled;

        if (_autoAimEnabled && _target != null)
        {
            AutoAim();
        }
        else
        {
            ManualControl();
        }

        // El disparo manual
        if (Input.GetKeyDown(KeyCode.Space)) DealDamage();
    }

    private void ManualControl()
    {
        float yawInput = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
        float pitchInput = Input.GetKey(KeyCode.W) ? -1f : Input.GetKey(KeyCode.S) ? 1f : 0f;

        // Yaw y Pitch Manuales
        _currentYaw += yawInput * _manualSpeed * Time.deltaTime;
        _yawPivot.localRotation = Quaternion.Euler(0, _currentYaw, 0);

        _currentPitch += pitchInput * _manualSpeed * Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localRotation = Quaternion.Euler(_currentPitch, 0, 0);
    }

    private void AutoAim()
    {
        Vector3 direction = _target.position - transform.position;

        // Yaw y Pitch Automáticos
        Vector3 planarDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        Quaternion targetYaw = Quaternion.LookRotation(planarDirection);
        _yawPivot.rotation = Quaternion.Slerp(_yawPivot.rotation, targetYaw, Time.deltaTime * _autoRotationSpeed);

        
        Vector3 localTargetPos = _yawPivot.InverseTransformPoint(_target.position);
        float angle = -Mathf.Atan2(localTargetPos.y, localTargetPos.z) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, _pitchLimits.x, _pitchLimits.y);

        Quaternion targetPitch = Quaternion.Euler(angle, 0, 0);
        _pitchPivot.localRotation = Quaternion.Slerp(_pitchPivot.localRotation, targetPitch, Time.deltaTime * _autoRotationSpeed);
      
        _currentYaw = _yawPivot.localEulerAngles.y;
        _currentPitch = angle;
    }

    public void DealDamage()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        IProjectile projectile = currentBullet.GetComponent<IProjectile>();
        if (projectile != null)
        {
            projectile.SetDamage(20f);
            projectile.Fire();
        }
    }
}

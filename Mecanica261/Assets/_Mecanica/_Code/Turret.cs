using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _target;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Modes")]
    [SerializeField] private bool _autoAimEnabled = false; // Presiona 'T' para cambiar de modo

    [Header("Reticula")]
    [SerializeField] private Transform _reticula;

    [Header("Auto Settings")]
    [SerializeField] private float _autoRotationSpeed = 5f;

    [Header("Bullet")]
    [SerializeField] private GameObject _explosiveBullet;
    private bool _usingExplosive = false;
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
            RotateMouse();
           
            if (Input.GetKeyDown(KeyCode.X))
            {
                _usingExplosive = !_usingExplosive;
                Debug.Log(_usingExplosive ? "Bala Explosiva" : "Bala Normal");
            }

        }

        // El disparo manual
        if (Input.GetKeyDown(KeyCode.Space)) FireProjectile();
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

    public void FireProjectile()
    {
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        IProjectile projectile = currentBullet.GetComponent<IProjectile>();
        if (projectile != null)
        {
            projectile.SetDamage(20f);
            projectile.Fire();
        }
    }

    private void RotateMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        int layerMask = LayerMask.GetMask("Default");
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPoint = hit.point;

            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion correctedRotation = targetRotation * Quaternion.Euler(0f, 180f, 0f);
                _yawPivot.rotation = Quaternion.Lerp(
                    _yawPivot.rotation, correctedRotation, _yawSpeed * Time.deltaTime);

            }


            float horizontalDistance = direction.magnitude;
            float heightDifference = hit.point.y - _pitchPivot.position.y;
            float targetPitch = Mathf.Atan2(heightDifference, horizontalDistance) * Mathf.Rad2Deg;
            _currentPitch = Mathf.Clamp(targetPitch, _pitchLimits.x, _pitchLimits.y);
            _pitchPivot.localEulerAngles = new Vector3(_currentPitch, 0f, 0f);
         
            if (_reticula != null)
                _reticula.position = new Vector3(hit.point.x, hit.point.y + 0.01f, hit.point.z);
        }


    }

}

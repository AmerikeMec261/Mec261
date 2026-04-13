using UnityEngine;

public class Turret : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _reticle;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 60f);

    [Header("Reticle Settings")]
    [SerializeField] private float _maxRange = 50f;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main; // Porqué se hace esto? 
    }

    private void Update()
    {
        AimAtMouse();
        UpdateReticle();

        if (Input.GetKeyDown(KeyCode.Space)) { FireProjectile(); }
    }

    private void AimAtMouse()
    {
        Ray mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(mouseRay, out RaycastHit hit)) { return; }

        Vector3 direction = hit.point - _yawPivot.position;

        
        float targetYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float clampedYaw = Mathf.Clamp(targetYaw, _yawLimits.x, _yawLimits.y);
        _yawPivot.localEulerAngles = new Vector3(0f, clampedYaw, 0f);

        float horizontalDistance = new Vector3(direction.x, 0f, direction.z).magnitude;
        float targetPitch = -Mathf.Atan2(direction.y, horizontalDistance) * Mathf.Rad2Deg;
        float clampedPitch = Mathf.Clamp(targetPitch, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(clampedPitch, 0f, 0f);
    }

    private void UpdateReticle()
    {
        if (_reticle == null) { return; }

        Ray bulletRay = new Ray(_bulletSpawn.position, _bulletSpawn.forward);

        if (Physics.Raycast(bulletRay, out RaycastHit hit, _maxRange))
        {
            _reticle.position = hit.point;
        }
        else
        {
            _reticle.position = _bulletSpawn.position + _bulletSpawn.forward * _maxRange;
        }
    }

    private void FireProjectile()
    {
        GameObject spawnedBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        spawnedBullet.GetComponent<IProjectile>()?.Fire();
    }
} // Trabajo en clase: usar la fórmula vista en clase para el cálculo del disparo. 
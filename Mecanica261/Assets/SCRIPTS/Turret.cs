using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Disparo")]
    [SerializeField] private float _bulletSpeed = 20f;

    private Camera _mainCamera;
    private Vector3 _targetPoint;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        AimAtMouse();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void AimAtMouse()
    {
        Ray mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(mouseRay, out RaycastHit hit))
            return;

        _targetPoint = hit.point;

        Vector3 direction = hit.point - _yawPivot.position;

   
        float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        _yawPivot.rotation = Quaternion.Euler(0f, yaw, 0f);

       
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
        float x = flatDirection.magnitude;

      
        float y = hit.point.y - _bulletSpawn.position.y;

        float g = Physics.gravity.magnitude;
        float v2 = _bulletSpeed * _bulletSpeed;

        
        float raiz =
            (v2 * v2) -
            g * (g * x * x + 2 * y * v2);

      
        if (raiz < 0)
            return;

       
        float angle =
            Mathf.Atan(
                (v2 - Mathf.Sqrt(raiz))
                / (g * x)
            );

      
        _pitchPivot.localEulerAngles =
            new Vector3(-angle * Mathf.Rad2Deg, 0f, 0f);
    }

    private void FireProjectile()
    {
        GameObject bullet =
            Instantiate(
                _bulletPrefab,
                _bulletSpawn.position,
                _bulletSpawn.rotation
            );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                _bulletSpawn.forward * _bulletSpeed;
        }
    }
}
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _reticle;

    [Header("Bullet Prefabs")]
    [SerializeField] private GameObject _simpleBulletPrefab;
    [SerializeField] private GameObject _explosiveBulletPrefab;

    private GameObject _currentBullet;

    [Header("Settings")]
    [SerializeField] private float _bulletSpeed = 25f;
    [SerializeField] private float _maxRange = 50f;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main; // porqué necesitas esta referencia? 
        _currentBullet = _simpleBulletPrefab;
    }

    private void Update()
    {
        Aim();
        UpdateReticle();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchBullet();
        }
    }

    
    private void Aim()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxRange))
        {
            Vector3 direction = hit.point - _yawPivot.position;

            
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
            if (flatDirection != Vector3.zero)
            {
                _yawPivot.rotation = Quaternion.LookRotation(flatDirection);
            }

            
            Vector3 localDirection = _yawPivot.InverseTransformDirection(direction);
            float angle = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;
            _pitchPivot.localRotation = Quaternion.Euler(-angle, 0, 0);
        }
    }

    
    private void UpdateReticle()
    {
        if (_reticle == null) return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxRange))
        {
            _reticle.position = hit.point;
        }
    }


    private void Shoot()
    {
        GameObject bullet = Instantiate(_currentBullet, _bulletSpawn.position, Quaternion.identity);

        if (!bullet.TryGetComponent<Rigidbody>(out Rigidbody rb)) return;

        Vector3 start = _bulletSpawn.position;
        Vector3 target = _reticle.position;

        Vector3 direction = target - start;

        float x = new Vector3(direction.x, 0, direction.z).magnitude; 
        float y = direction.y; 

        float g = Mathf.Abs(Physics.gravity.y); 
        float v = _bulletSpeed; 

        float v2 = v * v;
        float v4 = v2 * v2;

        float insideSqrt = v4 - g * (g * x * x + 2 * y * v2);

        
        if (insideSqrt < 0)
        {
            Debug.Log("No hay solución balística");
            return;
        }

        float sqrt = Mathf.Sqrt(insideSqrt);

        
        float angle = Mathf.Atan((v2 - sqrt) / (g * x));

        
        Vector3 dir = new Vector3(direction.x, 0, direction.z).normalized;

        
        Vector3 velocity =
            dir * v * Mathf.Cos(angle) +
            Vector3.up * v * Mathf.Sin(angle);

        
        rb.linearVelocity = velocity;
    }


    private void SwitchBullet()
    {
        if (_currentBullet == _simpleBulletPrefab)
        {
            _currentBullet = _explosiveBulletPrefab;
            Debug.Log("Bala explosiva");
        }
        else
        {
            _currentBullet = _simpleBulletPrefab;
            Debug.Log("Bala normal");
        }
    }
}// Trabajo en clase: Usar la formula que vimos en clase para calcular el ángulo de disparo y resolver los comentarios

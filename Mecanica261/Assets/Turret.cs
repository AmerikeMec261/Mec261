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

        Vector3 targetPoint = _reticle.position;
        Vector3 direction = (targetPoint - _bulletSpawn.position).normalized;

        Vector3 velocity = direction * _bulletSpeed;

        if (bullet.TryGetComponent<IProjectile>(out IProjectile projectile))
        {
            projectile.Shoot(velocity); //La bala no necesita una direccion. El barril del cañon determina la direccion del disparo parabólico.
        }
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

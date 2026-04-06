using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private float _maxDistance = 20f;

    [Header("Crosshair")]
    [SerializeField] private Transform _crosshair;

    [Header("Projectile")]
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private GameObject _simpleBulletPrefab;
    [SerializeField] private GameObject _explosiveBulletPrefab;

    private GameObject _currentProjectilePrefab;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _currentProjectilePrefab = _simpleBulletPrefab;
    }

    private void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentProjectilePrefab = _simpleBulletPrefab;
            Debug.Log("Bala simple seleccionada");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentProjectilePrefab = _explosiveBulletPrefab;
            Debug.Log("Bala explosiva seleccionada");
        }
    }

    private void FollowMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 target = hit.point;

            _crosshair.position = target + Vector3.up * 0.02f;
            
            Vector3 flatDirection = target - _yawPivot.position;
            flatDirection.y = 0;

            _yawPivot.rotation = Quaternion.LookRotation(-flatDirection);

            Vector3 fullDirection = target - _pitchPivot.position;
            float distance = flatDirection.magnitude;
            float height = fullDirection.y;

            float angle = CalculateHighAngle(distance, height, 20f);

            _pitchPivot.localRotation = Quaternion.Euler(2f, 0f, 0f);
        }
    }
    private float CalculateHighAngle(float distance, float height, float speed)
    {
        float g = Physics.gravity.magnitude;

        float speedSquared = speed * speed;
        float discriminant = (speedSquared * speedSquared) -
                             g * (g * distance * distance + 2 * height * speedSquared);

        if (discriminant < 0)
        {
            Vector3 maxPoint = _yawPivot.position + _yawPivot.forward * _maxDistance;
            _crosshair.position = maxPoint + Vector3.up * 0.02f;

            return 45f;
        }

        float angle = Mathf.Atan(
            (speedSquared + Mathf.Sqrt(discriminant)) /
            (g * distance)
        );

        return angle * Mathf.Rad2Deg;
    }

    private void FireProjectile()
    {
        GameObject bullet = Instantiate(
           _currentProjectilePrefab,
            _spawnPoint.position,
            _spawnPoint.rotation
        );

        IProjectile projectile = bullet.GetComponent<IProjectile>();

        if (projectile != null)
        {
            projectile.Fire();
        }
    }


}

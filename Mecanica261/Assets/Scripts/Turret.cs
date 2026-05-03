using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] private Transform _horizontalPivot;
    [SerializeField] private Transform _verticalPivot;
    [SerializeField] private float _maxDistance = 20f;


    [Header("Crosshair")]
    [SerializeField] private Transform _crosshair;

    [Header("Projectile")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _basicBulletPrefab;
    [SerializeField] private GameObject _explosiveBulletPrefab;


    [Header("Aiming Settings")]
    [SerializeField] private float _baseAngle = 95f;
    [SerializeField] private float _projectileSpeed = 18f;

    private GameObject _currentProjectile;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _currentProjectile = _basicBulletPrefab;
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
            _currentProjectile = _basicBulletPrefab;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentProjectile = _explosiveBulletPrefab;
        }
    }

    private void FireProjectile()
    {
        GameObject bullet = Instantiate( _currentProjectile, _spawnPoint.position, _spawnPoint.rotation);

        bullet.GetComponent<IProjectile>()?.Fire();
    }

    private void FollowMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 targetPoint = hit.point;

            _crosshair.position = targetPoint + Vector3.up * 0.02f;

            Vector3 horizontalDirection = targetPoint - _horizontalPivot.position;
            horizontalDirection.y = 0;

            _horizontalPivot.rotation = Quaternion.LookRotation(-horizontalDirection);

            Vector3 fullDirection = targetPoint - _verticalPivot.position;

            float horizontalDistance = horizontalDirection.magnitude;
            float heightDifference = fullDirection.y;

            float launchAngle = CalculateHighAngle(horizontalDistance, heightDifference, _projectileSpeed);

            _verticalPivot.localRotation = Quaternion.Euler(_baseAngle - launchAngle, 0f, 0f);
        }
    }


    private float CalculateHighAngle(float distance, float height, float speed)
    {
        float gravity = Physics.gravity.magnitude;

        float speedSquared = speed * speed;
        float discriminant = (speedSquared * speedSquared) - gravity * (gravity * distance * distance + 2 * height * speedSquared);

        if (discriminant < 0)
        {
            Vector3 maxPoint = _horizontalPivot.position + _horizontalPivot.forward * _maxDistance;
            _crosshair.position = maxPoint + Vector3.up * 0.02f;

            return 45f;
        }

        float angle = Mathf.Atan((speedSquared + Mathf.Sqrt(discriminant)) /(gravity * distance));

        return angle * Mathf.Rad2Deg;
    }
}

using UnityEngine;

public class torretabarco : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] private Transform _pivotHorizontal;
    [SerializeField] private Transform _pivotVertical;
    [SerializeField] private float _distance = 20f;


    [Header("Projectile")]
    [SerializeField] private Transform _spawn;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private GameObject _explosiveBullet;

    [SerializeField] private float Angulo = 95f;
    [SerializeField] private float VelocidadMouse = 18f;

    [SerializeField] private Transform _enemy;

    private GameObject _currentProjectile;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _currentProjectile = _BulletPrefab;
    }

    private void Update()
    {
        Direccion();

        if (Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }
    }


    private void FireProjectile()
    {
        GameObject bullet = Instantiate(_currentProjectile, _spawn.position, _spawn.rotation);

        bullet.GetComponent<IProjectile>()?.Fire();
    }

      private void Direccion()
    {
    if (_enemy == null) return;

    Vector3 target = _enemy.position;


    Vector3 flatDirection = target - _pivotHorizontal.position;

    flatDirection.y = 0f;

    if (flatDirection != Vector3.zero)
    {
        Quaternion lookRotation = Quaternion.LookRotation(flatDirection);

        _pivotHorizontal.rotation = Quaternion.Euler(
            0f,
            lookRotation.eulerAngles.y,
            0f
        );
    }

    Vector3 fullDirection = target - _pivotVertical.position;

    float dis = flatDirection.magnitude;
    float alt = fullDirection.y;

    float angle = CalculateHighAngle(dis, alt, VelocidadMouse);

    _pivotVertical.localRotation =
        Quaternion.Euler(Angulo - angle, 0f, 0f);
}

private float CalculateHighAngle(float distance, float height, float speed)
    {
        float gravity = Physics.gravity.magnitude;

        float speedSquared = speed * speed;
        float discriminant = (speedSquared * speedSquared) - gravity * (gravity * distance * distance + 2 * height * speedSquared);

        if (discriminant < 0)
        {
            Vector3 maxPoint = _pivotHorizontal.position + _pivotHorizontal.forward * _distance;
            return 45f;
        }

        float angle = Mathf.Atan((speedSquared + Mathf.Sqrt(discriminant)) / (gravity * distance));

        return angle * Mathf.Rad2Deg;
    }
}

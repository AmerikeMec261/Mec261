using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret")]
    [SerializeField] private Transform _pivotHorizontal;
    [SerializeField] private Transform _pivotVertical;
    [SerializeField] private float _distance = 20f;

    [Header("Crosshair")]
    [SerializeField] private Transform _mira; //falta estandarización

    [Header("Projectile")]
    [SerializeField] private Transform _spawn;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private GameObject _explosiveBullet;

    [SerializeField] private float Angulo = 95f; //falta estandarización
    [SerializeField] private float VelocidadMouse = 18f; //falta estandarización

    private GameObject _currentProjectile;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _currentProjectile = _BulletPrefab;
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
            _currentProjectile = _BulletPrefab;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentProjectile = _explosiveBullet;
        }
    }

    private void FireProjectile()
    {
        GameObject bullet = Instantiate( _currentProjectile, _spawn.position, _spawn.rotation);

        bullet.GetComponent<IProjectile>()?.Fire();
    }

    private void FollowMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 target = hit.point;

            _mira.position = target + Vector3.up * 0.02f;
            
            Vector3 flatDirection = target - _pivotHorizontal.position;
            flatDirection.y = 0;

            _pivotHorizontal.rotation = Quaternion.LookRotation(-flatDirection);

            Vector3 fullDirection = target - _pivotVertical.position;
            float dis = flatDirection.magnitude; //No usar abreviaciones
            float alt = fullDirection.y; //No usar abreviaciones

            float angle = CalculateHighAngle(dis, alt, VelocidadMouse);

            _pivotVertical.localRotation = Quaternion.Euler(Angulo - angle, 0f, 0f); //Si le pongo mas de 95 grados volteas a ver mas el cielo pero ya no se ve naatural 
            //Utiliza MathF.Clamp para limitar el ángulo a un rango específico, por ejemplo, entre 0 y 90 grados, para evitar que la torreta apunte hacia abajo o hacia arriba de manera poco realista.
        }
    }

    private float CalculateHighAngle(float distance, float height, float speed)
    {
        float gravity = Physics.gravity.magnitude;

        float speedSquared = speed * speed;
        float discriminant = (speedSquared * speedSquared) - gravity * (gravity * distance * distance + 2 * height * speedSquared);

        if (discriminant < 0)
        {
            Vector3 maxPoint = _pivotHorizontal.position + _pivotHorizontal.forward * _distance;
            _mira.position = maxPoint + Vector3.up * 0.02f;

            return 45f;
        }

        float angle = Mathf.Atan((speedSquared + Mathf.Sqrt(discriminant)) /(gravity * distance));

        return angle * Mathf.Rad2Deg;
    }
}
//Trabajo en clase: estandarizar y quitar abrevaciones
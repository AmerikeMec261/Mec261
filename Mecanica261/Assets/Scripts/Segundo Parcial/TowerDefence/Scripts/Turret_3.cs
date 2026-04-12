using UnityEngine;

public class Turret_3 : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject[] _bulletPrefabs;
    private int _currentBulletIndex = 0;

    [Header("Mouse")]
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _reticle;

    [Header("Projectile")]
    [SerializeField] private float _projectileSpeed = 30f;
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private bool _useHighArc = false;

    private Vector3 _targetPoint;
    private bool _hasSolution;

    private void Update()
    {
        UpdateMouseTarget();
        Aim();

        if (Input.GetMouseButtonDown(0))
        {
            FireProjectile();
        }

        // Cambiar de Bullet
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentBulletIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentBulletIndex = 1;
        }

    }

    // MOUSETARGET
    private void UpdateMouseTarget()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundLayer))
        {
            _targetPoint = hit.point;
        }
        else
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float enter))
            {
                _targetPoint = ray.GetPoint(enter);
            }
        }

        if (_reticle != null)
            _reticle.position = _targetPoint;
    }

    private void Aim()
    {
        Vector3 origin = _bulletSpawn.position;

        Vector3 dir = _targetPoint - _yawPivot.position; //recuerda no usar abreviaciones

        Vector3 flatDir = new Vector3(dir.x, 0f, dir.z); //recuerda no usar abreviaciones

        float distance = flatDir.magnitude;

        if (distance < _minDistance)
        {
            return;
        }

        // yaw
        if (flatDir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(flatDir); //recuerda no usar abreviaciones
            _yawPivot.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
        }

        // pitch
        _hasSolution = SolveBallisticAngle(origin, _targetPoint, _projectileSpeed, out float angle);

        if (_hasSolution)
        {
            float angleDeg = angle * Mathf.Rad2Deg;
            _pitchPivot.localEulerAngles = new Vector3(-angleDeg, 0f, 0f);
        }
        else
        {
            _pitchPivot.localEulerAngles = new Vector3(-45f, 0f, 0f);
        }
    }

    // El disparo 
    private void FireProjectile()
    {
        GameObject bullet = Instantiate(_bulletPrefabs[_currentBulletIndex], _bulletSpawn.position, _bulletSpawn.rotation);

        IProjectile proj = bullet.GetComponent<IProjectile>(); //recuerda no usar abreviaciones

        if (proj != null) // puedes cambiar el null check por ? depues del GetComponent
        {
            proj.SetSpeed(_projectileSpeed);
            proj.Fire();
        }
    }

    // Parabólico / Fórumla de la clase del Martess
    private bool SolveBallisticAngle(Vector3 origin, Vector3 target, float speed, out float angle) //recuerda no usar abreviaciones
    {
        float g = Physics.gravity.magnitude;

        Vector3 flat = new Vector3(target.x - origin.x, 0, target.z - origin.z);
        float x = flat.magnitude;

        float deltaY = target.y - origin.y;

        float v2 = speed * speed;
        float v4 = v2 * v2;

        float inside = v4 - g * (g * x * x + 2 * deltaY * v2);

        if (inside < 0f)
        {
            angle = 0f;
            return false;
        }

        float sqrt = Mathf.Sqrt(inside);

        float low = Mathf.Atan((v2 - sqrt) / (g * x));
        float high = Mathf.Atan((v2 + sqrt) / (g * x));

        angle = _useHighArc ? high : low;

        return true;
    }

    //Ejercicio en clase: cambia el método de arriba para no usar abrevaciones o varibales "mágicas", tienes que saber a qué se refiere X, G, etc. y poner nombres que lo reflejen. Por ejemplo, en vez de "x" podrías usar "horizontalDistance", en vez de "g" podrías usar "gravity", etc.
}
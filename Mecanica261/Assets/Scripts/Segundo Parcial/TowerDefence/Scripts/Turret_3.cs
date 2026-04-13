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
        //RayCast es para detectar alguna collision
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundLayer))
        {
            _targetPoint = hit.point;
        }

        //El otro iff porque era por si no encontraba un collider pero ahorita no necestio eso 

        if (_reticle != null)
        {
            _reticle.position = _targetPoint;
        }
    }

    private void Aim()
    {
        Vector3 originPosition = _bulletSpawn.position;

        Vector3 directionToTarget = _targetPoint - _yawPivot.position;

        Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

        float horizontalDistance = horizontalDirection.magnitude;

        if (horizontalDistance < _minDistance)
        {
            return;
        }

        // yaw
        if (horizontalDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
            _yawPivot.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }

        // pitch
        _hasSolution = SolveBallisticAngle(originPosition, _targetPoint, _projectileSpeed, out float launchAngle);

        if (_hasSolution)
        {
            float launchAngleDegrees = launchAngle * Mathf.Rad2Deg;
            _pitchPivot.localEulerAngles = new Vector3(-launchAngleDegrees, 0f, 0f);
        }
        else
        {
            _pitchPivot.localEulerAngles = new Vector3(-45f, 0f, 0f);
        }
    }

    // Disparo
    private void FireProjectile()
    {
        GameObject bulletInstance = Instantiate(
            _bulletPrefabs[_currentBulletIndex],
            _bulletSpawn.position,
            _bulletSpawn.rotation
        );

        IProjectile projectile = bulletInstance.GetComponent<IProjectile>();

        if (projectile != null)
        {
            projectile.SetSpeed(_projectileSpeed);
            projectile.Fire();
        }
    }

    // Parabólico / Fórumla de la clase del Martess
    private bool SolveBallisticAngle(Vector3 originPosition, Vector3 targetPosition, float projectileSpeed, out float launchAngle)
    {
        float gravity = Physics.gravity.magnitude;

        Vector3 horizontalVector = new Vector3(
            targetPosition.x - originPosition.x,
            0f,
            targetPosition.z - originPosition.z
        );

        float horizontalDistance = horizontalVector.magnitude;

        float heightDifference = targetPosition.y - originPosition.y;

        float speedSquared = projectileSpeed * projectileSpeed;
        float speedFourth = speedSquared * speedSquared;

        float discriminant = speedFourth - gravity * (gravity * horizontalDistance * horizontalDistance + 2 * heightDifference * speedSquared);

        if (discriminant < 0f)
        {
            launchAngle = 0f;
            return false;
        }

        float squareRoot = Mathf.Sqrt(discriminant);

        float lowAngle = Mathf.Atan((speedSquared - squareRoot) / (gravity * horizontalDistance));
        float highAngle = Mathf.Atan((speedSquared + squareRoot) / (gravity * horizontalDistance));

        launchAngle = _useHighArc ? highAngle : lowAngle;

        return true;
    }
}

//Ejercicio en clase:
//Cambia el método de arriba para no usar abrevaciones o varibales "mágicas", tienes que saber a qué se refiere X, G, etc. y poner nombres que lo reflejen.
//Por ejemplo, en vez de "x" podrías usar "horizontalDistance", en vez de "g" podrías usar "gravity", etc.
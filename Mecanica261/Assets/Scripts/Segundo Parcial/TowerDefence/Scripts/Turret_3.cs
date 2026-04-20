using UnityEngine;

public class Turret_3 : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject[] _bulletPrefabs;
    private int _currentBulletIndex = 0;

    [Header("BulletsSpawn")]
    [SerializeField] private Transform[] _bulletSpawns;
    private int _currentSpawnIndex = 0;

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentBulletIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentBulletIndex = 1;
        }
    }

    //NEW
    private Transform GetCurrentSpawn()
    {
        if (_bulletSpawns != null && _bulletSpawns.Length > 0) //verifica si el bulletSpawns tiene elementos
        {
            Transform spawn = _bulletSpawns[_currentSpawnIndex]; //usa el primer bulletSpawn (0)

            if (spawn != null) //verifica si el spawnactual no existe
            {
                return spawn;
            }
        }

        return _bulletSpawn; // si no hay errores, se ejecuta el spawn por defecto
    }


    private void UpdateMouseTarget()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundLayer))
        {
            _targetPoint = hit.point;
        }

        if (_reticle != null)
        {
            _reticle.position = _targetPoint;
        }
    }

    private void Aim()
    {
        Vector3 originPosition = GetCurrentSpawn().position; //ahora agarra el current spawn en vez del spawn por defecto

        Vector3 directionToTarget = _targetPoint - _yawPivot.position;
        Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

        float horizontalDistance = horizontalDirection.magnitude;

        if (horizontalDistance < _minDistance)
        {
            return;
        }

        if (horizontalDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
            _yawPivot.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }

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

    private void FireProjectile()
    {
        Transform currentSpawn = GetCurrentSpawn(); //obtiene el spawn actual

        GameObject bulletInstance = Instantiate(
            _bulletPrefabs[_currentBulletIndex],
            currentSpawn.position,
            currentSpawn.rotation
        );

        IProjectile projectile = bulletInstance.GetComponent<IProjectile>();

        if (projectile != null)
        {
            projectile.SetSpeed(_projectileSpeed);
            projectile.Fire();
        }

        _currentSpawnIndex++; //aumenta el indice del currentspawn para la próxima vez que se dispare

        if (_currentSpawnIndex >= _bulletSpawns.Length) //Si la bala actual es la última, se reinicia el indice para que sea un ciclo
        {
            _currentSpawnIndex = 0;
        }
    }

  

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
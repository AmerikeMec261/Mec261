using UnityEngine;
using NaughtyAttributes;

public class FinalTurret : MonoBehaviour
{

    //Alcance Máximo de la torret es de 18 - 24 km/h
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField, Required] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject[] _bulletPrefabs;
    private int _currentBulletIndex = 0;

    [Header("BulletsSpawn")]
    [SerializeField] private Transform[] _bulletSpawns;
    private int _currentSpawnIndex = 0;

    [Header("Targeting")]
    [SerializeField] private float _detectionRadius = 50f;
   

    [Header("Projectile")]
    [SerializeField] private float _projectileSpeed = 30f;
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private float _maxDistance = 180f;
    [SerializeField] private bool _useHighArc = false;

    private Transform _currentTarget;
    private bool _hasSolution;

    private void Update()
    {
        FindTarget();
        Aim();

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            FireProjectile();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _currentBulletIndex = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            _currentBulletIndex = 1;
    }


    //De UpdateMouseTarget a Find Target.
    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); 

        float _closestDistance = Mathf.Infinity; 
        Transform closest = null; 

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position); 
             
            if (distance < _closestDistance && distance <= _detectionRadius) //dist max = detection radius detectasr
                // velocidad bala 
            {
                _closestDistance = distance; 
                closest = enemy.transform;
            }
        }

        _currentTarget = closest; 

        if (closest != null)
        {
            _hasSolution = true;
        }
    }


    private Transform GetCurrentSpawn()
    {
        if (_bulletSpawns != null && _bulletSpawns.Length > 0) //verifica si el bulletSpawns tiene elementos
        {
           if (_bulletSpawns[_currentSpawnIndex] != null) //verifica que el spawn actual no sea nulo
            {
                return _bulletSpawns[_currentSpawnIndex]; //retorna el spawn actual
            }
        }

        return _bulletSpawn; // en caso de no tener bulletSpawns, se usa el spawn por defecto
    }

    private void Aim()
    {
        if (_currentTarget == null) //Encuentra el target más cercano
        return;

        Vector3 originPosition = GetCurrentSpawn().position;
        Vector3 targetPosition = _currentTarget.position; //agarramos la posición del target actual

        Vector3 directionToTarget = targetPosition - _yawPivot.position;
        Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

        float horizontalDistance = horizontalDirection.magnitude;

        if (horizontalDistance < _minDistance || horizontalDistance > _maxDistance) //verifica que el target esté dentro del rango mínimo y máximo
        {
            _minDistance = horizontalDistance;
            // Si el target está fuera del rango, no se apunta ni se dispara
            _maxDistance = horizontalDistance;
            // Aquí podrías agregar lógica para manejar esta situación, como dejar de apuntar o disparar
          
        }
        

        // YAW
        if (horizontalDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
            _yawPivot.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }

        // PITCH 
        _hasSolution = SolveBallisticAngle(originPosition, targetPosition, _projectileSpeed, out float launchAngle);

        if (_hasSolution)
        {
            float launchAngleDegrees = launchAngle * Mathf.Rad2Deg; // Convierte el ángulo de lanzamiento a grados
            _pitchPivot.localEulerAngles = new Vector3(-launchAngleDegrees, 0f, 0f);
        }

        else
        {
            _pitchPivot.localEulerAngles = new Vector3(-45f, 0f, 0f); // Si no hay solución, se apunta a un ángulo fijo (por ejemplo, -45 grados)
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


        projectile.SetSpeed(_projectileSpeed);
        projectile.Fire();

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
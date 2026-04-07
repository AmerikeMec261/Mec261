using UnityEngine;

public class TrackingTurret : MonoBehaviour //Se utilizo como base el codigo de turret que se utilizo para el examen quitando lo inecesario como la reticle y el apuntar con el mouse 
{
    [Header("Dependencies")]
    [SerializeField] private Transform _enemyTarget;
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Target Settings")]
    [SerializeField] private float _maxDistance = 90f;

    private Vector3 _targetPoint;
    private Vector3 _velocity;
    private bool _solution;

    public void FireProjectile()
    {
        if (_bulletPrefab == null || _bulletSpawn == null || !_solution)
        {
            return;
        }

        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.transform.forward = _velocity.normalized;
        currentBullet.GetComponent<ITrackingProjectile>()?.Fire();
    }

    private void Update()
    {
        UpdateTargetPoint(); //Actualiuza ek frame de donde s eesta posicionando el enemigo
        CalculatedShot(); //Calcula la trayectoria gracias al codigo del Examen/Proyecto hacia donde esta el objetivo

        //Ejecuta el tracking horizontal y vertical
        RotateYaw(GetYawInput());
        RotatePitch(GetPitchInput());

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void UpdateTargetPoint()
    {
        if (_enemyTarget != null) //Verifica si existe un enemigo asignado
        {
            _targetPoint = _enemyTarget.position; //Guarda la posicion del objetivo para convertirle en el punto que seguira la torreta
        }
        else //Si no hay enemigo crea un punto ficticio hacia adelante
        {
            _targetPoint = _bulletSpawn.position + (_bulletSpawn.forward * _maxDistance);
        }
    }

    private void CalculatedShot()
    {
        if (_bulletPrefab == null)
        {
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            return;
        }

        TrackingSimpleBullet projectile = _bulletPrefab.GetComponent<TrackingSimpleBullet>();

        if (projectile == null)
        {
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            return;
        }

        float speed = projectile.Speed();
        Vector3 direction = _targetPoint - _bulletSpawn.position;
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

        float x = directionXZ.magnitude;
        float y = direction.y;
        float g = Mathf.Abs(Physics.gravity.y);
        float speedSquared = speed * speed;

        if (x < 0.1f)
        {
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            return;
        }

        float root = (speedSquared * speedSquared) - g * ((g * x * x) + (2f * y * speedSquared));

        if (root < 0f)
        {
            _solution = false;
            _targetPoint = _bulletSpawn.position + directionXZ.normalized * _maxDistance;
            return;
        }

        float hAngle = Mathf.Atan((speedSquared + Mathf.Sqrt(root)) / (g * x));
        Vector3 fDirection = directionXZ.normalized * Mathf.Cos(hAngle) + Vector3.up * Mathf.Sin(hAngle);

        _velocity = fDirection * speed;
        _solution = true;
    }

    private float GetYawInput()
    {
        //Calcula la direccion desde la base de la torreta hacia el objetivo
        //Nota:No pongo la demas explicacion porque mucho de esto ya esta en el codigo de turret explicado
        Vector3 yawDirection = _targetPoint - _yawPivot.position;
        yawDirection.y = 0f;

        if (yawDirection == Vector3.zero)
        {
            return 0f;
        }
        //Aqui calcula el angulo desde donde mira la base  hacia donde se ubica el objetivo
        float angle = Vector3.SignedAngle(_yawPivot.forward, yawDirection, Vector3.up);

        if (angle > 1f) 
        {
            return 1f;
        }

        if (angle < -1f)
        {
            return -1f;
        }

        return 0f;
    }

    private float GetPitchInput()
    {
        //Si existe una solucion se usa la direccion calculada y si no, se usa la direccion directa al objetivo tambien calcula el angulo entre hacia donde esta mirando actualmente y hacia donde deberia inclinarse
        Vector3 targetDirection = _solution ? _velocity.normalized : (_targetPoint - _pitchPivot.position).normalized;
        float angle = Vector3.SignedAngle(_pitchPivot.forward, targetDirection, _pitchPivot.right);

        if (angle > 1f)
        {
            return 1f;
        }

        if (angle < -1f)
        {
            return -1f;
        }

        return 0f;
    }

    private void RotateYaw(float input)
    {
        float yawChange = input * _yawSpeed * Time.deltaTime;
        _yawPivot.Rotate(0f, yawChange, 0f, Space.Self);
    }

    private void RotatePitch(float input)
    {
        //Calcula donde debe subir o bajar el caon segun el frma tambien lee el angulo para trabajar con un valor real
        //POr ultimo calcula el nuevo angulo y lo limita para que pase de los limites que estan permitidos y que aplique la nueva inclinacion al pivote vertical del caon
        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float currentPitch = NormalizeAngle(_pitchPivot.localEulerAngles.x);
        float newPitch = Mathf.Clamp(currentPitch - pitchChange, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3(
            newPitch,
            _pitchPivot.localEulerAngles.y,
            _pitchPivot.localEulerAngles.z
        );
    }

    private float NormalizeAngle(float angle) //Este fue con el proposito para que pitch se lea de manera correcta y que los limites como -10 y 90 funcionene bien teniendo una mejor comparacion
    {
        if (angle > 180f) //Aqui se verifica si el angulo que da unity es mayor a los 180 grados
        {
            angle -= 360f; //Si es mayor este le restara 360 grados convirtiendolo en numero negativos devolviendo un angulo normalizafo para que sea mas facil trabajar con ello
        }

        return angle;
    }
}

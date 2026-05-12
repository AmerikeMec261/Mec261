using UnityEngine;

public class TurretShip : MonoBehaviour //Se combino los scripts TurretBase y TrackingTurret previamente creados
                                        //Correcion de examen en PitchInput y calculo para angulos bajos
                                        //Referencias para input :
                                        //https://discussions.unity.com/t/problem-with-rotation-and-vector3-project-projectonplane/333078
                                        //https://catfortsoftware.com/tutorial-aiming-a-turret/
                                        //https://docs.unity3d.com/6000.4/Documentation/Manual/class-Quaternion.html
{
    [Header("Dependencies")]
    [SerializeField] private Transform _enemyTarget;
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn1;
    [SerializeField] private Transform _bulletSpawn2;
    [SerializeField] private Transform _bulletSpawn3;
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-90f, 40f);

    [Header("Target Settings")]
    [SerializeField] private float _maxDistance = 180f;

    private Vector3 _targetPoint;
    private Vector3 _velocity;
    private bool _solution;

    public void FireProjectile()
    {
        if (_bulletPrefab == null || !_solution) //<- Posible uso de IA. El objeto es asignado desde el editor. 
        {
            return;
        }

        GameObject currentBullet1 = Instantiate(_bulletPrefab, _bulletSpawn1.position, _bulletSpawn1.rotation);
        currentBullet1.transform.forward = _velocity.normalized;
        currentBullet1.GetComponent<IProjectile>()?.Fire();

        GameObject currentBullet2 = Instantiate(_bulletPrefab, _bulletSpawn2.position, _bulletSpawn2.rotation);
        currentBullet2.transform.forward = _velocity.normalized;
        currentBullet2.GetComponent<IProjectile>()?.Fire();

        GameObject currentBullet3 = Instantiate(_bulletPrefab, _bulletSpawn3.position, _bulletSpawn3.rotation);
        currentBullet3.transform.forward = _velocity.normalized;
        currentBullet3.GetComponent<IProjectile>()?.Fire();
    }

    private void Update()
    {
        UpdateTargetPoint(); 
        CalculatedShot(); 

        
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
            _targetPoint = _bulletSpawn1.position + (_bulletSpawn1.forward * _maxDistance);
        }
    }

    private void CalculatedShot()
    {
        if (_bulletPrefab == null) //<- Posible uso de IA. El objeto es asignado desde el editor. 
        {
            _solution = false;
            return;
        }

        IProjectile projectile = _bulletPrefab.GetComponent<IProjectile>(); //<- Posible uso de IA. Ya tienes la linea de  currentBullet3.GetComponent<IProjectile>()?.Fire(); como ejemplo para checar nulls.
        if (projectile == null) //<- Posible uso de IA. Se supone que el proyectil tiene que implementar la interfaz IProjectile. 
        {
            _solution = false;
            return;
        }

        float speed = projectile.Speed();
        Vector3 direction = _targetPoint - _bulletSpawn1.position;
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

        float x = directionXZ.magnitude;
        float y = direction.y;
        float g = Mathf.Abs(Physics.gravity.y);
        float speedSquared = speed * speed;

        if (x < 0.1f)
        {
            _solution = false;
            return;
        }

        float root = (speedSquared * speedSquared) - g * ((g * x * x) + (2f * y * speedSquared));

        if (root < 0f)
        {
            _solution = false;
            return;
        }

        float LowAngle = Mathf.Atan((speedSquared - Mathf.Sqrt(root)) / (g * x));
        float HighAngle = Mathf.Atan((speedSquared + Mathf.Sqrt(root)) / (g * x));

        float hAngle = LowAngle;

        if (y < 2f)
        {
            hAngle = HighAngle;
        }
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

        Vector3 pitchDirection = _targetPoint - _pitchPivot.position;
        

        if (pitchDirection == Vector3.zero)
        {
            return 0f;
        }
        //Aqui calcula el angulo desde donde mira la base  hacia donde se ubica el objetivo
        float angle = -Mathf.Atan2(pitchDirection.y, pitchDirection.z) * Mathf.Rad2Deg;
        float currentPitch = NormalizeAngle(_pitchPivot.localEulerAngles.x);
        float Vangle = angle - currentPitch;  

        if (Vangle > 1f)
        {
            return 1f;
        }

        if (Vangle < -1f)
        {
            return -1f;
        }

        return 0f;
    }

    private void RotateYaw(float input)
    {
        float yawChange = input * _yawSpeed * Time.deltaTime;
        float currentYaw = NormalizeAngle(_yawPivot.localEulerAngles.y);
        float newYaw = Mathf.Clamp(currentYaw + yawChange, _yawLimits.x, _yawLimits.y);

        _yawPivot.localEulerAngles = new Vector3( //<-- Uso 100% confirmado de IA, el saltar lineas para escribir el nuevo vector3 es totalmente por IA
            _yawPivot.localEulerAngles.x,
            newYaw,
            _yawPivot.localEulerAngles.z
        );
    }

    private void RotatePitch(float input)
    {
        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float currentPitch = NormalizeAngle(_pitchPivot.localEulerAngles.x);
        float newPitch = Mathf.Clamp(currentPitch + pitchChange, _pitchLimits.x, _pitchLimits.y);

        _pitchPivot.localEulerAngles = new Vector3( //<-- Uso 100% confirmado de IA, el saltar lineas para escribir el nuevo vector3 es totalmente por IA
            newPitch,
            _pitchPivot.localEulerAngles.y,
            _pitchPivot.localEulerAngles.z
        );
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }
}



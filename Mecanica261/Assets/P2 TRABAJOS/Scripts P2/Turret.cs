using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _reticle;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Disparo Settings")]
    [SerializeField] private float _maxDistance = 90f;

   
    private Vector3 _mousePoint;
    private Vector3 _targetPoint;
    private Vector3 _Velocity;
    private bool _solution;

    
    public void FireProjectile()
    {
        
        if (_bulletPrefab == null || !_solution)
        {  
            return; 
        }
        
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.transform.forward = _Velocity.normalized;
        currentBullet.GetComponent<IProjectile>()?.Fire();
       

    }

    private void Update()
    {
        UpdateMousePoint(); 
        CalculatedShot(); 

        RotateYaw(GetYawInput()); 
        RotatePitch(GetPitchInput());

      
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            FireProjectile();
        }
    }

    private void UpdateMousePoint()
    {
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;


    
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
        {
            _mousePoint = hit.point; 
        }
        else
        {
            _mousePoint = _bulletSpawn.position + (_bulletSpawn.forward * _maxDistance); 
        }

    }

    private void CalculatedShot()
    {
        if (_bulletPrefab == null)
        {
            
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            MoveReticle();
            return; 
        }

        IProjectile projectile = _bulletPrefab.GetComponent<IProjectile>();

        if (projectile == null) 
        {
            
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            MoveReticle();
            return;
        }

        float speed = projectile.Speed(); 
        Vector3 direction = _mousePoint - _bulletSpawn.position; 
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);

       
        float x = directionXZ.magnitude;
        float y = direction.y; 
        float g = Mathf.Abs(Physics.gravity.y); 
        float speedFOR2 = speed * speed; 

        if (x < 0.1f) 
        {
            
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            MoveReticle();
            return;
        }

       
     
        float root = (speedFOR2 * speedFOR2) - g * ((g * x * x) + (2f * y * speedFOR2));

        if (root < 0f) 
        {
            
            _solution = false;
            _targetPoint = _bulletSpawn.position + directionXZ.normalized * _maxDistance; 
            MoveReticle();
            return;
        }
        
        float HAngle = Mathf.Atan((speedFOR2 + Mathf.Sqrt(root)) / (g * x));
        Vector3 FDirection = directionXZ.normalized * Mathf.Cos(HAngle) + Vector3.up * Mathf.Sin(HAngle); 

        _Velocity = FDirection * speed; 
        _targetPoint = _mousePoint;
        _solution = true; 

        MoveReticle(); 
    }

    private void MoveReticle()
    {
        if (_reticle != null) 
        {
            _reticle.position = _targetPoint; 
        }
    }
    private float GetYawInput()
    {
        
        Vector3 yawDirection = _targetPoint - _yawPivot.position;
        yawDirection.y = 0f;

        if (yawDirection == Vector3.zero) 
        {
            return 0f;
        }

        
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
       
        Vector3 targetDirection = _solution ? _Velocity.normalized : (_targetPoint - _pitchPivot.position).normalized;
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
        
        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float newPitch = Mathf.Clamp(_pitchPivot.localEulerAngles.z + pitchChange, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, newPitch);
    }
}// trabajo en clase: eliminar por completo los comentarios. El codigo debe ser "auto comentado" es decir que el codigo se entienda por si solo sin necesidad de los comentarios.

//Investigacion de metodos y funciones

//Instantiate = Crea una copia ak ejecutar de un objeto GameObject.
//.notmalized =Devuelve el vector con la misma direccion pero la longitud lo vuelve a util para direcciones
//GetComponent<IProjectile> = Busca el componente si implementa la interfaz en este caso
//? = es una pequeña condicion donde si cumple se ejecutara la accion y si no solo devuelve null 
// Ray ray = _camera.ScreenPointToRay(Input.mousePosition); = Este convierte la posicion del mouse que es 2D a un rayo en 3D que sale desde la camara. 
//RaycastHit hit;= Guarda la variable de donde choco el rayo.
// (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground") = lanza el rayo en la escenca  este verifica si toca el layer ground con una distancia maxima de 1000 unidades para que este solo apunte a esa capa.
// .magnitude = devuelve la longitud del vector
//Mathf.Abs = devuelve el valor absoluto de un numero sin signo negativo
//(Physics.gravity.y = la gravedad vertical que tiene por default unity que es negativa y con ayuda de Mathf.Abs se conmvierte en positivo
//Mathf.Atan = devuelve el arcotangente de un valor
//Mathf.Atan2 = Calcula el angulo usando dos valores X y Y
//Mathf.Sqrt = La raiz cuadrada de un numero
//Mathf.Cos = Es el coseno de un angulo
//Mathf.Sin = Es el seno de un angulo
//Vector3.SignedAngle = Este te dice cuanto agulo falta y hacia que lado girar
//Mathf.Clamp = Limita un valor entre el minimo hasta un maximo
//.Rotate = Accedemos a modificar la rotacion de un transform
//Vector3.up= un vector que apunta hacia arriba (0,1,0)
using System.Timers;
using TMPro;
using UnityEngine;


public class Tower2 : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;


    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);


    [Header("Bullet")]
    [SerializeField] private GameObject _explosivePrefab;
    private bool _usingExplosive = false;
    private float _currentYaw = 0f;
    private float _currentPitch = 0f;
    private bool _useHighArc; 

    [Header("Turret")]
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _turret;

    [SerializeField] private Transform _cannonPivot;
     


    public void FireProjectile()
    {
        GameObject prefabToSpawn = _usingExplosive ? _explosivePrefab : _bulletPrefab;
        if (prefabToSpawn == null)
        {
            Debug.Log("Prefab No Agsinado");
            return;
        }
        GameObject currentBullet = Instantiate(prefabToSpawn, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.GetComponent<IProjectile>()?.Fire();

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red, 2f);
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

   /*private void Rotation()
    {
        Vector3 direction = _target.position - _turret.position;
        direction.y = 0f;

        float angle = Mathf.Atan2(  direction.x, direction.z) * Mathf.Rad2Deg;

        _yawPivot.localEulerAngles = new Vector3(0f, angle - 90f , 0f);
    }*/
    private void Update()
    {
        //RotateMouse();
        SolveBallisticAngle();


        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _usingExplosive = !_usingExplosive;
            Debug.Log(_usingExplosive ? "Bala Explosiva" : "Bala Normal");
        }
    }


    // Diferencia entre Euler y Quaternion ? (Investigación de IA)


    //Ambos son métodos para manejar la rotación de objetos en Unity

    //El Quaternion es un metodo con calculos tan complejos que solo entienden las computadoras , tiene 4 vaalores ( X , Y , Z , W ) , su principal ventaja es que 
    // no sufren de bloqueos y sirven para físicas avanzadas.

    //El Euler solo funciona en 3 ángulos ( X , Y , Z ) , es más fácil de leer ( para los humanos ) , su desventaja era que tiene "Gimbal Lock" que es que cuando rotas 
    // un ángulo puedes hacer que los otros dos se alíneen con el que rotaste .
    // Yo había usado ambas para la torreta ( la IA me los dió así para rotar el mouse .
    // El Math.Clamp sirve para mantener a los números a ver algo y que no se salgan de su rango máximo o minimo.

    //El Math.Clamp lo usé oara que la torreta ( el yaw y el pitch ) no apuntara así hacía abajo o se volteara el cañon , la mantenía de un comportamiento normal .
    //Quaternion lo usé porque con ciertos ángulos mi torreeta digamos que iba a la inversa , para la base ( el yaw )
    //Euler para poder mover el cañon en unity y poder estar modificando ángulos de manera local 

    //https://docs.unity3d.com/es/current/ScriptReference/Quaternion.html Quaternion Librebria
    //https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Quaternion.Euler.html Euler Libreria
    //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.Clamp.html Math Clamp

    /* private bool SolveBallisticAngle(Vector3 originPosition, Vector3 targetPosition, float projectileSpeed, out float launchAngle)
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

     if (discriminant< 0f)
     {
         launchAngle = 0f;
         return false;
     }

float squareRoot = Mathf.Sqrt(discriminant);

float lowAngle = Mathf.Atan((speedSquared - squareRoot) / (gravity * horizontalDistance));
float highAngle = Mathf.Atan((speedSquared + squareRoot) / (gravity * horizontalDistance));

launchAngle = _useHighArc ? highAngle : lowAngle;

return true;
 }*/
    //Formula corregida 



}
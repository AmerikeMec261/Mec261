using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _cannonPivot;
    [SerializeField] private Transform _shipReferenceTransform;

    [SerializeField] private float _yawLimit = 145f;
    [SerializeField] private float _projectileSpeed = 250f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);

    private float _startingYaw;

    // Usamos las variables con serializefield para hacerlas privadas y usarlas dentro de unity 

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);
    }
    // Ponemos la formula para el yaw y que este nos de los angulos para la torreta 
    private void Update()
    {
        RotateTurretBase();
        ElevateCannon();
    }

    private void RotateTurretBase()
    {
        if (_targetTransform == null)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;
        directionToTarget.y = 0f;

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget); //hace que la rotacion de la torreta gire de coordenadas globales a locales

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);
    }

    //Aqui es la formula para la torreta para darle los angulos y que esta gire junto con el math.clamp para limitar el valor flotante minimo y maximo 

    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.Clamp.html


    private void ElevateCannon()
    {
        if (_targetTransform == null)
        {
            _cannonPivot.localRotation = Quaternion.identity;
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f);
    }

    // Aqui ponemos la formula para que el cañon suba de formal vertical la parte del cañon igualmente con el math.clamp.

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position;

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude;
        float verticalDistanceToTarget = directionFromCannonToTarget.y;
        float gravityStrength = Mathf.Abs(Physics.gravity.y);
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed;

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);

        if (formulaValueInsideSquareRoot < 0f)
        {
            cannonPitchAngle = _pitchLimits.y;
            return false;
        }
        
        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;

        return true;
    }
}  // Creo aqui tratamos de identificar el target del cañon junto con la gravedad y la celocidad del proyectil, el math.atan devielve el angulo en grados para que no tenga problema el codigo el math. sqrt devuelve la raiz cuadrada del numero real
   //https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.Atan.html
   //https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Mathf.Sqrt.html

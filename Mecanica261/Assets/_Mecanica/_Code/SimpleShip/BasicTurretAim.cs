using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform; //variable  la cual hace que asignemos al enemigo y nuestra torreta lo detecte 
    [SerializeField] private Transform _cannonPivot;//variable de donde sale la bala 
    [SerializeField] private Transform _shipReferenceTransform;//variable que hace referencia a nuestro barco 

    [SerializeField] private float _yawLimit = 145f;//el limite del yaw 
    [SerializeField] private float _projectileSpeed = 250f;//la velocidad en la cual va a salir nuestro proyectil 
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);//el limite del pitch 

    private float _startingYaw;//variable la cual va a hacer que nuestra torreta inice en una poscion neutra 

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);//formula la cual hace que nuesta torreta inicie en posicion neutra 
    }

    private void Update()
    {
        RotateTurretBase();//llamamos al metodo que hace que nuestra torreta gire 
        ElevateCannon();//llamamos al metodo que hace que nuestra torrta se eleve 
    }

    private void RotateTurretBase()
    {
        if (_targetTransform == null)//en este if hacemos que si en el targettransform no hay nada entonces la torreta regrese a su posicion original 
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;//aqui hacemos que la direccion del target sea la resta de su posicion menos el transform 
        directionToTarget.y = 0f;//

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);//hace que cambie de valores globales a locales y que la torreta pueda girar de mejor manera

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;//hacemos que la torreta voltee a ver al enemigo con esta formula 
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);//hacemos que la torreta tenga limite de giro 

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);//hacemos que todos los valores anteriores se guarden en esta variable para que la torreta gire
    }

    private void ElevateCannon()
    {
        if (_targetTransform == null)//aqui es similar al que la torreta gire solo que aqui es para que el cañon se eleve 
        {
            _cannonPivot.localRotation = Quaternion.identity;
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);//hacemos que el cañon tenga limite hacia donde puede alzarse y hasta donde baja

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f);//hacemos que se guarden los valores y este pueda elevarse de forma correcta 
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position;//aqui hacemos que el cañon sepa donde esta el enemigo mediante esa resta  

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude;//aqui hacemos que vea donde esta el target de manera horizontal
        float verticalDistanceToTarget = directionFromCannonToTarget.y;
        float gravityStrength = Mathf.Abs(Physics.gravity.y);//aqui aplicamos que aplique la gravedad 
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed;//aqui hacemos que la velocidad del proyectil se multiplique al cuadrado

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);

        if (formulaValueInsideSquareRoot < 0f)
        {
            cannonPitchAngle = _pitchLimits.y;
            return false;
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;//

        return true;
    }
}
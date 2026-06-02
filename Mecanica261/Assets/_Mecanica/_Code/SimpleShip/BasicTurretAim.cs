using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{

    //Marcamos todas las variables que utilizaremos en nuestro script de la torreta


    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _cannonPivot;
    [SerializeField] private Transform _shipReferenceTransform;

    [SerializeField] private float _yawLimit = 145f;
    [SerializeField] private float _projectileSpeed = 250f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);    

    private float _startingYaw;         

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);       //Aqui modificamos el angulo para el movimiento de la base en un angulo horizontal
    }

    private void Update()
    {
        RotateTurretBase();        //Aqui solo tenemos las funciones del cañon y del movimiento de la base que es literalmente todo lo que hace que funcionen nuestras torretas.
        ElevateCannon();
    }

    private void RotateTurretBase()    
        
    {
        if (_targetTransform == null)    //Aqui hacemos que la torreta empiece en su posicion inicial.
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;   //Ajustamos la posicion del objetivo restando la posicion del objetivo con la de nuestra torreta.
        directionToTarget.y = 0f;

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg; // estas formulas son para obtener el angulo de nuestra base.
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);    // obtenemos el angulo desde el punto inicial. 
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);   //

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);  //aca aplicamos la rotacion para que se mueva la base de la torreta
    }

    private void ElevateCannon()    
    {
        if (_targetTransform == null)             //Ponemos en la posicion inicial los cañones.
        {
            _cannonPivot.localRotation = Quaternion.identity;
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }          

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);      //Ajustamos los limites del cañon

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f);            //Aplicamos la rotacion de los cañones.
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)         //Calculamos el angulo de nuestro cañon hacia el objetivo.
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
}
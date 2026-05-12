using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;  //asigan el tranform del tageta al que va a seguir 
    [SerializeField] private Transform _cannonPivot;  //
    [SerializeField] private Transform _shipReferenceTransform;  //asigna el tranform del barco

    [SerializeField] private float _yawLimit = 145f;  //es el limite de apertura de la torreta
    [SerializeField] private float _projectileSpeed = 250f;  //la velocidad a la que va el proyectil
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);  //el limite se sube y baja del cañon 

    private float _startingYaw;

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);
    }

    private void Update()
    {
        RotateTurretBase();  //lama la funcion de la base de la torreta 
        ElevateCannon();  //llama la funcion para elevar el cañon 
    }

    private void RotateTurretBase()
    {
        if (_targetTransform == null)  //si el enemy esta en rango o no 
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);  //regresa la torreta a su posicion inicial 
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;  //esta liena hace que la torreta apunte 
        directionToTarget.y = 0f;

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);  //hace que la torrte gire aunque el barco este inclinado 

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);
    }

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
}

//InverseTransformDirection https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform.InverseTransformDirection.html
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

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z); // Guarda la posicion iniciaal de la torretas
    }

    private void Update()
    {
        RotateTurretBase();
        ElevateCannon();
    }

    private void RotateTurretBase() // Aqui gira la torreta de la base pero nadamas de la eje z y enfocando al enemigos
    {
        if (_targetTransform == null)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;
        directionToTarget.y = 0f;

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);//Convierte los coordenadas globales a coordenadas locales 

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);// El inicio de la piosicion de los cañones
    }

    private void ElevateCannon()// Aqui eleva tu cañon con el pivot con los angulos
    {
        if (_targetTransform == null)
        {
            _cannonPivot.localRotation = Quaternion.identity;//Rotacion del cañon
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }// Para calcular los pitch de os algunos del cañon

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);//Calcula los limites de los algunos del cañon con los limites del eje x y y

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f);
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)// Aqui es para calcular los angulos del cañon 
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position;

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude;// Fija al enemigo rn direccion a el
        float verticalDistanceToTarget = directionFromCannonToTarget.y;
        float gravityStrength = Mathf.Abs(Physics.gravity.y);
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed;// Aqui es la velocidad del proyectil

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);
        // Calcula la velocidad del proyectil junto a la gravedad de este y la distancia que va tener el enemigo
        if (formulaValueInsideSquareRoot < 0f)
        {
            cannonPitchAngle = _pitchLimits.y;//Calcula el limite del eje y
            return false;
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;

        return true;
    }
}
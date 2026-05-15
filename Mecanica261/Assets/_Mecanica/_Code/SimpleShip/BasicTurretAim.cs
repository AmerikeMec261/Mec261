using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform; //Este Transform se encarga de encontrar el objetivo al que va a a apuntar la torreta
    [SerializeField] private Transform _cannonPivot; // Este Transform es el que va a marcar el pivote sobre el que va a girar la torreta
    [SerializeField] private Transform _shipReferenceTransform; // Este transform hace que la torreta siempre este detectando la posición del barco 

    [SerializeField] private float _yawLimit = 145f; //El yaw limit es el que marca el limite de elevación que va a tener la torreta
    [SerializeField] private float _projectileSpeed = 250f; // Esta línea es la que determina la velocidad a la que va a salir disparada el proyectil
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f); //Esta línea es la que marca el limite del pitch de la torreta

    private float _startingYaw; // Esta línea indica la inclinacion inicial de la torreta

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z); //Esta linea se encarga de calcular el angulo inicial de la torreta
    }

    private void Update()
    {
        RotateTurretBase();
        ElevateCannon();
    }

    private void RotateTurretBase()
    {
        if (_targetTransform == null)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw); //Por lo que entiendo esta línea se encarga de que la torreta este en una posición neutral si no detecta objetivo
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position; // Esta linea se encarga de que la torreta gire hacía el objetivo
        directionToTarget.y = 0f; // Esta línea se encarga de que la torreta no se mueva en el eje y

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget); //Esta línea hace que el vector local de la posición del objetivo se vuelva un vector global

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg; //Esta línea hace que la torreta tenga la inclinación necesaria para que alcance el objetivo
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle); //Hace la comparación entre el angulo inicial y el angulo actual
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit); //Esta línea se encarga de confirmar que el cambio en el yaw cumpla con el limite

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);
    }

    private void ElevateCannon()
    {
        if (_targetTransform == null)
        {
            _cannonPivot.localRotation = Quaternion.identity; //Esta línea hace que si no hay objetivo detectado la torreta este estatica
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; } //Esta liinea se encarga de calcular el angulo de elevación que debe tener la torreta

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y); // Esta línea se encarga de que el angulo de la torreta respete el límite marcado

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f); //Esta línea se encarga de colocar la torreta en la posición calculada
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position; //Esta línea se encarga de calcular el angulo del pitch conforme a la posición del cañon y la posicion del objetivo

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude; // Esta línea se encarga de calcular la distancia horizontal entre objetivo y torreta
        float verticalDistanceToTarget = directionFromCannonToTarget.y; // Esta línea se encarga de calcular la idtsnacia vertical entre objetivo y torreta
        float gravityStrength = Mathf.Abs(Physics.gravity.y); // Esta linea se encarga de agrgar la gravedad al proyectil
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
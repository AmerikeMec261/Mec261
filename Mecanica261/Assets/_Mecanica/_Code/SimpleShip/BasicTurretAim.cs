using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform; //Objetivo al que apunta la torreta 
    [SerializeField] private Transform _cannonPivot; //Parte del cañon que apunta hacia arriba
    [SerializeField] private Transform _shipReferenceTransform; // Referencia de orientacion del barco

    [SerializeField] private float _yawLimit = 145f; //Limite de giro horizontal
    [SerializeField] private float _projectileSpeed = 250f; // Velocidad del proyectil
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f); // Son los limites de inclinacion vertical

    private float _startingYaw; //Rotacion inicial de la torreta

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z); //Guarda el angulo de inicio de la torreta 
    }

    private void Update()
    {
        RotateTurretBase(); //Gira la base de la torreta
        ElevateCannon(); // Mueve el cañon de arriba de abajo   
    }

    private void RotateTurretBase()
    {
        if (_targetTransform == null) //Si no hay objetivo
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw); // Regresa a la rotación inicial
            return;
            // Documentación:
            // https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html

        }

        Vector3 directionToTarget = _targetTransform.position - transform.position; // Dirección desde la torreta hacia el objetivo
        directionToTarget.y = 0f; // Ignora la altura

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);  // Convierte la dirección al espacio local del barco
        // Documentación:
        // https://docs.unity3d.com/ScriptReference/Transform.InverseTransformDirection.html

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg; // Calcula el ángulo horizontal hacia el objetivo
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle); // Diferencia entre el ángulo inicial y el objetivo
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit); // Limita el giro máximo

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference); // Aplica la rotación horizontal
    }

    private void ElevateCannon()
    {
        if (_targetTransform == null) // Si no hay objetivo
        {
            _cannonPivot.localRotation = Quaternion.identity; // Reinicia la rotación del cañón
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }  // Intenta calcular el ángulo correcto

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y); // Limita el ángulo vertical

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f); // Aplica la inclinación del cañón
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position; // Dirección desde el cañón hasta el objetivo

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude; // Distancia horizontal al objetivo
        float verticalDistanceToTarget = directionFromCannonToTarget.y; // Distancia vertical al objetivo
        float gravityStrength = Mathf.Abs(Physics.gravity.y);  // Aplica fuerza de gravedad
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed;  // Velocidad del proyectil al cuadrado

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);  // Fórmula balística para calcular el disparo

        if (formulaValueInsideSquareRoot < 0f)  // Si el valor es menor a 0 no puede llegar al objetivo
        {
            cannonPitchAngle = _pitchLimits.y; // Usa el ángulo máximo
            return false;
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg; // Calcula el ángulo de inclinación necesario

        return true;

        // Documentación:
        // https://docs.unity3d.com/ScriptReference/Mathf.Sqrt.html
    }
}
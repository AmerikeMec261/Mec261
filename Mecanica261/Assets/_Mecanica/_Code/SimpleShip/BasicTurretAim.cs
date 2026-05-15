using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform; // Transform del objetivo del cañon
    [SerializeField] private Transform _cannonPivot; // Transform del pivote del cañon
    [SerializeField] private Transform _shipReferenceTransform; // Transform del barco

    [SerializeField] private float _yawLimit = 145f; // Limite del yaw de la torreta
    [SerializeField] private float _projectileSpeed = 250f; // Velocidad del priyectil
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f); // limites del pitch del cañon

    private float _startingYaw; // Yaw inicial de la torreta

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z); // Se calcula la diferencia para obtener el yaw inicial con 0 y el transform.localEulerAngles.z 
    }

    private void Update()
    {
        RotateTurretBase(); // Llama la funcion para rotar la base de la torreta
        ElevateCannon(); // Llama la funcion para elevar el cañon
    }

    private void RotateTurretBase() // Funcion para rotar la base de la torreta
    {
        if (_targetTransform == null) // Si el transform del objetivo es nulo, 
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw); // Se calcula la rotacion del transform del gameObject usando Euler Angles con el yaw inicial
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position; // Se resta la posicion del target a la posicion que tenemos para saber en donde esta lo que queremos atacar
        directionToTarget.y = 0f;

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget); // Se obtiene la referencia del barco

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg; // Se usa Mathf.Atan para calcular las radianes de x y z multiplicado por Mathf.Rad2Deg para pasar eso radianes a grados
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle); // Se usa Mathf.DeltaAngle para regresar un valor entre la diferencia del Yaw Inicial y el angulo del Yaw objetivo
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit); // Se hace un Clamp para regresar el valor min y max entre la diferencia del yaw del inicio con los limites del Yaw y obtener asi la diferencia de los limites 

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference); // Se obtiene y calcula la rotacion del gameObject usando Euler agregando el Yaw inicial y la diferencia de los limites del Yaw
    }

    private void ElevateCannon() // Funcion que eleva el cañon
    {
        if (_targetTransform == null) // Si el _targetTransform es nulo
        {
            _cannonPivot.localRotation = Quaternion.identity; // Se obtiene la rotacion del pivote del cañon y se alinea a 0 con Quaternion.Identity
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; } // Si TryCalculateCannonPitchAngle falla regresa false

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y); // Se calcula y suaviza los angulos del cañon con los limites del pitch x y

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f); // Se obtiene la rotacion del pivote del cañon y se calculan los limites del angulo del pitch del cañon
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle) // Un metodo booleano para calcular el angulo de pitch 
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position; // Se resta la posicion del target a la del pivote del cañon para obtener a donde se dirige el cañon al target

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude; // Se obtiene la manitud de la direccion del cañon al target en x con la del z para su distancia horizontal
        float verticalDistanceToTarget = directionFromCannonToTarget.y; // Se obtiene la referencia en vertical
        float gravityStrength = Mathf.Abs(Physics.gravity.y); // Regresa un valor absoluto con Mathf.Abs de la gravedad en y
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed; // Obtiene el cuadrado de la velocidad del proyectil multiplicandola?)


        //  Calcula la formula del proyectil siendo que se adjuntan todos los valores
        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);

        if (formulaValueInsideSquareRoot < 0f) // Si formulaValueInsideSquareRoot es menor a 0
        {
            cannonPitchAngle = _pitchLimits.y; // Regresa los angulos a los limites del pitch
            return false;
        }

        // Cacula la velocidad del disparo para que al momento de ser lanzado, vaya en un trayectoria limpia
        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;

        return true;
    }
}
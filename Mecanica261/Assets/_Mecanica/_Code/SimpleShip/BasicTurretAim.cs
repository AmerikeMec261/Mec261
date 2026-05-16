using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform; // Esta es para asignar el target en el inspector.
    [SerializeField] private Transform _cannonPivot; // Aquí se asigna el pivote del cañon igualmente en el inspector.
    [SerializeField] private Transform _shipReferenceTransform;// Aqui para asignar el asset del bbarco igual en el inspector.

    [SerializeField] private float _yawLimit = 145f; // El límite par la rotación en Y.
    [SerializeField] private float _projectileSpeed = 250f; //Velocidad del proyectil.
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f); //Límites del cañon en X, Y.

    private float _startingYaw;

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z); //Aquí se calcula el angulo en el que empieza .
    }

    private void Update() //Aquí las cosas se ejecutan varias veces o siempre 
    {
        RotateTurretBase();
        ElevateCannon();
    }

    private void RotateTurretBase() //Aquí es la parte en la que se hacen los calculos para que rote .
    {
        if (_targetTransform == null) //Aquí a lo que entiendo es una condición para que te regrese un null en la consola si no hay  Target.
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);
            return; //Aquí es dónde regresa a la torreta a su posición zero , a la base de la torreta-
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;
        directionToTarget.y = 0f; //Aquí ve la direccón del target menos la posición de la torreta y el resultado de esa resta es la distancia que los separa.

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget); // A esta línea no le entendí . ( https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Transform.InverseTransformPoint.html  

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg; // Aqui calcula la distancia entre yaw y el targett.
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle); // Aquí es el calculo del yaw inicial ( su posicion )
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit); // limita el rango de yaw.

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference); // Tampoco entemdí que hace aquí.
    }

    private void ElevateCannon() //Aquí se eleva el calon de la torreta
    {
        if (_targetTransform == null) //Igualmente  te regresa un null si no hay un target.
        {
            _cannonPivot.localRotation = Quaternion.identity;
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; } // Aquí a lo que entiedp se calcula algo de el cañón y lo regresa pero no entiendo que.

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y); // Aquí son los límites de los ángulos de X , Y del Pitch.

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f); //Es una rotación límitada del Pitch pero no la comprendo bien nuevamente.
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle) // Un bool que intenta calcular creo que la elevación .
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position; // Nuevamente es el calculo de la posición del enemigo menos la posición del cañon.

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude; //Distancia horizontal del cañon al enemigo.
        float verticalDistanceToTarget = directionFromCannonToTarget.y; //Distancia al enemigo en Y.
        float gravityStrength = Mathf.Abs(Physics.gravity.y); // Gravedad y fuerza.
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed; //Velocidad del proyectil multiplicada.

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared); // Una operación para calcular la fuerza del proyectil y la velocidad usando la física y usa el tiro parabolico.

        if (formulaValueInsideSquareRoot < 0f) //SI la formula anterior da o es menir a 0 es igual a false .
        {
            cannonPitchAngle = _pitchLimits.y;
            return false;
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg; //Formula nuevamente para tiro párabolico.

        return true; // Si la formula amterior es correcta o el calculo devuelve un true.
    }
}
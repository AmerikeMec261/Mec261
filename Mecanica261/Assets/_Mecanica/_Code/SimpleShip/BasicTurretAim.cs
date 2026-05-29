using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform; //el transform es para indicar la posicion, rotacion y escala de un objeto en unity
    [SerializeField] private Transform _cannonPivot;
    [SerializeField] private Transform _shipReferenceTransform;

    [SerializeField] private float _yawLimit = 145f; // yaw es el giro horizontal
    [SerializeField] private float _projectileSpeed = 250f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f); //inclinacion vertical el pitch

    private float _startingYaw;

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z); //antes del juego normal unity llama a awake para guardar el angulo inicial de la torreta
        // los euler angles son rotaciones en x y y z
    }

    private void Update()
    {
        RotateTurretBase();// se llama a update para que cada frame gire la base de la torreta y eleve el canon
        ElevateCannon();
    }

    private void RotateTurretBase()
    {
        if (_targetTransform == null)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw); //aqui si no hay objetivo  la torreta vuelve a su rotacion inicial, el Quaternion sirve para crear una rotacion
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;
        directionToTarget.y = 0f; //aqui se calcula la direccion desde la torreta hacia el objetivo, pone la direccion global del objeto, al restar 2 posiciones de una direccion y ak finak ignora la diferencia de altura

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);// el inverse convierte la direccion desde espacio global a espacio local para evitar que el calculo del yaw falle cuando la nave gira

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;//mathf.atan2 devuelve el angulo de un vector en radianes y sirve para obtener el angulo de una direccion
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);//calcula cuando debe girar la torreta desde su angulo inicial hasta el angulo objetivo
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);// el mathclamp limita un valor entre minimo y maximo, se impide que la torreta gire mas de lo que quieres

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);//aplica la rotacion final de la base, parte desde el angulo inicial. le suma la diferencia y se crea la rotacion con el quaternion.euler
    }

    private void ElevateCannon()
    {
        if (_targetTransform == null)
        {
            _cannonPivot.localRotation = Quaternion.identity;//el quaternion identity se usa para resetear rotaciones, aqui si no hay objetivo el canon vuelve a rotacion neutra
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }//se llama al metodo que intenta calcular el angulo vertical del disparo, si no pudo calcular el angulo ps sale del metodo

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);// aqui solo se limita el angulo vertical entre el mimimo y maximo

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f);//aplica la inclinacion del canon, se rota en el eje y
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle) //se intenta calcular el angulo vertical necesario para que el proyectil alcance el objetivo teniendo en cuenta todo, devuelve false si fisicamente no puede llegar
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position;//calcula la direccion desde el eje del canon hacia el objetivo

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude;//toma solo la distancia en horizontal, calcula la distanci horizontal al objetivo
        float verticalDistanceToTarget = directionFromCannonToTarget.y;// esto ps los contrario, calcula la distancia vertical entre canon y objetivo
        float gravityStrength = Mathf.Abs(Physics.gravity.y);//physiscs gravity es la gravedad global de unity
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed;//guarda la velocidad 

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);//aqui se busca el angulo necesario pata que un proyectil lanzado con cierta velocidad llegue al blanco bajo gravedad, se usa la formula de tiro parabolico en si

        if (formulaValueInsideSquareRoot < 0f)
        {
            cannonPitchAngle = _pitchLimits.y;
            return false;//si el valor es negativo el disparo no puede llegar al objetivo con esa velocidad aunque elevatecannon esa asignacion realmente no afecta en este flujo
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;//calcula el angulo del disparo usando la formula balistica, el mathf.sqrt es raiz cuadrada, mathf.atan arcotangente,devuelve el angulo en radianes y mathf.rad2Deg convierte a grados

        return true;
    }
}
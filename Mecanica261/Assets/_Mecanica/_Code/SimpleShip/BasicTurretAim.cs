using UnityEngine;
using UnityEngine.Rendering;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _cannonPivot;
    [SerializeField] private Transform _shipReferenceTransform;

    [SerializeField] private float _yawLimit = 145f;
    [SerializeField] private float _projectileSpeed = 250f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);

    //El punto de origen en donde hace que las torretas giren
    private float _startingYaw;

    private void Awake()
    {
        // Primero debemos calcular en que dirección esta mirando cada torreta en el eje Z, que si esta mirando hacia adelante (+z) o hacia atras (-z)
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);
    }

    private void Update()
    {
        //Y por último la torreta aplicara un giro de manera horizontal en la cual pueda apuntar figamente a un target
        // Y mientras que la torreta va girar de manera vertical para asi para presenciar un tiro barabólico
        RotateTurretBase();
        ElevateCannon();
    }

  
    //Primero debemos calcular la rotación que debe hacer nuestra torreta de manera horizontal y pueda apuntar figamente a un target(enemigo)
    private void RotateTurretBase()
    {
        if (_targetTransform == null) //Primero si detecta al target, entonces la torreta hará una rotación hacia mira fijamente al target
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);
            return;
        }

        //Ahora lo que tenemos que hacer es calcular las referencias de la rotación que tomará la torreta hacia el enemigo.
        // Aca estamos calculando la distancia que hay entre la toreta y el target, pero no tomando en cuenta el eje y. 
        Vector3 directionToTarget = _targetTransform.position - transform.position;
        directionToTarget.y = 0f;

        //Aca lo que estamos haciendo es que el directionToTarget lo estamos convirtiendo de coordenadas globales a locales, lo que hace de que la rotación
        //del barco hace que gire de manera local y disminuyendo la cantidad del giro de rotación.
        //Igual lo que puede hacer es que la torreta gire diferente a comparación del barco.
        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);

        //Ahora debemos ahora la rotación de nuestra torreta
        //Y para eso cuparemos un Mathf.Atan2 que lo que hace es devolver el ángulo en radianes cuya Tan es y/x. O en este caso seria z/x
        //Mediante la ditancia que esta el target en z y x. Y por último el resultado nos lo da en radianes, pero con un Mathf.Rad2Deg los radianos los convertimos en grados.
        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;

        //Con esto la torreta puede girar normalmente pero nosotros queremos que haya un limite de rotación
        //Para eso primero agregamos un float en la cual puede almacenar la diferencia de ángulos entre la posición de la torreta entre el enemigo.
        //Después agregamos otro float en la cual especifica el límite de rotación del yawDifferenceFromStart mediante la informacion que tenemos en yawLimit
        // Y ya por último agregamos que cuando vaya a girar la torreta de manera horizontal tome en cuenta el limite de rotación
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);
        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);
    }

    //Ahora que tenemos en que angulo debe girar las torretas para apuntar a un target de manera horizontal, ahora debemos calcular
    //el ángulo de elevación de la torreta para apuntar al target
    private void ElevateCannon()
    {
        //Primero si detecta al target, entonces la torreta hará una rotación hacia mira fijamente al target
        if (_targetTransform == null)
        {
            _cannonPivot.localRotation = Quaternion.identity;
            return;
        }

        // Después debemos de calcular el ángulo de lanzamiento parabólico.
        //Para eso ocuparemos el void de TryCalculateCannonPitchAngle, en la cual hará el calculo del ángulo de lanzamiento parabólico
        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }

        //Ahora que ya tenemos el calculo de angulo de lanzamiento parabólico, igual vamos a agregar un límite a la torreta
        //Primero vamos agregar un nuevo float en la cual entro de ahí vamos a darle la información de las limitantes de rotacion mediante 
        //un mathf.clmap. Dentro del Mathf.Clamp estamos indicando que el resultado de la calculadora del angulo de rotación
        // le estamos dando sus límites del pitch a base de los valores que los tenemos asignados.
        //Y por último le agregamos la informacion de limitedCannonPitchAngle a la hora de que la torreta detecta a un target
        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);
        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f);
    }


    //Esta es la calculadora de ángulo de lanzamiento parabólico
    // Esto es esencial si queremos que nuestras torretas hagan un giro vertical a la hora de apuntar un target, y cada torreta tendrá un
    //giro diferente por donde están colocados.
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
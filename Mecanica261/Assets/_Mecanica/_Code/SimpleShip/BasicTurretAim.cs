using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;  //asigna el transform del target al que va a seguir 
    [SerializeField] private Transform _cannonPivot;  //tranfrom del punto de que permite subir y bajar 
    [SerializeField] private Transform _shipReferenceTransform;  //asigna el transform del barco

    [SerializeField] private float _yawLimit = 145f;  //es el limite de apertura de la torreta
    [SerializeField] private float _projectileSpeed = 250f;  //la velocidad a la que va el proyectil
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);  //el limite se sube y baja del cañon 

    private float _startingYaw; //guarda el angulo inicial de la torreta 

    private void Awake()
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);  //guarda el angulo de Z al inicio
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

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;    //calcula en angulo necesario para apuntar al enemy 
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);  //calcula la diferencia entre el angulo inicial y el que se ocupa
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);    //no permite que el cañon gier mas de lo que puede 

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);    //aplica la rotacion a la base en el eje Z
    }

    private void ElevateCannon()
    {
        if (_targetTransform == null)   //si no hay objetivo
        {
            _cannonPivot.localRotation = Quaternion.identity;   //regresa el cañon a su posicion original 
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }  //llama a un funcion matematica y se cancela si el objetivo no esta en rango 

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);  //si el enemy esta en rango limita la subida o baja de la torreta entre 0 y 45

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f); //aplica la elevacion del cañon en el eje Y
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position;    //distancia entre el cañon y el enemy

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude; //calcula la distancia en un plano
        float verticalDistanceToTarget = directionFromCannonToTarget.y; //disntancia de la altura 
        float gravityStrength = Mathf.Abs(Physics.gravity.y);   //valor de la gravedad 
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed; //velocidad elevada al cuadrado 

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared); //esto es la parte de dentro de la raiz cuadrada de la ecuaicon vista en el salon

        if (formulaValueInsideSquareRoot < 0f)  //si el valor es negativo la raiz cuadrada el enemy esta muy lejos 
        {
            cannonPitchAngle = _pitchLimits.y;  //si ele enemy esta muy lejos el cañon se va a elevar lo mas que pueda para disparar 
            return false;
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;    //la ecuacion convierte los radianes a grados para elegir la trayectoria del tiro

        return true;
    }
}

//InverseTransformDirection https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform.InverseTransformDirection.html
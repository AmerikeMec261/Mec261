using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform; // guarda el transform del enemigo al que la torreta le va a apuntar.
    [SerializeField] private Transform _cannonPivot; // guarda el transfor del cañon que se mueve de arriba a abajo
    [SerializeField] private Transform _shipReferenceTransform;// es para saber donde esta orientado el transform del barco

    [SerializeField] private float _yawLimit = 145f; // este hace que se limite la torreta en 145 grados
    [SerializeField] private float _projectileSpeed = 250f;// esta es la velocidads de la bala
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f);//son los limites verticales de la torreta de 0 a 45°

    private float _startingYaw;// es el que guarda la posicion de la torreta

    private void Awake()// el awake es el que se ejecuta al iniciar el objeto
    {
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);//este creo es lo que guarda el angulo inicial z y calcula con el mathf la diferencia de angulos
    }

    private void Update()// se ejecuta en cada fps
    {
        RotateTurretBase();//llama a la funcion del codigo que gira la torreta
        ElevateCannon();//y este llama a la funcion del codigo que mueve la torreta de arriba a abajo
    }

    private void RotateTurretBase()
    {
        if (_targetTransform == null)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw);//aqui hace que si no existe el objeto no siga con el codigo y de el return y que si no esta el objeto regrese a su posicion la torreta
            return;
        }

        Vector3 directionToTarget = _targetTransform.position - transform.position;//este calcula la direccion hacia el enemigo
        directionToTarget.y = 0f;// este hace que solo sea el movimiento horizontal

        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget);// esto hace que la torreta gire aunque el barco este rotado

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg;//calcula el angulo horizontal hacia el enemigo y calcula radianes a grados
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle);// calcula cuanto gira desde donde inicia el barquito
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);// 

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference);//
    }

    private void ElevateCannon()//
    {
        if (_targetTransform == null)
        {
            _cannonPivot.localRotation = Quaternion.identity;//
            return;
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }//

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y);//

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f);//
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle)//
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position;//

        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude;//
        float verticalDistanceToTarget = directionFromCannonToTarget.y;//
        float gravityStrength = Mathf.Abs(Physics.gravity.y);//
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed;//

        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);//

        if (formulaValueInsideSquareRoot < 0f)
        {
            cannonPitchAngle = _pitchLimits.y;//
            return false;
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;//

        return true;
    }
}
using DG.Tweening;
using UnityEngine;

public class BasicTurretAim : MonoBehaviour
{
    //Se necesitan transforms para saber las rotacion, distancia y posiciones en cada uno 
    [SerializeField] private Transform _targetTransform; //Se utiliza para saber que tan lejos esta el target de la torreta utilizada para calcular la diferencia de distancia
    [SerializeField] private Transform _cannonPivot; //Este es el caon el cual se actualiza su transofrm para apuntar de acuerdo a los resultados  del calculo
    [SerializeField] private Transform _shipReferenceTransform; //Se utiliza este para tener una referencia de donde se posiciona el barco y las torretas tengan una referenciaSS

    [SerializeField] private float _yawLimit = 145f; //Se crea una variable de tipo float llamada _yawLimits que almacena el limite de rotacion utilizada para justo limitar el resultado de yawDifferenceFromStart
    [SerializeField] private float _projectileSpeed = 250f; //Se crea una variable de tipo float llamada _projectileSpeed  que almacena la velocidad utilizada en los calculos de la elevacion de torreta
    [SerializeField] private Vector2 _pitchLimits = new Vector2(0f, 45f); //Se crea una variable de tipo Vector 2 _pitchLimits donde este almacena los limites en el eje X (0) y (45) Y

    private float _startingYaw;//Se crea una variable privada de tipo float donde se almacena el dato inicial (0) para reiniciar la rotacion local de la torreta

    private void Awake()
    {
        //Link de Referencia de Mathf.DeltaAngle: https://docs.unity3d.com/es/530/ScriptReference/Mathf.DeltaAngle.html
        //Link de Referencia de transform.localEulerAngles: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Transform-localEulerAngles.html
        _startingYaw = Mathf.DeltaAngle(0f, transform.localEulerAngles.z); //Dentro de la variable _startingYaw se almacena el calculo de la diferencia entre el numero 0f y el transform local de la torreta en el eje z en angulos Euler (Con el objetivo que su rotacion inicie desde 0)
        //Nota: entiendo para que sirve Mathf.DeltaAngle y entiendo el objetivo de esta linea pero no entiendo como este es necesario, si este inicialmente puede iniciar su rotacion en cero si lo colocamos exactemente dentro de Unity
    }

    private void Update()
    {
        RotateTurretBase(); //Se actualiza la rotacion de la torreta constantemente
        ElevateCannon(); //Se actualiza la elevacion de los cañones de la torreta constantemente
    }

    private void RotateTurretBase() //Metodo para rotar la torreta
    {
        if (_targetTransform == null) //Se checa con una condicion si _targetTransform es deicr si el target no existe o si es nulo  
        {
            //Link de referencia Quaternion.Euler: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Quaternion.Euler.html
            transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw); 
            return; //Si se cumple esta condicion regresa la rotacion local de la torreta en los 3 ejes reiniciandolos a 0 (0, 0, Rotacion Euler guardad en _startingYaw) es decir su posicion inicial en angulo Euler
        }

        //Si la condicion no se cumple prosigue con las demas lineas
        Vector3 directionToTarget = _targetTransform.position - transform.position; //En tipo Vector3 se crea una variable llamada directionToTarget = que almacena la distancia del _targetTransform.position del transform.position que es de la torreta para la rotacion con una resta
        directionToTarget.y = 0f; //Una vez almacenada este calculo, se ignora el eje Y modificando su dato a 0 ya que solo necesitamos para la rotacion el eje X y Z


        //Link de Referencia de TnverseTransformDirection: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Transform.InverseTransformDirection.html
        Vector3 localDirectionToTarget = _shipReferenceTransform.InverseTransformDirection(directionToTarget); //Se crea otra variable de Vector3 llamada localDirectionToTarget, que almacena el transform inverso de directionToTarget desde la perspectivo o desde el transform de _shipReferenceTransform
        //Duda: no tengo muy aterrizada la linea pero gracias a la investigacion de InverseTransformDirection se como funciona pero no entiendo el objetivo de ponerlo.

        float targetYawAngle = -Mathf.Atan2(localDirectionToTarget.z, localDirectionToTarget.x) * Mathf.Rad2Deg; //Se crea una variable de tipo float llamada targetYawAngle que almacena 
        float yawDifferenceFromStart = Mathf.DeltaAngle(_startingYaw, targetYawAngle); //Se crea una variable detipo float llamada yawDifferenceFromStart que almacena  el calculo de la diferecnia entre dos angulos, en este caso calcula la diferecnia entre _startingYaw y de targetYawAngle
        float limitedYawDifference = Mathf.Clamp(yawDifferenceFromStart, -_yawLimit, _yawLimit);//Se crea una ultima variable de tipo float donde al utilizar Mathf.Clamp se limita a donde puede girar la torreta se le indica el valor que tiene yawDiferrenceFromStart y lo limita entre el minimo (valor negativo de -_yawLimit) y el maximo (valor positivo de _yawLimit)
        //Link de Referencia Mathf.Atan2: https://docs.unity3d.com/es/530/ScriptReference/Mathf.Atan2.html
        //Link de Referencia Mathf.Rad2Deg: https://docs.unity3d.com/es/530/ScriptReference/Mathf.Rad2Deg.html
        //Link de Referencia Mathf.Clamp: https://docs.unity3d.com/es/530/ScriptReference/Mathf.Clamp.html

        transform.localRotation = Quaternion.Euler(0f, 0f, _startingYaw + limitedYawDifference); //En la rotacion local del transform se guarda el nuevo vector en en angulo Euler donde X y Y se mantienen en cero y en z se almacena el nuevo vector que es la suma de _startingYaw y  limitedYawDifference   
    }

    private void ElevateCannon() //Metodo para elevar los cañones
    {
        if (_targetTransform == null) //Se checa con una condicion si _targetTransform es deicr si el target no existe o si es nulo  
        {
            //Link de Referencia de Quaternion.idntify: https://docs.unity3d.com/ScriptReference/Quaternion-identity.html
            _cannonPivot.localRotation = Quaternion.identity; 
            return;//Si se cumple la condicion dentro del _cannonPivot establece la rotacion local del objeto para que coincida con la orientacion de el padre
        }

        if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; } //Aqui se almacena los datos recolectados que se hicieron en private bool TryCalculateCannonPitchAngle junto conlo que regrese donde si el resultado es fisicamente posible es decir return (true o false)

        float limitedCannonPitchAngle = Mathf.Clamp(cannonPitchAngle, _pitchLimits.x, _pitchLimits.y); //Se crea una variable de tipo float llamada limitedCannonPitchAngle donde con Mathf.Clamp se limita el valor que tiene cannonPtchAngle con el valor minimo que es _pitchLimits.x del eje x y el valor maximo que es _pitchLimits.y del eje Y

        _cannonPivot.localRotation = Quaternion.Euler(0f, limitedCannonPitchAngle, 0f); //dentro de la rotacion local de _cannonPivot guarda en angulos euler el el vector que es (0, limitedCannonPitchAngle, 0) solo se necesita el eje Y de la rotacion local ya que es la elevacion del cañon
    }

    private bool TryCalculateCannonPitchAngle(out float cannonPitchAngle) //En este private bool solo almacena los datos para los calculos en cannonPtchAngle y posicion en la que se deberia encontrar el vector
    {
        Vector3 directionFromCannonToTarget = _targetTransform.position - _cannonPivot.position; //Se crea una variable de tipo Vector3 llamada directionFromCannonToTarget que guarda la distancia en vectores, la distancia que tiene posición del cañon con la del target, con una resta es decir _targetTransform.position - _cannonPivot.Position


        float horizontalDistanceToTarget = new Vector2(directionFromCannonToTarget.x, directionFromCannonToTarget.z).magnitude; //Se crea otra variable float llamada horizontalDistanceToTarget que como dice el nombre guardara la distancia que tienen en horizontal en un nuevo vector (Vector2) que manejara eje X y Z solo devolviendo la longitud de estos vectores (.magnitude)
        //Link de referencia de .magnitude: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Vector3-magnitude.html 
        float verticalDistanceToTarget = directionFromCannonToTarget.y; //Se crea otra variable de tipo float llamada verticalDistanceToTarget que guarda el eje Y de directionFromCannonToTarget (la distancia vertical)
        float gravityStrength = Mathf.Abs(Physics.gravity.y);  //Se crea otra variable de tipo float para guardar la fuerza de la gravedad, se guarda el dato de la gravedad del eje Y que nos da unity (Physics.gravity.y) sin embargo este lo maneja negativo y lo necesitamos positivo entonces con "Mathf.Abs" convertimos ese número a uno sin signo, es de decir a un valor positivo para poder utilizarla en la fórmula y tener un calculo correcto.
        //Link de referencia de Mathf.Abs: https://docs.unity3d.com/es/530/ScriptReference/Mathf.Abs.html
        //Link de referencia de que signo tiene por default Unity en cuanto a la gravedad en Y: https://docs.unity3d.com/es/530/Manual/class-PhysicsManager.html
        //Link de referencia Physics.gravity: https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Physics-gravity.html
        float projectileSpeedSquared = _projectileSpeed * _projectileSpeed; //Se crea otra variable de tipo float que guardará la velocidad al cuadrado del projectile es decir para almacenar esto solo se necesita multiplicar la velocidad del projectile por si misma.

        //Para resolver la fórmula primero necesitamos resolver la raíz el cual se almacenara en la variable de tipo float formulaValueInsideSquareRoot con el objetivo de que se verifique si es posible el calculo. 
        float formulaValueInsideSquareRoot = projectileSpeedSquared * projectileSpeedSquared - gravityStrength * (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget + 2f * verticalDistanceToTarget * projectileSpeedSquared);
        //Primero se necesita la velocidad al cubo V⁴ dónde solo tenemos que multiplicar la velocidad al cuadrado de projectileSpeedSquared por si misma = projectileSpeedSquared * projectileSpeedSquared
        //Se necesita restar la fuerza de la gravedad en este caso gravityStrength = - gravityStrength 
        //Se abre paréntesis donde se multiplica la gravedad por la distancia horizontal al cuadrado = (gravityStrength * horizontalDistanceToTarget * horizontalDistanceToTarget 
        //Por último se suma 2 multiplicado por la distancia vertical por la velocidad al cuadrado cerrando el paréntesis y terminando con la raíz =  2f * verticalDistanceToTarget * projectileSpeedSquared);


        if (formulaValueInsideSquareRoot < 0f) //Se crea una condición el cual checha si el valor que da la raíz (formulaValueInsideSquareRoot) es menor que cero 
        {
            cannonPitchAngle = _pitchLimits.y; //Si se cumple el pitch del cañón es decir verticalmente en el eje Y se pone automáticamente en el límite maximo asignado (_pitchLimits.y).
            return false; //Regresa falso(0) el cual se utilizara en la condicion de ElevateCannon dentro de la condicion if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }. Esto se realiza con el objetivo de decirle a la formula, confirmar que el resultado posible para ejecutar la accion

            //Nota: tuve duda para que era false y true si la condicion ya cumple con la accion de seguir con el calculo o almecenarlo en la distancia maxima sin embargo este metodo de private bool solo almacena los datos y en la condicion de ElevateCannon es donde ocurre la accion y donde se utiiiza los datos almacenados que regresa returno osea True o False
        }

        cannonPitchAngle = Mathf.Atan((projectileSpeedSquared - Mathf.Sqrt(formulaValueInsideSquareRoot)) / (gravityStrength * horizontalDistanceToTarget)) * Mathf.Rad2Deg;
        //Para a completar el calculo se resuelven los parentesis que es la velocidad al cuadrado (projectileSpeedSquared) menos la raiz cuadrada (Mathf.Sqrt) de el valor de (formulaValueInsideSquareRoot) entre (/) la fuerza de la gravedad por la distancia horizontal (gravityStrength * horizontalDistanceToTarget))
        //Luego se calcula el resulta de esa division con el arco tangente(Mathf.Atan)
        //Por ultimo todo se deja en grados multiplicando todo el resultado por (Mathf.Rad2Deg) para de radianes a grados

        //Link de referencia de Mathf.Atan: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.Atan.html
        //Link de referencia de Mathf.Sqrt: https://docs.unity3d.com/es/530/ScriptReference/Mathf.Sqrt.html
        //Link de referencia de Mathf.Rad2Deg; https://docs.unity3d.com/6000.1/Documentation/ScriptReference/Mathf.Rad2Deg.html

        return true; //Regresa true(1) el cual se utilizara en la condicion de ElevateCannon dentro de la condicion if (!TryCalculateCannonPitchAngle(out float cannonPitchAngle)) { return; }. Esto se realiza con el objetivo de decirle a la formula, confirmar que el resultado posible para ejecutar la accion
    }
}
using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _reticle;

    [Header("Yaw Settings")]
    [SerializeField] private float _yawSpeed = 90f;
    [SerializeField] private Vector2 _yawLimits = new Vector2(-90f, 90f);

    [Header("Pitch Settings")]
    [SerializeField] private float _pitchSpeed = 90f;
    [SerializeField] private Vector2 _pitchLimits = new Vector2(-10f, 90f);

    [Header("Disparo Settings")]
    [SerializeField] private float _maxDistance = 90f;

    //Apartir de aqui pongo estos datos en privado porque no son necesarios que se vean en el editor o que se modifiquen en el
    private Vector3 _mousePoint;
    private Vector3 _targetPoint;
    private Vector3 _Velocity;
    private bool _solution;

    //En el proyecto que se sta desarrolando se utiliza Vector3 para saber donde se encuentra cada cosa

    /*Por ejemplo
     Mouse point y target point guardan los datos de la posicion en donde se encuentran.
    Mouse point guarda la posicion dentro del juego de a donde se esta apuntando mientras que target point guarda a donde se esta apuntando o donde debe estar ka reticula. 
    En cuanto a Velocity este guardara hacia donde ira la bala y con que fuerza ira en cada direccion para que la bala llegue hacia el objetivo.
    Por ultimo para solution utilizaremos bool para decidir si la puede llegar hacia donde apuntemos o si es imposible por el calculo por la fisica.

    Nota: Utilizar Vecotr3 en la parte del mouse es importante porque si este no esxiste la torreta no sabra si el objetivo esta en el suelo o volando lo cual no sabra especificamente a donde disparar. 

     */ 
    public void FireProjectile()
    {
        //Aqui la condicion de if es sobre  si falta el prefab de bullet o si no hay solucion la torreta no disparara nada
        //Lo que regresa (return) es nada ya que lo detiene ahi mismo 
        if (_bulletPrefab == null || !_solution)
        {  
            return; 
        }
        
        GameObject currentBullet = Instantiate(_bulletPrefab, _bulletSpawn.position, _bulletSpawn.rotation);
        currentBullet.transform.forward = _Velocity.normalized;
        currentBullet.GetComponent<IProjectile>()?.Fire();
        /*
        Primero se crea un objeto llamado currentBullet este copiara el prefab de la bala (Instantiate(_bulletprefab)) lo pone en la posicion que se encuentre _bulletSpawn junto con su rotacion
        La mueva bala aparece mirando hacia frente en la direccion que se encuentre el calculo de velocity sin la velocidad apuntando correctamente primero para despues aplicar la velocidad con Fire()
        La misma bala que se utilizara se revisara si tiene el script de <IProjectile> en las balas, si la ecnuentra llama a la funcion de Fire ya sea en el SimpleBullet o Explosive Bullet de acuerdo al script que tenga la bala llamara esta funcion ?.Fire()   
        */

    }

    private void Update()
    {
        UpdateMousePoint(); //Se utiliza para convertir la posicion a 3D del suelo para saber donde llegara la bala constantemente
        CalculatedShot(); //Aqui se calcula la trayectoria de la bala considerenado facotres como la gravedad, velocidad del proyectil y distancia del objetivo. Tambien verifica contantemente si es posible disparar a donde se esta apuntando y como este debe de volar.
       

        RotateYaw(GetYawInput()); //En este caso gira la base de la torreta ya sea izquierda o derecha calculando cuanto debe girar horizontalemnte para llegar al objetivo
        RotatePitch(GetPitchInput());//Esta funcion hace que el cañon de la torreta vaya arriba o abajo calculando cuanto deberia ir verticalmente para llegar al objetivo.

        //Una condicion simple donde verifica si se presiono el espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            FireProjectile();//FireProectile es el que dispara y apunta si se cumple la condicion creando una nueva bala y lanzandolo con la fisica aplicada 
        }
    }

    private void UpdateMousePoint()
    {
        //Utilizamos Ray una linea recta infinita guardada en la variable ray para que desde la camara salga una limea recta hacia donde este apuntando el mouse.
        //Y se utiliza Raycasthit para guardar la informacion del choque con la variable hit.
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;


        /*En esta condicion si utilizamos el sistema de fisicas de Unity y lanza el rayo en un maximo de 1000 metros pero solo contra objetos que tengan la capa (Layer)"Ground" con el fin de que solo choque con superficies que tengas ese layer gracias al filtro de ( LayerMask.GetMask("Ground")) 
        ya que si este no tuviera este filtro estaria chocando con todo y este solo se acercaria a donde esta puntando el mouse contando incluso el cielo no estaria apuntando exatamente en el suelo*/

        //Nota: Utilizamos la cantidad de 1000f ya que si no tuviera esta cantidad el rayo invisible seria inifnito y a pesar de que este seguiria funcionando igual
        //encontrando una superficie con el layer "Ground" por cuestiones de optimizacion pongo ese limite para que la computadora en la que se esta trabjando no
        //cargue de manera lenta ya que este checa la escena o mundo entero en cada frame
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
        {
            _mousePoint = hit.point; //Si se cumple la condicion se copia la posicion o coordenadas que tenga el suelo en donde choco el rayo en la variable hit
        }
        else
        {
            _mousePoint = _bulletSpawn.position + (_bulletSpawn.forward * _maxDistance); //Si no se cumple disparara en la distancia maxima que son los 90f hacia adelante si no encuentra suelo
        }

    }

    private void CalculatedShot()
    {
        //Aqui la condicion evalua si no hay prefab de la bala asignada
        if (_bulletPrefab == null)
        {
            //Si se cumple se desactiva el disparo y de nuevo calcula el punto hacia enfrente en la maxima distancia que denuevo es 90 
            //Y mueve la reticula al punto en el que se posiciona y sale de la funcion sin calcular ninguna trayectoria
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            MoveReticle();
            return; 
        }
        //Dentro del prefab de la bala busca el script de la bala SimpleBullet o ExplosiveBullet dependiendo de que tenga lo busca dentro del prefab y lo guarda en la variable projectile
        IProjectile projectile = _bulletPrefab.GetComponent<IProjectile>();

        if (projectile == null) //En este caso verifica si no se encontro el script de IProjectile en el prefab de la bala
        {
            //Aqui sucede lo mismo que en la condicion de arriba 
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            MoveReticle();
            return;
        }

        float speed = projectile.Speed(); //De la variable projectile Speed llamada al metodo de Speed() de Simple y Explosive Bullet y guarda el valor que tenga en speed que tiene un dato de tipo float
        Vector3 direction = _mousePoint - _bulletSpawn.position; //Lo que guarda en direction es el calculo de la posicion del mouse restando la posicion de donde se encuente el spawn de la bala con el objetivo de saber cuanta es la distancia que necesita recorrer la bala para llegar al objetivo de donde esta apuntando el mouse
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);//En la variable directionXZ lo que guarda aqui es la direccion de X y Z sin contar la altura Y 

        //Estos son datos flotantes que se utilizaran para el calculo del disparo
        float x = directionXZ.magnitude;//Guarda la distancia horizontal del plano del suelo
        float y = direction.y; //Se guarda la distnacia de la altura entre el caon y del objetivo
        float g = Mathf.Abs(Physics.gravity.y); //Aqui lo que hacemos es transformar la gravedad que nos da unity que es -9.81 a positivo gracias a Mathf.abs transformandolo en 9.81 funcional para el calculo
        float speedFOR2 = speed * speed; //Esta variable solo calcula le velocidad al cuadrado

        if (x < 0.1f) //Aqui se verifica si la distancia horizontal es menor a 0.1f es decir 0.1m
        {
            //Si es menor sucede lo mismo que las primeras dos condiciones de bulletprefab y projectile
            _solution = false;
            _targetPoint = _bulletSpawn.position + _bulletSpawn.forward * _maxDistance;
            MoveReticle();
            return;
        }

        /*
        Primero empezamos calculando root (La raiz) entonces este lo ponemos en tipo float y con nombre root para que guarde el calculo de root
        Dentro de esta formula se multiplican las velocidades al cuadrado y se resta con la graveadad para comprobar la trayectoria
        Luego g * x * x representa el efecto de la gravedad sobre la distancia hoizontal sobre el objetivo
        Por ultimo 2f * Y * speedFor2 es el efecto de la altura entre el objetivo y el cañon  junto con la velocidad del disparo

        En resumen el valor de root se calcula con la velocidad al cuadrado, la gravedad, la distancia horizontal y vertical con el objetivo 
        de saber si la bala llegara el objetivo

        Nota:Cuando es imposible que la bala llegue fisicamente a un punto es cuando el resultado de la raiz (root) es negativo
         */
        float root = (speedFOR2 * speedFOR2) - g * ((g * x * x) + (2f * y * speedFOR2));

        if (root < 0f) //la condicion verifica si el resultado de root es negativo 
        {
            //Si es negativo es fisicamente imposible que la bala llegue ahi marcando que no hay una solucion
            _solution = false;
            _targetPoint = _bulletSpawn.position + directionXZ.normalized * _maxDistance; //El punto del objetivo se coloca automaticamente en la distancia maxima hacia adelante al igual que las demas condiciones de este metodo CalculatedShot
            MoveReticle();
            return;
        }
        /*
        Se calcula HAngle usando una formula utilice Mathf.Atan para convertir el angulo en radianaes para que la apunte de manera correcta
        Dentro de esto se calcula la velocidad al cuadrado mas la raiz cuadrada de (root) para eso utilice la funcion Mathf.Sqrt entre la gravedad por la distancia horizontal de el cañon al objetivo
         
        Despues se calcula la direccion final (FDirecion) con la direccion horizontal del plano XZ por el Coseno direecion horizontal (Hangle) mas el Vector vertical es decir hacia arriba  que representa la direccion al cielo por el Seno del angulo horizontal (HAngle)

        Nota: El proposito de estos calculos es calcular el nagulo correcto (Hangle) y para convertir ese angulo en una diraccion de Vector3D osea la direccion final
         */
        float HAngle = Mathf.Atan((speedFOR2 + Mathf.Sqrt(root)) / (g * x));
        Vector3 FDirection = directionXZ.normalized * Mathf.Cos(HAngle) + Vector3.up * Mathf.Sin(HAngle); 

        _Velocity = FDirection * speed; //Velocity es el caluclo de la direccion final por la velocidad para darle la velocidad y que vuele con la intensidad indiciada
        _targetPoint = _mousePoint;//Se actualiza el targetpoint para que apunte a donde apunte el mouse
        _solution = true; //Si la solucion es verdadera esto indicara que es fisicamente posible que la bala llegue al objetivo

        MoveReticle(); //Llama a MoveReticle para mover la reticula al punto de impacto calculado
    }

    private void MoveReticle()
    {
        if (_reticle != null) //Primero se verifica si la reticula existe para evitar errores
        {
            _reticle.position = _targetPoint; //Si la reticula esta asignada se pone en la posicion que se encuentre _targetpoint en la escena
        }
    }
    private float GetYawInput()
    {
        //Primero se calcula la direccion desde la base de la torreta hasta el punto por eso yawDirection toma esa direccion restandola entre _targetPoint y la posicion del pivote yawPivot.position
        //De esa dirrecion se ignora la direccion Y para que sea totalmente horizontal para que gire hacia la izquierda o hacia la derecha
        Vector3 yawDirection = _targetPoint - _yawPivot.position;
        yawDirection.y = 0f;

        if (yawDirection == Vector3.zero) //Verifica si la direccion hacia el objetivo yawDirection es igual a cero es decir si esta en la misma posicion en la que esta el pivote
        {
            return 0f; //Si se cumple la condicion la torreta no girara ni a la dercha o izquierda para evitar calculos inecesarios o algun movimiento extraño
        }

        //Se calcula el angulo con signo entre dos direcciones para girar a la derecha (positivo) o izquierda (negativo)
        float angle = Vector3.SignedAngle(_yawPivot.forward, yawDirection, Vector3.up);

        if (angle > 1f) //Si el angulo es mayor a 1f gira hacia la derecha devolviendo 1f
        {
            return 1f;
        }
        if (angle < -1f) //Si el angulo es menor a -1f gira hacia la izquierda devolviendo -1f
        {
            return -1f;
        }
        
        return 0f; //Si el angulo esta dentro del rango de 1f y -1f es decir que que se queda en 0f la base no se mueve
    }

    private float GetPitchInput()
    {
        //Aqui se calcula la direccion de hacia donde quiere apunatr el cañon si la bala puede llegar al punto usa la direccion calculada,si no hay solucion solo usa la direccion directa hacia el objetivo
        //Tambien se calcula el angulo con signo entre la direccion hacia enfrente del cañom y hacia la direccion del objetivo mide el giro hacia arriba y abajo usadno el ejede derecho del cañon
        Vector3 targetDirection = _solution ? _Velocity.normalized : (_targetPoint - _pitchPivot.position).normalized;
        float angle = Vector3.SignedAngle(_pitchPivot.forward, targetDirection, _pitchPivot.right); 

        //Si el angulo es mayour que 1f el cañon sube
        if (angle > 1f)
        {
            return 1f;
        }
        //Si el angulo es menor a -1f el caon bajara poco
        if (angle < -1f)
        {
            return -1f;
        }
        //Al igual que el otro si el angulo se encuentra entre estos datos el cañon no se mueve y se devuelve 0f
        return 0f;
    }

    private void RotateYaw(float input)
    {
        //Se calcula cuanto girara la base de la torreta en un frame y para que este gire depende del valor de input 1f izquierda, -1f derecha y 0 sin movimiento
        //Se aplica tambien cuanto una rotacion hacia la izquierda o derecha al objeto del pivote y solo rota en el eje Y, en su propio espacio consiguiendo girar suavementa el base de la torreta hacia donde debe apuntar
        float yawChange = input * _yawSpeed * Time.deltaTime;
        _yawPivot.Rotate(0f, yawChange, 0f, Space.Self); 
    }

    //Este es parte del codigo base que nos dio en clase para evitar errores solo lo comente no requiere de ningun uso pero si tener referencia para hacer el proyecto o entender como funciona
    /*private void RotateYaw(float input) 
    {

        float yawChange = input * _yawSpeed * Time.deltaTime;
        float newYaw = Mathf.Clamp(_yawPivot.localEulerAngles.y + yawChange, _yawLimits.x, _yawLimits.y);
        _yawPivot.localEulerAngles = new Vector3(_yawPivot.localEulerAngles.x, newYaw, _yawPivot.localEulerAngles.z);
    }
    */

    private void RotatePitch(float input)
    {
        //Aqui se calcula cuanto debe subir o bajar el caon usando el input y la velocidad configurada
        //Para evitar giros exagerados actualiza el angulo de pitch y lo limita entre un minimo y maximo
        //Aqui hace que el cañon gire suavemente limitnado el angulo z sin cambiar X y Y
        float pitchChange = input * _pitchSpeed * Time.deltaTime;
        float newPitch = Mathf.Clamp(_pitchPivot.localEulerAngles.z + pitchChange, _pitchLimits.x, _pitchLimits.y);
        _pitchPivot.localEulerAngles = new Vector3(_pitchPivot.localEulerAngles.x, _pitchPivot.localEulerAngles.y, newPitch);
    }
}

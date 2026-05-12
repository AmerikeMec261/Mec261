using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class TurretBarco : MonoBehaviour
{
    [Header("Cannon parts")]
    [SerializeField] private Transform towerOrigin; //el origen de la torreta 
    [SerializeField] private Transform picth; //punto de inclinacion de arriba abajo
    [SerializeField] private Transform yaw; //punto de giro de izquerda a derecha 
    [SerializeField] private Transform bulletExit; //punto del que sale la bala

    [Header("Ammo configuration")]
    [SerializeField] private List<GameObject> bullets; //es una lista para poner las balas
    [SerializeField] private float bulletSpeed; //la velocidad de las balas 

    [Header("Turret movement")]
    [SerializeField] private float rotationSpeed; //la velocida de rotacion de izquierda a derecha 
    [SerializeField] private float rotationDegrees; //velocidad en que sube o baja
    [SerializeField] private float maxElevation = 45f; //el maximo de elevacion
    [SerializeField] private float maxDown = 5f; //lo maximo que baje para que la bala pueda salir y no se dispare a si mismo

    [Header("Tracking")]
    [SerializeField] private float trackingRange = 50f; //el rango del seguimiento del tag
    [SerializeField] private string enemyTag = "Enemy"; //el nombre del tag
    [SerializeField] private float visionAngle = 90f; //lo maximo que se abre 

    private int currentBullet = 0; //dice que bala esta en el momenot, como en la programcion se cuenta desde 0 por eso inicia en 0
    private bool enemyInRange = false; //es un bool que dice si el enemy esta en rango o no
    private Transform _target; //referencia de aquien esta apuntando 

    void Start() //un start solo se ejecuta al pirncipio del juego 
    {
        SelectWeapon(); //esta parte llama la fucnion de la bala para asignarle su velocidad 
    }
    
    void Update() //este se ejecuta constantmente 
    {
        ChangeBullet(); //llama la fucnion para ver si el jugador presiono la tecla para cambiar de bala
        
        
        if (Input.GetKeyDown(KeyCode.Space) && enemyInRange) //comprueba si el jugador presiono la tecla y si el enemigo esta en rango
        {
            FireBullet(); //si lo hizo dispara la bala
        }
    }

    private void FixedUpdate() //este es parecido al update normal pero sirve mas si el codigo tiene calculos matematicos o rotaciones en este caso las funciones de Yaw y Pitch
    {
        RotationYaw(); //llama la funcion de la rotacion de Yaw
        RotationPitch(); //llama la funcion de la rotacion de Pitch
    }

    private void RotationYaw() //es el nombre de la funcion para la rotacion del yaw
    {
        GameObject enemy = GameObject.FindGameObjectWithTag(enemyTag); //busca en la escena el primer objeto con el tag de enemy
        enemyInRange = false;  //de manera predeterminada supone que no hay enemy8888

        if (enemy != null) //si encontro al enemy 
        {
            Vector3 dirToEnemy = enemy.transform.position - yaw.position;  //usando un Vector3 pq este ve por las direcciones de x,y,z, la matematica que va depues de dirToEnemy es para crear una especie de flecha que apunta desde la torreta hasta el enemy
            float distance = dirToEnemy.magnitude; //el .magnitude sirve para calular la distancia en metros de entre la torreta y el enemigo
            float angleToEnemy = Vector3.Angle(yaw.forward, dirToEnemy); //el.angle calcula la direccion del enemigo en grados

            if (distance <= trackingRange && angleToEnemy <= visionAngle) //sila distancia es menor o igual al rango de track y el angulo esta dentro del campo de viison 
            {
                _target = enemy.transform; //va a estabkecer el campo del enemy como el enemigo actual
                enemyInRange = true; //confrima que hay un enemy en el rango
            }
        }

        if (enemyInRange && _target != null) //si hay un enemigo en rango y la variable es nula 
        {
            Vector3 dirForRotation = _target.position - yaw.position; //vuelve a calcular el vector de direccion 
            dirForRotation = Vector3.ProjectOnPlane(dirForRotation, towerOrigin.up); //<- Posible uso de IA   //esta linea permite hacer que la torreta no se gire hacia arriba o abajo solo se gire el cañon

            float signedAngle = Vector3.SignedAngle(towerOrigin.forward, dirForRotation, towerOrigin.up); //calcula el agnulo entre el frente de la base y el enemy, la diferncia entre el Angle() y este es que este da tanbto numeros positivos como negativos, estos numeros dicen si debe girar a la izquerda o a la derecha 
            float clampedAngle = Mathf.Clamp(signedAngle, -rotationDegrees, rotationDegrees); //esta parte usa el .Clamp ya que este fuerza atener un dato mino y un dato como maximo, en este caso el minimo y el maximo de abertura de la torreta

            Quaternion targetRotation = Quaternion.Euler(0f, clampedAngle, 0f); //esat liena hace que solo se tome el dato de "y" para la rotacion 
            yaw.localRotation = Quaternion.RotateTowards(yaw.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); //esta linea permite rotar la torreta levemente. La ultim parte dice cuanto puede rotar por frame usando el Time.fixedDeltaTime todo eso para que rote sin importar los fps
        }
        else
        {
            _target = null; //si no hay enemigo en el rango que borre al enemy actual 
            Quaternion idleRotation = Quaternion.Euler(0f, 0f, 0f); //permite que la torreta vuelva a su posicion origial 
            yaw.localRotation = Quaternion.RotateTowards(yaw.localRotation, idleRotation, rotationSpeed * Time.fixedDeltaTime); //hace quye gire suavemente a si posision original 
        }
    }

    private void RotationPitch()//es el nombre de la funcion para la rotacion del Picth
    {
        if (picth == null || _target == null || !enemyInRange) //<- Posible uso de IA por sobre uso de código defensivo //este es un seguro que hace que si el trasnfroma del pitch, el objetivo es nulo o no hay enemigo 
        {            
            picth.localRotation = Quaternion.RotateTowards(picth.localRotation, Quaternion.identity, rotationSpeed * Time.fixedDeltaTime); //que regrese el cañon a su posiicon original 
            return; //detiene la ejecucion de la funcion 
        }
        
        Vector3 dirToTarget = _target.position - picth.position; //calcula la direccion en 3D apuntando desde el cañon pitch hacia el enemy
        
        Quaternion lookRotation = Quaternion.LookRotation(dirToTarget, yaw.up); //hace que el pitch apunte directamente al enemy en este caso arriba o abajo 
        Quaternion relativeRotation = Quaternion.Inverse(yaw.rotation) * lookRotation; //<- Posible uso de IA por sobre uso de código defensivo  //este hace que la base de la torreta no gire y solo lo haga el cañon

        float targetPitchAngle = relativeRotation.eulerAngles.x; //este saca el valor de los grados que deben de tener los cañones 
        
        if (targetPitchAngle > 180f) targetPitchAngle -= 360f; //esta linea hace que el anuglo de inclinaicon que sean mayores a 180 se hagan negativos 


        float clampedPitch = Mathf.Clamp(targetPitchAngle, -maxElevation, maxDown); //el .clamp limite los grados de inclinaicon de la torreta 
        
        Quaternion targetRotation = Quaternion.Euler(clampedPitch, 0f, 0f); //convierte la inclinaicon en grados aplicado al eje x
        picth.localRotation = Quaternion.RotateTowards(picth.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); //aplica una rotaicon suave para la torreta
    }


    private void FireBullet()
    {
        if (bullets.Count > 0 && bullets[currentBullet] != null && bulletExit != null) //<- Posible uso de IA por sobre uso de código defensivo //es un candado que ayuda a verificar que haya balas, que haya una bvala exisitiendo y que exista el punto de bulletExit()
        {
            GameObject bullet = Instantiate(bullets[currentBullet], bulletExit.position, bulletExit.rotation); //instacia una bala en la posisicion de bulletExit()

            IProjectile projectileScript = bullet.GetComponent<IProjectile>(); //llama al script IProjectile en la bala que se acaba de crear
            if (projectileScript != null) //<- Posible uso de IA por sobre uso de código defensivo //si la bala tiene el script 
            {
                projectileScript.Fire(); //va a llamar al metodo de Fire() de IProjectile
            }
            else //si no lo ignora y dispara por medio de fisicaas 
            {
                Rigidbody rigidBody = bullet.GetComponent<Rigidbody>(); //obtiene el RigidBodyd de la bala para acceder a sus fisicaas
                if (rigidBody != null) //<- Posible uso de IA por sobre uso de código defensivo //si tienen un RigidBoidy
                {
                    rigidBody.linearVelocity = bulletExit.forward * bulletSpeed; //le aplica una velocidad mandandola hacia delante multiplicandola por la velocidad establecida al principio 
                }
            }
        }
    }

    private void MathShoot()
    {
        if (bulletExit == null || picth == null || _target == null || !enemyInRange) return; //<- Posible uso de IA por sobre uso de código defensivo //si falta alguna de las cosas que busca cancela toda la matematica

        Vector3 dir = _target.position - bulletExit.position; //dirreccion del enemy 
        float x = new Vector2(dir.x, dir.z).magnitude; //calcula la distancia entre X y Z 
        float y = _target.position.y - bulletExit.position.y; //calcula la diferencia de altura Y
        float g = 9.81f; //valor de la graveda de la tierra 
        float v = bulletSpeed; //velocidad de la bala 
        float targetPitchAngle = 0f; //variable para guardar el angulo que ocupa el cañon para dar al enemy

        if (v > 0) //si la bala no tiene velocidad 
        {
            float root = (v * v * v * v) - g * (g * (x * x) + 2 * y * (v * v)); //esat fromula calcula la raiz de la ecuacion dada por el profe 

            if (root >= 0) //si la rais es mayor o igual a 0
            {
                float angleRadians = Mathf.Atan(((v * v) - Mathf.Sqrt(root)) / (g * x)); //obtiene el angulo en radianes lo que permite hacer que el tiro haga la parabola 
                targetPitchAngle = -(angleRadians * Mathf.Rad2Deg); //es una constante que convierte los angulos a grados es unity 
            }
            else //si la raiz es negativa es imposible alcanzar el objetivo por la garvedad 
            {
                Vector3 localDir = picth.parent.InverseTransformPoint(_target.position); //convierte la psosion del enemy a coordenadas locales para el cañon
                targetPitchAngle = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg; //calcula el angulo basao en las posiciones de X y Z en lugar de hacer una diviosn que  pueda dar 0
            }
        }
        else
        {
            Vector3 localDir = picth.parent.InverseTransformPoint(_target.position); //si la bala no tiene velocidad apunta al objetivo sin calcular la caida 
            targetPitchAngle = Mathf.Atan2(-localDir.y, localDir.z) * Mathf.Rad2Deg;
        }

        float clampedPitch = Mathf.Clamp(targetPitchAngle, -maxElevation, maxDown); //limita el calculo de la inclinacion 
        Quaternion targetRotation = Quaternion.Euler(clampedPitch, 0f, 0f); //convierte en Quaternion el eje X
        picth.localRotation = Quaternion.RotateTowards(picth.localRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); //rota el cañon
    }

    private void ChangeBullet()
    {
        if (bullets.Count <= 1) return; //si hayn 1 o 0 balas en  la lista cancela la funcion 

        int previousBullet = currentBullet; //guarda la bala que teniamos antes de cambiar
        if(Input.GetKeyDown(KeyCode.Q)) //si presionas la Q
        {
            if (currentBullet >= bullets.Count - 1) //Si llego a al final de la lista de las balas 
            {
                currentBullet = 0; //regresa a la bala 0
            }
            else
            {
                currentBullet++; //si no ah llegado suma 1
            }
        }

        if(previousBullet != currentBullet) //el jugador presiono la Q se cambio de bala 
        {
            SelectWeapon(); //llama la funciuon que trae los datos de la bala 
        }

    }

    private void SelectWeapon()
    {
        if(bullets.Count > 0 && bullets[currentBullet] != null) //verifica que la lista de balas no este vacia 
        {
            bulletSpeed = bullets[currentBullet].GetComponent<IProjectile>().Speed; //actualiza la variable de velocidad con el IProjectile 
            print("Bala cambiada" + bullets[currentBullet].name); //pone en la consola que la bala se cambio 
        }
    }

    private void OnDrawGizmos()
    {
        if (yaw != null)
        {            
            Gizmos.color = Color.yellow; //la pinta de color amarillo 
            Gizmos.DrawWireSphere(yaw.position, trackingRange); //hace una esfera que marca el rango de la torreta 
            
            Gizmos.color = Color.red; //la pinta de color rojo 
            Vector3 rightLimit = Quaternion.AngleAxis(visionAngle / 2f, Vector3.up) * yaw.forward; //calcula el limite derecho del cono de vision
            Vector3 leftLimit = Quaternion.AngleAxis(-visionAngle / 2f, Vector3.up) * yaw.forward; //calcula el limite izquuerdo del cono de vision

            Gizmos.DrawRay(yaw.position, rightLimit * trackingRange); //muestra un linea en la parte derecha que muestra el limite de abertura 
            Gizmos.DrawRay(yaw.position, leftLimit * trackingRange); //muestra un linea en la parte izquerda que muestra el limite de abertura 
        }

        if (enemyInRange && _target != null) //solo se activa su hay un enemigo en el rango
        {
            Gizmos.color = Color.green; //pinta de color verde 
            Gizmos.DrawLine(bulletExit != null ? bulletExit.position : picth.position, _target.position); //dibuja una liena desde bulletExit() hasta el enemy 
            Gizmos.DrawWireSphere(_target.position, 1f); //si hya varios enemy pone un esfera verde para saber a cual esta apuntando en ese instante 
        }
    }
}

//Para el cambio de balas use este video
//https://www.youtube.com/watch?v=Dn_BUIVdAPg

//Para el traqueo del enemigo 
//https://www.youtube.com/watch?v=lV47ED8h61k

//ProyectOnPlane https://docs.unity3d.com/ScriptReference/Vector3.ProjectOnPlane.html
//Mathf https://docs.unity3d.com/es/530/ScriptReference/Mathf.html
//Quaternion https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Quaternion.html
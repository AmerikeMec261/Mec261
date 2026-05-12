using UnityEngine;
using NaughtyAttributes;
using UnityEditor.Rendering;

public class FinalTurret : MonoBehaviour
{

    //Alcance Máximo de la torret es de 18 - 24 km/h
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField, Required] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private GameObject[] _bulletPrefabs;
    private int _currentBulletIndex = 0;

    [Header("================================================================")]
    [Header("BulletsSpawn")]
    [SerializeField] private Transform[] _bulletSpawns;
    private int _currentSpawnIndex = 0;

    [Header("================================================================")]
    [Header("Targeting")]
    [SerializeField] private float _detectionRadius = 24f;

    [Header("================================================================")]
    [Header("Projectile")]
    [SerializeField] private float _projectileSpeed = 30f;
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private float _maxDistance = 18000f;
    [SerializeField] private bool _useHighArc = false;

    [Header("================================================================")]
    private Transform _currentTarget;
    private bool _hasSolution;

    private void Update()
    {
        //Primero aqui en el Update lo que hace es que aplican las funciones de FindTaregt y Aim
        //Y después aplique lo del disparo que en este caso sera con el Espacio 
        //Y por último lo de cambiar de proyectil con las teclas 1 y 2.

        FindTarget();
        Aim();

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            FireProjectile();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _currentBulletIndex = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            _currentBulletIndex = 1;
    }

    // Aqui hice una funcion en la cual la torreta buscara a al enemigo mas cercano
    //Para ello primero defini un array de Object en la cual consideraba enemigos aquellos objetos que tengan un tag llamdo Enemyu
    // Ya después agregué un float para almacenar la distancia mas cercana como infinito para que detecte a cualquier enemigo y un transform vacio para guardar el enemigo mas cercano

    // Por último, agregamos un foreach para recorrer el array de los enemies y por último calculamos la distancia entre la torreta y cada enemigo en la escena, 
    //Y para poder que detectar a cada uno de ellos por separados, puse un if en la digiera que si la distancia recien calculada esta en el límite o es menor al rango de deteccion de la torreta,
    //entonces guardamos esa información al Transform de closet para que después la podemos almacenar en currentTarget y así poder apuntar a ese enemigo en específico.


    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); 
       
        Transform closest = null; // Transform para almacenar el enemigo más cercano encontrado

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position); //dinstancia entre la torreta y cada enemigo

            if ( distance <= _detectionRadius) //si la dinstacia es igual o menor que el rango de detección de la torreta,
            {

                closest = enemy.transform; //entonces guardamos esa información en nuestro Transform de closet.
            }
        }

        _currentTarget = closest; // Para que después podamos usar esa información para apuntar a ese enemigo en específico.

    }


    //Este sirve para apuntar a u enemigo
    //Este lo que hace es calcular en que angulo debe apuntar la torreta para poder apuntar a un enemigo de manera horizontal y vertical.
    //Tomando en cuenta la posicion de la torreta y el enemigo
    private void Aim()
    {
        if (_currentTarget == null) //Usa el enemigo más cercano que se encontró en la función de FindTarget y lo coloca como nulo.
        return; // Y si no hay ningún enemigo cercano, la torreta se quedará quieta.

        Vector3 originPosition = _bulletSpawns[_currentSpawnIndex].position; // Agarramos la posicion actual del spawn de la bala. 
        Vector3 targetPosition = _currentTarget.position; //Agarramos la posición del enemigo actual. 

        // Calculamos la dirección hacia el enemigo mediante: La posicion de nuestro enemigo actual y la posición de nuestro yawPivot de la torreta.
        Vector3 directionToTarget = targetPosition - _yawPivot.position;
      
        //Y para finalizar pedimos a la torreta que solamente pueda moverse de manera horizontal (x y z) y no en el eje y.
        Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z); //


        // YAW (Rotación horizontal)
        // Aca ponemos un if en la cual diga que si la distancia entre la torreta y el enemigo con que no llegue ser numeros negativos o muy pequeños.
        // Entonces podemos hacemos unos calculas para obtener el angulo correcto mediante la posición de la torreta y el enemigo.

        // El "sqrMagnitude" lo usamos para poder tener una verificación mas simple y rápida,
        // gracias a que este hace calculos con raiz cuadrada, asi podemos conseguir resultados mas rápidos y eficientes.
        if (horizontalDirection.sqrMagnitude > 0.001f) 
        {
            //Para poder saber en que angulo debe usar el YawPivor para apuntar a un enemigo, debemos obtener la rotación en la cual esta mirando la torreta y el enemigo mediante un Quaternion.LookRotation()
            //Con este metódo la torreta puede figar a cualquier enemigo de izquierda a derecha sin problema dependiendo de la posicion de la torreta y el enemigo.
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);


            //Pero el problema es que queremos que el YawPivot tenga un limite de rotación de izquierda a derecha.
            //Asi que agregamos un yawAngle en la cual podemos guardar el ángulo de rotación horizontal (eje Y) con límites.
            float yawAngle = targetRotation.eulerAngles.y;
            if (yawAngle > 180f)
            {
               yawAngle -= 360f;
            }

            // Finalemnte usamos el Mathf.Clamp para limitar el ángulo de rotación de YawPivot entre -90 a 90 grados.
            // El Mathf.Clamp se usa principamente para limitar un valor numerico entre el rango mínimo y el máximo
            // En este caso, puse que el yawAngle no pueda moverse mas alla de los 90 grados de izquierda a derecha
            yawAngle = Mathf.Clamp(yawAngle, -90f, 90f);

            //Entonces ya por último, agregamos esta información del limite de angulo al YawPivot cuando tenga que rotar de izquierda a derecha.
            _yawPivot.rotation = Quaternion.Euler(0f, yawAngle, 0f); 

            // Y ya de resultado la torreta puede apuntar figamente a un enemigo cuando este en el rango de detección, y la torreta siempre va estar fijando al
            //enemigo más cercano pero con un límite de rotacion de izquierda a derecha.

        }

        // PITCH 
        // Ya tenemos una solución para que la torreta pueda apuntar a un enemigo de manera horizontal,
        // Ahora tenemos que calcular en algulo que debe tomar la torrta para apuntar a un enemigo de manera vertical.

        // Primero debemos de calcular el ángulo de lanzamiento parabólico.
        // Para esta solución hice en un void difetente para conseguir el origen del lanzamiento, el objetivo(osea el enemigo) y la velocidad de nuestro proyectil
        // Para que nos entregue el angulo de lanzamiento correcto que es el "launchAngle"
        _hasSolution = SolveBallisticAngle(originPosition, targetPosition, _projectileSpeed, out float launchAngle); 

        //Ya por último, decimos que el void de SolveBallisticAngle nos devuelve el angulo de lanzamiento correctamente.
        if (_hasSolution)
        {
            // Entonces solo decimos que el PitchPivot tome ese ángulo de lanzamiento para apuntar al enemigo de manera vertical.
            // Pero tenemos un problema, launchAngle por determinado esta en radianes y el PitchPivot solo puede trabajar con grados.
            // Asi que creamos un float en la cual podemos convertir el launchAngle a grados con un Mathf.Rad2Deg, en la cual convierte de radianes a grados.
            // Y por útlimo, solo especificamos al PitchPivot gire de manera verticial tomando en cuenta la información del "launchAngleDress".
            float launchAngleDegrees = launchAngle * Mathf.Rad2Deg; 
            _pitchPivot.localEulerAngles = new Vector3(-launchAngleDegrees, 0f, 0f); 
        }
    }

    // En este void tomamos en cuenta el disparo de la torreta.
    private void FireProjectile()
    {
        // Primero, tomamos tomamos en cuenta la posición del spawn de la bala "_currentSpawnIndex" de nuestro transform de bulletSpawns.
        // Por determinado el _currentSpawnIndex esta en 0
        Transform currentSpawn = _bulletSpawns[_currentSpawnIndex];


        // Después instanciamos la Bala en la posición y rotacion del _currentSpawn acutal
        GameObject bulletInstance = Instantiate(_bulletPrefabs[_currentBulletIndex],currentSpawn.position,currentSpawn.rotation);

        // Luego, obtenemos el componente IProjectile de la bala instanciada para poder configurar su velocidad y daño. 
        //El IProjectile es una interfaz par que pueda manejar mejor las diferentes tipos de proyectiles, así pude crear un simplebullet y un explosivebullet.
        IProjectile projectile = bulletInstance.GetComponent<IProjectile>();

        // Y por último, configuramos la velocidad del proyectil y lo dispara con la información de nuestra interfaz IProjectile.
        projectile.SetSpeed(_projectileSpeed);
        projectile.Fire();


        //Con esto la torreta puedes disparar a un enemigo y poderle infiguir daño
        //Pero ahora tenemos que configurar el _currentSpawnIndex
        //Cada vez que la torreta dispare, el _currentSpawnIndex aumentará en 1, para que asi la proxima bala pueda salir en el siguiente bulletSpawns, y así sucesivamente
        _currentSpawnIndex++; 

        //Por útlimo puse un if simple en la cual diga si el _currentSpawIndez haya llegado hasta el final de la lista de los Transforms de los bulletSpawns,
        // Entonces el _currentSpawIndex vuele estar en 0, asi para que pueda seguir  disparando en ciclo cada bulletSpawns.
        if (_currentSpawnIndex >= _bulletSpawns.Length) 
        {
            _currentSpawnIndex = 0;
        }
    }

    // Este void es para calcular el ángulo de lanzamiento parabólico para que la torreta pueda apuntar a un enemigo de manera vertical.
    private bool SolveBallisticAngle(Vector3 originPosition, Vector3 targetPosition, float projectileSpeed, out float launchAngle)
    {
        float gravity = Physics.gravity.magnitude;

        Vector3 horizontalVector = new Vector3(
            targetPosition.x - originPosition.x,
            0f,
            targetPosition.z - originPosition.z
        );

        float horizontalDistance = horizontalVector.magnitude;
        float heightDifference = targetPosition.y - originPosition.y;

        float speedSquared = projectileSpeed * projectileSpeed;
        float speedFourth = speedSquared * speedSquared;

        float discriminant = speedFourth - gravity * (gravity * horizontalDistance * horizontalDistance + 2 * heightDifference * speedSquared);

        if (discriminant < 0f)
        {
            launchAngle = 0f;
            return false;
        }

        float squareRoot = Mathf.Sqrt(discriminant);

        float lowAngle = Mathf.Atan((speedSquared - squareRoot) / (gravity * horizontalDistance));
        float highAngle = Mathf.Atan((speedSquared + squareRoot) / (gravity * horizontalDistance));

        launchAngle = _useHighArc ? highAngle : lowAngle;

        return true;
    }
}
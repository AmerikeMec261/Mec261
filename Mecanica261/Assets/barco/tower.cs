using UnityEngine;

public class tower : MonoBehaviour //Todo debe estar en inglés. 
{
    [Header("Dependencies")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _reticula;

    [Header("Ammo")]
    [SerializeField] private GameObject[] _bulletPrefabs;

    [Header("mortar configuration")]
    [SerializeField] private float _Mortarforce = 15f;
    [SerializeField] private float _maxRange = 1000f;
    [SerializeField] private LayerMask _layersPoint;

    private Transform enemigoObjetivo;

    private void Update()
    {
        
        ApuntarTorreta();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        int indiceAleatorio = Random.Range(0, _bulletPrefabs.Length);

        GameObject GameObject = Instantiate(_bulletPrefabs[indiceAleatorio], _bulletSpawn.position, _bulletSpawn.rotation); // no usar abreviaciones

        Rigidbody Rigibody = GameObject.GetComponent<Rigidbody>(); // no usar abreviaciones
        IProjectile proyectil = GameObject.GetComponent<IProjectile>();

        if (Rigibody != null && proyectil != null)
        {
            Rigibody.useGravity = true; // no usar abreviaciones

            Vector3 objetivo; // no usar abreviaciones

            if (enemigoObjetivo != null)
                objetivo = enemigoObjetivo.position;
            else
                objetivo = _pitchPivot.forward * _maxRange + _bulletSpawn.position;

            Vector3 velocidadFinal = CalcularMortero(_bulletSpawn.position, objetivo, _Mortarforce);

            Rigibody.linearVelocity = velocidadFinal; // no usar abreviaciones

            proyectil.Fire();
        }
    }

    private Vector3 CalcularMortero(Vector3 origen, Vector3 destino, float v)
    {
        Vector3 differential = destino - origen; // no usar abreviaciones
        float x = new Vector2(differential.x, differential.z).magnitude; // no usar abreviaciones
        float y = differential.y; // no usar abreviaciones
        float g = Physics.gravity.magnitude;

        float v2 = v * v;
        float root = v2 * v2 - g * (g * x * x + 2 * y * v2);

        if (root < 0) return differential.normalized * v; // evita que un objeto desaparezca cuando el rayo no encuentra un calculo  devolviendolo a su posicion normalizada

        float angle = Mathf.Atan((v2 + Mathf.Sqrt(root)) / (g * x));

        Vector3 direction = differential;
        direction.y = 0;
        Vector3 velocidadFinal = direction.normalized * v * Mathf.Cos(angle);
        velocidadFinal.y = v * Mathf.Sin(angle);

        return velocidadFinal; // no usar abreviaciones
    }

   /* private void MoverReticulaAlMouse()
    {
        Ray rayMouse = Camera.main.ScreenPointToRay(Input.mousePosition); // no usar abreviaciones
        if (Physics.Raycast(rayMouse, out RaycastHit hit, _maxRange, _layersPoint))       // qué se está evaluando aqui? como lo popdrias sustituir por un operador ternario? (si es que es posible) 
            _reticula.position = hit.point;                                               // calcula si el rayo que esta en la posicion del raton golpea un colisionador dentro de la especificaciones
        else
            _reticula.position = rayMouse.GetPoint(_maxRange / 10);
    }*/

    private void ApuntarTorreta()
    {
        float rangoVision = 20000f; // no usar abreviaciones
        Collider[] enemigosEnRango = Physics.OverlapSphere(transform.position, rangoVision);

        enemigoObjetivo = null;
        float distanciaMinima = Mathf.Infinity;

        foreach (Collider collider in enemigosEnRango)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distancia = Vector3.Distance(transform.position, collider.transform.position);

                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    enemigoObjetivo = collider.transform;
                }
            }
        }

        if (enemigoObjetivo == null) return;

        Vector3 targetYaw = enemigoObjetivo.position;
        targetYaw.y = _yawPivot.position.y;
        _yawPivot.LookAt(targetYaw);

        _pitchPivot.LookAt(enemigoObjetivo.position);
    }

    private void OnDrawalzmossolected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _maxRange);
    }
}
// Trabajo en clase: hacer los cambios de los comentarios (y todos los que apliquen) 
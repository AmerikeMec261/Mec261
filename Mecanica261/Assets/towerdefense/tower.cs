using UnityEngine;

public class tower : MonoBehaviour //Todo debe estar en inglés. 
{
    [Header("Dependencias")]
    [SerializeField] private Transform _yawPivot;
    [SerializeField] private Transform _pitchPivot;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private Transform _reticula;

    [Header("Municion")]
    [SerializeField] private GameObject[] _bulletPrefabs;

    [Header("Configuracion Mortero")]
    [SerializeField] private float _fuerzaMortero = 15f;
    [SerializeField] private float _maxRange = 1000f;
    [SerializeField] private LayerMask _capasApuntado;

    private void Update()
    {
        MoverReticulaAlMouse();
        ApuntarTorreta();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        int indiceAleatorio = Random.Range(0, _bulletPrefabs.Length);

        GameObject go = Instantiate(_bulletPrefabs[indiceAleatorio], _bulletSpawn.position, _bulletSpawn.rotation); // no usar abreviaciones

        Rigidbody rb = go.GetComponent<Rigidbody>(); // no usar abreviaciones
        IProjectile proyectil = go.GetComponent<IProjectile>();

        if (rb != null && proyectil != null)
        {
            rb.useGravity = true; // no usar abreviaciones

            Vector3 velocidadFinal = CalcularMortero(_bulletSpawn.position, _reticula.position, _fuerzaMortero);

            rb.linearVelocity = velocidadFinal; // no usar abreviaciones

            proyectil.Fire();
        }
    }

    private Vector3 CalcularMortero(Vector3 origen, Vector3 destino, float v)
    {
        Vector3 diff = destino - origen; // no usar abreviaciones
        float x = new Vector2(diff.x, diff.z).magnitude; // no usar abreviaciones
        float y = diff.y; // no usar abreviaciones
        float g = Physics.gravity.magnitude;

        float v2 = v * v;
        float raiz = v2 * v2 - g * (g * x * x + 2 * y * v2);

        if (raiz < 0) return diff.normalized * v;

        float angulo = Mathf.Atan((v2 + Mathf.Sqrt(raiz)) / (g * x));

        Vector3 dir = diff;
        dir.y = 0;
        Vector3 velFinal = dir.normalized * v * Mathf.Cos(angulo);
        velFinal.y = v * Mathf.Sin(angulo);

        return velFinal; // no usar abreviaciones
    }

    private void MoverReticulaAlMouse()
    {
        Ray rayoMouse = Camera.main.ScreenPointToRay(Input.mousePosition); // no usar abreviaciones
        if (Physics.Raycast(rayoMouse, out RaycastHit hit, _maxRange, _capasApuntado)) // qué se está evaluando aqui? como lo popdrias sustituir por un operador ternario? (si es que es posible) 
            _reticula.position = hit.point;
        else
            _reticula.position = rayoMouse.GetPoint(_maxRange / 10);
    }

    private void ApuntarTorreta()
    {
        Vector3 targetYaw = _reticula.position;
        targetYaw.y = _yawPivot.position.y;
        _yawPivot.LookAt(targetYaw);

        _pitchPivot.LookAt(_reticula.position);
    }
}// Trabajo en clase: hacer los cambios de los comentarios (y todos los que apliquen) 